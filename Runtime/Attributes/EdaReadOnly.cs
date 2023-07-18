// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using System.Diagnostics;
using UnityEngine;

namespace Edanoue
{
    /// <summary>
    /// Editor からは編集不可能とする Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public sealed class EdaReadOnly : PropertyAttribute
    {
    }
}