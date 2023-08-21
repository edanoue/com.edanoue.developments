// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using UnityEditor;
using UnityEngine;

namespace Edanoue.Editor
{
    [CustomPropertyDrawer(typeof(EdaSceneObject))]
    public sealed class EdaSceneObjectPropertyDrawer : PropertyDrawer
    {
        private const string _PROPERTY_NAME = "m_sceneName";

        private static SceneAsset? GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
            {
                return null;
            }

            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.path.IndexOf(sceneObjectName, StringComparison.Ordinal) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath(scene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }

            Debug.Log(
                $"Scene [{sceneObjectName}] cannot be used. Add this scene to the 'Scenes in the Build' in the build settings.");
            return null;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var sceneObj = GetSceneObject(property.FindPropertyRelative(_PROPERTY_NAME).stringValue);
            var newScene = EditorGUI.ObjectField(position, label, sceneObj, typeof(SceneAsset), false);
            if (newScene is null)
            {
                var prop = property.FindPropertyRelative(_PROPERTY_NAME);
                prop.stringValue = "";
            }
            else
            {
                if (newScene.name != property.FindPropertyRelative(_PROPERTY_NAME).stringValue)
                {
                    var scnObj = GetSceneObject(newScene.name);
                    if (scnObj is null)
                    {
                        Debug.LogWarning(
                            $"The scene {newScene.name} cannot be used. To use this scene add it to the build settings for the project.");
                    }
                    else
                    {
                        var prop = property.FindPropertyRelative(_PROPERTY_NAME);
                        prop.stringValue = newScene.name;
                    }
                }
            }

            EditorGUI.EndProperty();
        }
    }
}