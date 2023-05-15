// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Edanoue
{
    partial class EdaDebug
    {
        /// <summary>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="duration"></param>
        public static void DrawSphereRuntime(in Vector3 center, float radius, float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawCircle(0, center, Quaternion.identity, radius, DefaultColor, duration);
                DrawCircle(1, center, Quaternion.identity, radius, DefaultColor, duration);
                DrawCircle(2, center, Quaternion.identity, radius, DefaultColor, duration);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="rotation"></param>
        /// <param name="radius"></param>
        /// <param name="duration"></param>
        public static void DrawSphereRuntime(in Vector3 center, in Quaternion rotation, float radius,
            float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawCircle(0, center, rotation, radius, DefaultColor, duration);
                DrawCircle(1, center, rotation, radius, DefaultColor, duration);
                DrawCircle(2, center, rotation, radius, DefaultColor, duration);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public static void DrawSphereRuntime(in Vector3 center, float radius, in Color color, float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawCircle(0, center, Quaternion.identity, radius, color, duration);
                DrawCircle(1, center, Quaternion.identity, radius, color, duration);
                DrawCircle(2, center, Quaternion.identity, radius, color, duration);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="center"></param>
        /// <param name="rotation"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public static void DrawSphereRuntime(in Vector3 center, in Quaternion rotation, float radius, in Color color,
            float duration = 0f)
        {
            if (CheckDrawer())
            {
                DrawCircle(0, center, rotation, radius, color, duration);
                DrawCircle(1, center, rotation, radius, color, duration);
                DrawCircle(2, center, rotation, radius, color, duration);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawCircle(int axis, in Vector3 center, in Quaternion rotation, float radius,
            in Color color, float duration)
        {
            const int division = 8;
            const float pi2 = Mathf.PI * 2;
            var prevPos = Vector3.zero;
            for (var i = 0; i <= division; i++)
            {
                var alpha = i / (float)division;
                var x = Mathf.Sin(pi2 * alpha);
                var y = Mathf.Cos(pi2 * alpha);

                var currentPos = axis switch
                {
                    // X
                    0 => new Vector3(0, x * radius, y * radius),
                    // Y
                    1 => new Vector3(x * radius, 0, y * radius),
                    _ => new Vector3(x * radius, y * radius, 0)
                };

                currentPos = rotation * currentPos;
                currentPos += center;
                if (i > 0)
                {
                    _drawer!.RegisterLine(prevPos, currentPos, color, duration);
                }

                prevPos = currentPos;
            }
        }
    }
}