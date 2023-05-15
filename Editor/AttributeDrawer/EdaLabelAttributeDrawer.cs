// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using UnityEditor;
using UnityEngine;

namespace Edanoue.Editor
{
    /// <summary>
    /// </summary>
    [CustomPropertyDrawer(typeof(EdaLabelAttribute))]
    public sealed class EdaLabelAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is EdaLabelAttribute attr)
            {
                var newLabel = label;
                label.text = attr.CustomLabel;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}