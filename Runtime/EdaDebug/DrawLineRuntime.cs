// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using UnityEngine;

namespace Edanoue
{
    partial class EdaDebug
    {
        /// <summary>
        /// <para>Draws a line between specified start and end points in editor and runtime</para>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        public static void DrawLineRuntime(in Vector3 start, in Vector3 end, float duration = 0f)
        {
            if (CheckDrawer())
            {
                _drawer!.RegisterLine(start, end, DefaultColor, 0.0f);
            }
        }


        /// <summary>
        /// <para>Draws a line between specified start and end points in editor and runtime</para>
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public static void DrawLineRuntime(in Vector3 start, in Vector3 end, in Color color, float duration = 0f)
        {
            if (CheckDrawer())
            {
                _drawer!.RegisterLine(start, end, color, duration);
            }
        }
    }
}