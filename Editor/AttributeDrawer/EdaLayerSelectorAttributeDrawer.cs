// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using UnityEditor;
using UnityEngine;

namespace Edanoue.Editor
{
    [CustomPropertyDrawer(typeof(EdaLayerSelectorAttribute))]
    public sealed class EdaLayerSelectorAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.BeginProperty(position, label, property);
                property.intValue = EditorGUI.LayerField(position, label, property.intValue);
                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}