// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using UnityEditor;
using UnityEngine;

namespace Edanoue.Editor
{
    [CustomPropertyDrawer(typeof(EdaLayerAttribute))]
    public sealed class EdaLayerAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }
    }
}