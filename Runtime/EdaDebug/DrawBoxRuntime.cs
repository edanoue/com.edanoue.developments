// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Edanoue
{
    partial class EdaDebug
    {
        /// <summary>
        /// <para>Draws a box in editor and runtime</para>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <param name="duration"></param>
        public static void DrawBoxRuntime(in Vector3 center, float size, float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawCubeInternal(center, Quaternion.identity, size, _defaultColor, duration);
            }
        }

        /// <summary>
        /// <para>Draws a box in editor and runtime</para>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <param name="duration"></param>
        public static void DrawBoxRuntime(in Vector3 center, in Vector3 size, float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawBoxInternal(center, Quaternion.identity, size, _defaultColor, duration);
            }
        }

        /// <summary>
        /// <para>Draws a box in editor and runtime</para>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="duration"></param>
        public static void DrawBoxRuntime(in Vector3 center, in Quaternion rotation, float size, float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawCubeInternal(center, rotation, size, _defaultColor, duration);
            }
        }

        /// <summary>
        /// <para>Draws a box in editor and runtime</para>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="duration"></param>
        public static void DrawBoxRuntime(in Vector3 center, in Quaternion rotation, in Vector3 size,
            float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawBoxInternal(center, rotation, size, _defaultColor, duration);
            }
        }

        /// <summary>
        /// <para>Draws a box in editor and runtime</para>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public static void DrawBoxRuntime(in Vector3 center, float size, in Color color, float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawCubeInternal(center, Quaternion.identity, size, color, duration);
            }
        }

        /// <summary>
        /// <para>Draws a box in editor and runtime</para>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public static void DrawBoxRuntime(in Vector3 center, in Vector3 size, in Color color, float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawBoxInternal(center, Quaternion.identity, size, color, duration);
            }
        }

        /// <summary>
        /// <para>Draws a box in editor and runtime</para>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public static void DrawBoxRuntime(in Vector3 center, in Quaternion rotation, float size, in Color color,
            float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawCubeInternal(center, rotation, size, color, duration);
            }
        }

        /// <summary>
        /// <para>Draws a box in editor and runtime</para>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public static void DrawBoxRuntime(in Vector3 center, in Quaternion rotation, in Vector3 size, in Color color,
            float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawBoxInternal(center, rotation, size, color, duration);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawBoxInternal(in Vector3 center, in Quaternion rotation, in Vector3 size, in Color color,
            float duration)
        {
            // Get axis direction from rotation
            var up = rotation * Vector3.up;
            var forward = rotation * Vector3.forward;
            var right = rotation * Vector3.right;

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
            _drawer!.RegisterLine(pos0, pos1, color, duration);
            _drawer.RegisterLine(pos1, pos2, color, duration);
            _drawer.RegisterLine(pos2, pos3, color, duration);
            _drawer.RegisterLine(pos3, pos0, color, duration);
            _drawer.RegisterLine(pos0, pos4, color, duration);
            _drawer.RegisterLine(pos1, pos5, color, duration);
            _drawer.RegisterLine(pos2, pos6, color, duration);
            _drawer.RegisterLine(pos3, pos7, color, duration);
            _drawer.RegisterLine(pos4, pos5, color, duration);
            _drawer.RegisterLine(pos5, pos6, color, duration);
            _drawer.RegisterLine(pos6, pos7, color, duration);
            _drawer.RegisterLine(pos7, pos4, color, duration);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawCubeInternal(in Vector3 center, in Quaternion rotation, float size, in Color color,
            float duration)
        {
            // Get axis direction from rotation
            var up = rotation * Vector3.up;
            var forward = rotation * Vector3.forward;
            var right = rotation * Vector3.right;

            // Calculate 8 points
            var pos0 = center - size * 0.5f * (up + forward + right);
            var pos1 = pos0 + size * right;
            var pos2 = pos1 + size * forward;
            var pos3 = pos2 - size * right;
            var pos4 = pos0 + size * up;
            var pos5 = pos4 + size * right;
            var pos6 = pos5 + size * forward;
            var pos7 = pos6 - size * right;

            // Draw edge lines
            _drawer!.RegisterLine(pos0, pos1, color, duration);
            _drawer.RegisterLine(pos1, pos2, color, duration);
            _drawer.RegisterLine(pos2, pos3, color, duration);
            _drawer.RegisterLine(pos3, pos0, color, duration);
            _drawer.RegisterLine(pos0, pos4, color, duration);
            _drawer.RegisterLine(pos1, pos5, color, duration);
            _drawer.RegisterLine(pos2, pos6, color, duration);
            _drawer.RegisterLine(pos3, pos7, color, duration);
            _drawer.RegisterLine(pos4, pos5, color, duration);
            _drawer.RegisterLine(pos5, pos6, color, duration);
            _drawer.RegisterLine(pos6, pos7, color, duration);
            _drawer.RegisterLine(pos7, pos4, color, duration);
        }
    }
}