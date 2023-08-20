// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using System.Diagnostics;
using UnityEngine;

namespace Edanoue
{
    /// <summary>
    /// Inspector で単一 Layer を選択するためのアトリビュート(公式で用意しろ)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public sealed class EdaTagSelectorAttribute : PropertyAttribute
    {
    }
}