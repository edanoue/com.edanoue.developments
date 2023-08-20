// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using UnityEditor;
using UnityEngine;

namespace Edanoue.Editor
{
    [CustomPropertyDrawer(typeof(EdaTagSelectorAttribute))]
    public sealed class EdaTagSelectorAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
                property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}