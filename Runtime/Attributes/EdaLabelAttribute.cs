// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using System.Diagnostics;
using UnityEngine;

namespace Edanoue
{
    /// <summary>
    /// Unity の Inspector に表示される名前を上書きする Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public class EdaLabelAttribute : PropertyAttribute
    {
        public readonly string CustomLabel;

        public EdaLabelAttribute(string text)
        {
            CustomLabel = text;
        }
    }
}