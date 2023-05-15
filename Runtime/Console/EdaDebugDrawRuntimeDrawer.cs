// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Edanoue
{
    [DisallowMultipleComponent]
    internal sealed class EdaDebugDrawRuntimeDrawer : MonoBehaviour
    {
        private const string _SHADER_NAME    = "Hidden/Edanoue/Debug/DebugLineZTestEnable";
        private const int    _MAX_LINE_COUNT = 1000;

        private readonly DrawLineEntry[] _lineEntries     = new DrawLineEntry[_MAX_LINE_COUNT];
        private          BatchedLineDraw _batchedLineDraw = null!;
        private          bool            _isNeedRebuild;

        private void Awake()
        {
            // (南) Mesh の new のタイミングの関係上, Awake で初期化する必要があります
            _batchedLineDraw = new BatchedLineDraw();
        }

        private void LateUpdate()
        {
            if (_isNeedRebuild)
            {
                RebuildDrawLine();
                _isNeedRebuild = false;
            }

            var rParams = new RenderParams(_batchedLineDraw.Mat)
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
                _batchedLineDraw.Mesh,
                0,
                Matrix4x4.identity
            );

            foreach (ref var lineEntry in _lineEntries.AsSpan())
            {
                if (!lineEntry.Occupied)
                {
                    continue;
                }

                lineEntry.Timer -= Time.deltaTime;

                if (lineEntry.Timer >= 0)
                {
                    continue;
                }

                lineEntry.Occupied = false;
                _isNeedRebuild = true;
            }
        }

        private void OnDestroy()
        {
            _batchedLineDraw.Dispose();
        }

        internal void RegisterLine(in Vector3 start, in Vector3 end, in Color color, float duration)
        {
            foreach (ref var lineEntry in _lineEntries.AsSpan())
            {
                if (lineEntry.Occupied)
                {
                    continue;
                }

                lineEntry.Occupied = true;
                lineEntry.Start = start;
                lineEntry.End = end;
                lineEntry.Color = color;
                lineEntry.Timer = Mathf.Max(duration, 0f);

                _isNeedRebuild = true;
                break;
            }
        }

        private void RebuildDrawLine()
        {
            _batchedLineDraw.Clear();

            foreach (ref readonly var lineEntry in _lineEntries.AsSpan())
            {
                if (lineEntry.Occupied)
                {
                    _batchedLineDraw.AddLine(lineEntry.Start, lineEntry.End, lineEntry.Color);
                }
            }

            _batchedLineDraw.BuildMesh();
        }

        private struct DrawLineEntry
        {
            public bool    Occupied;
            public Vector3 Start;
            public Vector3 End;
            public Color   Color;
            public float   Timer;
        }

        private sealed class BatchedLineDraw : IDisposable
        {
            private readonly List<Color>   _colors   = new();
            private readonly List<int>     _indices  = new();
            private readonly List<Vector3> _vertices = new();
            public readonly  Material      Mat;
            public readonly  Mesh          Mesh;

            public BatchedLineDraw()
            {
                var shader = Shader.Find(_SHADER_NAME);
                if (shader == null)
                {
                    throw new ApplicationException($"Failed to find shader: {_SHADER_NAME}");
                }

                Mat = new Material(shader);
                Mesh = new Mesh();
                Mesh.MarkDynamic();
            }

            public void Dispose()
            {
                DestroyImmediate(Mesh);
                DestroyImmediate(Mat);
            }

            public void AddLine(in Vector3 from, in Vector3 to, Color color)
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

            public void BuildMesh()
            {
                Mesh.SetVertices(_vertices);
                Mesh.SetColors(_colors);
                Mesh.SetIndices(_indices.ToArray(), MeshTopology.Lines, 0);
            }
        }
    }
}