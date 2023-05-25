// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using UnityEngine;

namespace Edanoue
{
    partial class EdaDebug
    {
        /// <summary>
        /// <para>Draws a line from start to start + dir in world coordinates also draw on runtime.</para>
        /// </summary>
        /// <param name="start">Point in world space where the ray should start.</param>
        /// <param name="dir">Direction and length of the ray.</param>
        /// <param name="duration">How long the line will be visible for (in seconds). 0 means one frame.</param>
        public static void DrawRayRuntime(in Vector3 start, in Vector3 dir, float duration = 0f)
        {
            if (CheckDrawer())
            {
                _drawer!.RegisterLine(start, start + dir, _defaultColor, 0.0f);
            }
        }

        /// <summary>
        /// <para>Draws a line from start to start + dir in world coordinates also draw on runtime.</para>
        /// </summary>
        /// <param name="start">Point in world space where the ray should start.</param>
        /// <param name="dir">Direction and length of the ray.</param>
        /// <param name="color">Color of the drawn line.</param>
        /// <param name="duration">How long the line will be visible for (in seconds). 0 means one frame.</param>
        public static void DrawRayRuntime(in Vector3 start, in Vector3 dir, in Color color, float duration = 0f)
        {
            if (CheckDrawer())
            {
                _drawer!.RegisterLine(start, start + dir, color, duration);
            }
        }
    }
}