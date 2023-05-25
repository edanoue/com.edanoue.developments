// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using Edanoue.Developments.Internal;
using UnityEngine;

namespace Edanoue
{
    /// <summary>
    /// </summary>
    public static partial class EdaDebug
    {
        private static          EdaDebugDrawRuntimeDrawer? _drawer;
        private static readonly Color                      _defaultColor = Color.white;

        private static bool CheckDrawer()
        {
            if (_drawer != null)
            {
                return true;
            }

            var go = new GameObject("__AUTO_GENERATED_DEBUG_DRAWER");
            _drawer = go.AddComponent<EdaDebugDrawRuntimeDrawer>();

            return true;
        }
    }
}