using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

// ReSharper disable StringLiteralTypo

namespace Edanoue.Editor
{
    /// <summary>
    /// .lineobj 形式のアセットを Mesh としてインポートする
    /// </summary>
    [ScriptedImporter(1, "lineobj")]
    public class LineObjImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var lastSlash = ctx.assetPath.LastIndexOf('/');
            var lastDot = ctx.assetPath.LastIndexOf('.');
            var assetName = ctx.assetPath.Substring(lastSlash + 1, lastDot - lastSlash - 1);

            var mainAsset = new GameObject();

            var obj = ParseObj(ctx.assetPath);
            var mesh = ConstructMesh(obj, out var triangleSubmeshExists);
            if (mesh == null)
            {
                return;
            }

            mesh.name = assetName;

            // Add Mesh Filter
            var meshFilter = mainAsset.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            ctx.AddObjectToAsset("Mesh", mesh);
            /*
            var standard = Shader.Find("Standard");
            Material[] materials = new Material[mesh.subMeshCount];
            if (triangleSubmeshExists && mesh.subMeshCount == 2)
            {
                materials[0] = new Material(standard);
                materials[0].name = "Face Material";
                materials[1] = new Material(standard);
                materials[1].name = "Edge Material";
                ctx.AddObjectToAsset("Face Material", materials[0]);
                ctx.AddObjectToAsset("Edge Material", materials[1]);
            }
            else if (triangleSubmeshExists && mesh.subMeshCount == 1)
            {
                materials[0] = new Material(standard);
                materials[0].name = "Face Material";
                ctx.AddObjectToAsset("Face Material", materials[0]);
            }
            else if (!triangleSubmeshExists && mesh.subMeshCount == 1)
            {
                materials[0] = new Material(standard);
                materials[0].name = "Edge Material";
                ctx.AddObjectToAsset("Edge Material", materials[0]);
            }
            */

            var renderer = mainAsset.AddComponent<MeshRenderer>();
            // renderer.materials = materials;

            ctx.AddObjectToAsset(assetName, mainAsset);
            ctx.SetMainObject(mainAsset);
        }

        private Mesh ConstructMesh(IReadOnlyDictionary<string, List<string[]>> data, out bool triangleSubmeshExists)
        {
            Mesh result;
            var f = data["f"];
            var e = data["e"];
            triangleSubmeshExists = false;

            if (f.Count > 0 && e.Count > 0)
            {
                result = new Mesh
                {
                    subMeshCount = 2
                };
                triangleSubmeshExists = true;
            }
            else if (f.Count > 0)
            {
                result = new Mesh
                {
                    subMeshCount = 1
                };
                triangleSubmeshExists = true;
            }
            else if (e.Count > 0)
            {
                result = new Mesh
                {
                    subMeshCount = 1
                };
            }
            else
            {
                return null;
            }

            var v = data["v"];
            var vertices = new Vector3[v.Count];
            for (var i = 0; i < v.Count; i++)
            {
                var raw = v[i];

                var x = float.Parse(raw[0]);
                var y = float.Parse(raw[1]);
                var z = float.Parse(raw[2]);
                vertices[i] = new Vector3(x, y, z);
            }

            result.vertices = vertices;

            // subMesh 0 is like a regular mesh which uses MeshTopology.Triangles
            if (f.Count > 0)
            {
                var triangleIndices = new int[f.Count * 3];
                for (var i = 0; i < f.Count; i++)
                {
                    var raw = f[i];
                    var s1 = raw[0];
                    var s2 = raw[1];
                    var s3 = raw[2];
                    if (s1.Contains("//"))
                    {
                        s1 = s1.Remove(s1.IndexOf("//", StringComparison.Ordinal));
                    }

                    if (s2.Contains("//"))
                    {
                        s2 = s2.Remove(s2.IndexOf("//", StringComparison.Ordinal));
                    }

                    if (s3.Contains("//"))
                    {
                        s3 = s3.Remove(s3.IndexOf("//", StringComparison.Ordinal));
                    }

                    var v1 = int.Parse(s1) - 1;
                    var v2 = int.Parse(s2) - 1;
                    var v3 = int.Parse(s3) - 1;
                    triangleIndices[i * 3] = v1;
                    triangleIndices[i * 3 + 1] = v2;
                    triangleIndices[i * 3 + 2] = v3;
                }

                result.SetIndices(triangleIndices, MeshTopology.Triangles, 0, false);
                result.RecalculateNormals();
            }

            // subMesh 1 is the line mesh which uses MeshTopology.Lines
            if (e.Count > 0)
            {
                var edgeIndices = new int[e.Count * 2];
                for (var i = 0; i < e.Count; i++)
                {
                    var raw = e[i];
                    var v1 = int.Parse(raw[0]) - 1;
                    var v2 = int.Parse(raw[1]) - 1;
                    edgeIndices[i * 2] = v1;
                    edgeIndices[i * 2 + 1] = v2;
                }

                result.SetIndices(edgeIndices, MeshTopology.Lines, triangleSubmeshExists ? 1 : 0, false);
            }

            result.RecalculateBounds();
            return result;
        }

        /*
        Converts obj text file into json-like structure:
            {v: [], vn: [], f: [], e: []}
         */
        private static Dictionary<string, List<string[]>> ParseObj(string filepath)
        {
            var result = new Dictionary<string, List<string[]>>();
            var v = new List<string[]>();
            var vn = new List<string[]>();
            var f = new List<string[]>();
            var e = new List<string[]>();

            using var sr = File.OpenText(filepath);
            var s = string.Empty;
            string[] line;
            while ((s = sr.ReadLine()) != null)
            {
                if (s.StartsWith("v "))
                {
                    line = s.Split(' ');
                    string[] lineData = { line[1], line[2], line[3] };
                    v.Add(lineData);
                }
                else if (s.StartsWith("vn "))
                {
                    line = s.Split(' ');
                    string[] lineData = { line[1], line[2], line[3] };
                    vn.Add(lineData);
                }
                else if (s.StartsWith("f "))
                {
                    line = s.Split(' ');
                    if (line.Length > 4)
                    {
                        Debug.LogError("Your model must be exported with triangulated faces.");
                        continue;
                    }

                    string[] lineData = { line[1], line[2], line[3] };
                    f.Add(lineData);
                }
                else if (s.StartsWith("l "))
                {
                    line = s.Split(' ');
                    string[] lineData = { line[1], line[2] };
                    e.Add(lineData);
                }
            }

            result.Add("v", v);
            result.Add("vn", vn);
            result.Add("f", f);
            result.Add("e", e);
            return result;
        }

        /*
        // for debugging
        private void LogObj(Dictionary<string, List<string[]>> obj)
        {
            var result = "";
            result += "{\n";
            result += LogChild(obj, "v");
            result += LogChild(obj, "vn");
            result += LogChild(obj, "f");
            result += LogChild(obj, "e");

            result += "}";
            Debug.Log(result);
        }

        private string LogChild(Dictionary<string, List<string[]>> obj, string key)
        {
            var result = "";
            const string indent = "\t";
            result += indent + key + ": [\n";
            foreach (var sarr in obj[key])
            {
                result += indent + indent + "[";
                result = sarr.Aggregate(result, (current, s) => current + (s + ", "));
                result += "]\n";
            }

            result += indent + "]\n";
            return result;
        }
        */
    }
}