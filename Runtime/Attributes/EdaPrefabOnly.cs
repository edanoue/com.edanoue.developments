// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using System.Diagnostics;
using UnityEngine;

namespace Edanoue
{
    /// <summary>
    /// Prefab Mode のときのみ表示される Property を設定するための Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public sealed class EdaPrefabOnly : PropertyAttribute
    {
    }
}