// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using UnityEditor;
using UnityEngine;

namespace Edanoue.Editor
{
    /// <summary>
    /// EH_ImplementedInterfaceAttribute が付与されているフィールドに設定できる Object を制限するための Unity 拡張クラスです
    /// </summary>
    [CustomPropertyDrawer(typeof(EdaImplementedInterfaceAttribute))]
    public sealed class EdaImplementedInterfaceAttributeDrawer : PropertyDrawer
    {
        // Inspector に エラーメッセージを表示するための内部用の関数
        private static void ShowErrorMessage(Rect position, GUIContent label, string message)
        {
            // If field is not ObjectReference, show error message.
            // Save previous color and change GUI to red.
            var previousColor = GUI.color;
            GUI.color = Color.red;
            // Display label with error message.
            EditorGUI.LabelField(position, label, new GUIContent(message));
            // Revert color change.
            GUI.color = previousColor;
        }

        // OnGUI を Override して 設定できる型に制限をしています
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var propertyType = property.propertyType;
            if (propertyType == SerializedPropertyType.ObjectReference)
            {
                if (attribute is EdaImplementedInterfaceAttribute restriction)
                {
                    var targetType = restriction.Type;
                    if (targetType.IsInterface)
                    {
                        EditorGUI.ObjectField(
                            position,
                            property,
                            targetType, // このフィールドに割り当てることができる型を束縛する
                            label
                        );
                    }
                    // アトリビュートで設定されている型が Interface でない場合はエラーを表示する
                    // 南: 実はあまり制限する意味はないのですが, アトリビュート名と期待する動作が一致したほうが問題がおきにくそうなので
                    //     こういったチェックを置いています
                    else
                    {
                        ShowErrorMessage(position, label, $"{targetType} is not interface.");
                    }
                }
                // キャストに失敗した場合 (南: 発生しないですがフローをちゃんと明確化するために置いています)
                else
                {
                    ShowErrorMessage(position, label,
                        $"attribute is not a {nameof(EdaImplementedInterfaceAttribute)} implemented");
                }
            }
            // SerializedPropertyType.ObjectReference ではなかったときにエラー表示する
            // これは値型 例えば int とかに EH_ImplementedInterfaceAttribute が置かれた場合にエラーが表示されます
            else
            {
                ShowErrorMessage(position, label, "Property is not a reference type");
            }
        }
    }
}