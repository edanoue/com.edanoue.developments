// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Edanoue.Developments
{
    [DisallowMultipleComponent]
    public class Eda3dStickyNote : MonoBehaviour
    {
        [SerializeField]
        private Vector3 m_size = new(1, 1, 1);

        [SerializeField]
        private Color m_color = Color.green;

        [SerializeField]
        [Range(0.01f, 5f)]
        private float m_textScale = 1f;

        [SerializeField]
        [Multiline(5)]
        private string m_text = "サンプル\nText";

        [SerializeField]
        [EdaPrefabOnly]
        private Shader m_shader = null!;

        [SerializeField]
        [EdaPrefabOnly]
        private TextMesh m_textMesh = null!;

        private BatchedBoxDraw _drawer = null!;

        private void Awake()
        {
            _drawer = new BatchedBoxDraw(Vector3.zero, m_size, m_color, m_shader);
        }

        private void LateUpdate()
        {
            var rParams = new RenderParams(_drawer.Mat)
            {
                camera = null,
                layer = 0,
                lightProbeProxyVolume = null,
                lightProbeUsage = LightProbeUsage.Off,
                motionVectorMode = MotionVectorGenerationMode.ForceNoMotion,
                receiveShadows = false,
                reflectionProbeUsage = ReflectionProbeUsage.Off,
                shadowCastingMode = ShadowCastingMode.Off
            };

            Graphics.RenderMesh(
                rParams,
                _drawer.Mesh,
                0,
                transform.localToWorldMatrix
            );
        }

        private void OnDestroy()
        {
            _drawer.Dispose();
        }

        private sealed class BatchedBoxDraw : IDisposable
        {
            private readonly List<Color>   _colors   = new();
            private readonly List<int>     _indices  = new();
            private readonly List<Vector3> _vertices = new();
            public readonly  Material      Mat;
            public readonly  Mesh          Mesh;

            public BatchedBoxDraw(in Vector3 center, in Vector3 size, in Color color, Shader shader)
            {
                Mat = new Material(shader);
                Mesh = new Mesh();
                Mesh.MarkDynamic();
                BuildCubeMesh(center, size, color);
            }

            public void Dispose()
            {
                DestroyImmediate(Mesh);
                DestroyImmediate(Mat);
            }

            private void AddLine(in Vector3 from, in Vector3 to, Color color)
            {
                _vertices.Add(from);
                _vertices.Add(to);
                _colors.Add(color);
                _colors.Add(color);
                var verticesCount = _vertices.Count;
                _indices.Add(verticesCount - 2);
                _indices.Add(verticesCount - 1);
            }

            public void Clear()
            {
                Mesh.Clear();
                _vertices.Clear();
                _colors.Clear();
                _indices.Clear();
            }

            private void BuildCubeMesh(in Vector3 center, in Vector3 size, in Color color)
            {
                // Get axis direction from rotation
                var up = Vector3.up;
                var forward = Vector3.forward;
                var right = Vector3.right;

                // Calculate 8 points
                var pos0 = center - size.y * 0.5f * up - size.z * 0.5f * forward - size.x * 0.5f * right;
                var pos1 = pos0 + size.x * right;
                var pos2 = pos1 + size.z * forward;
                var pos3 = pos2 - size.x * right;
                var pos4 = pos0 + size.y * up;
                var pos5 = pos4 + size.x * right;
                var pos6 = pos5 + size.z * forward;
                var pos7 = pos6 - size.x * right;

                // Draw edge lines
                AddLine(pos0, pos1, color);
                AddLine(pos1, pos2, color);
                AddLine(pos2, pos3, color);
                AddLine(pos3, pos0, color);
                AddLine(pos0, pos4, color);
                AddLine(pos1, pos5, color);
                AddLine(pos2, pos6, color);
                AddLine(pos3, pos7, color);
                AddLine(pos4, pos5, color);
                AddLine(pos5, pos6, color);
                AddLine(pos6, pos7, color);
                AddLine(pos7, pos4, color);

                Mesh.SetVertices(_vertices);
                Mesh.SetColors(_colors);
                Mesh.SetIndices(_indices.ToArray(), MeshTopology.Lines, 0);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            m_textMesh.color = m_color;
            m_textMesh.text = m_text;
            m_textMesh.transform.localScale = m_textScale * 0.01f * new Vector3(1f, 1f, 1f);
        }

        /// <summary>
        /// エディタで Radius と Center のプレビューを行う Gizmo の描写
        /// </summary>
        private void OnDrawGizmos()
        {
            var cache = Gizmos.matrix;
            var thisTransform = transform;
            Gizmos.matrix = Matrix4x4.TRS(thisTransform.position, thisTransform.rotation, thisTransform.lossyScale);
            Gizmos.color = m_color;
            Gizmos.DrawWireCube(Vector3.zero, m_size);
            Gizmos.matrix = cache;
        }
#endif
    }
}