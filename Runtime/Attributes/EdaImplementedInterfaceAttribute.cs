// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable

using System;
using System.Diagnostics;
using UnityEngine;

namespace Edanoue
{
    /// <summary>
    /// 特定の Interface を実装していることを 強制するための Unity Inspector 公開 Property 向けの Attribute です
    /// </summary>
    /// <remarks>https://scrapbox.io/edanoue/特定のインタフェースを実装した_MonoBehaviour_のみを制限するアトリビュートをつくる</remarks>
    /// <example>
    /// <code>
    /// [SerializeField]
    /// [EH_ImplementedInterfaceAttribute(typeof(IMyInterface))]
    /// MonoBehaviour m_param = null!; // Type is MonoBehaviour
    /// IMyInterface _param => m_param as IMyInterface ??
    ///     throw new NotImplementedException($"{nameof(m_param)} is not implemented {nameof(IMyInterface)}");
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public sealed class EdaImplementedInterfaceAttribute : PropertyAttribute
    {
        public readonly Type Type = null!;

        public EdaImplementedInterfaceAttribute(Type? type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}