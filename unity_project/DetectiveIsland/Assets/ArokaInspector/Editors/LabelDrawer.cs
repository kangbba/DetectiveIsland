using UnityEditor;
using UnityEngine;
using ArokaInspector.Attributes;

namespace ArokaInspector.Editors
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LabelAttribute labelAttribute = (LabelAttribute)attribute;
            string labelText = labelAttribute.LabelText;

            if (labelText.StartsWith("$"))
            {
                string fieldName = labelText.Substring(1);
                SerializedProperty labelProperty = property.serializedObject.FindProperty(fieldName);

                if (labelProperty != null)
                {
                    labelText = GetLabelText(labelProperty);
                }
                else
                {
                    labelText = $"Field '{fieldName}' not found";
                }
            }

            EditorGUI.LabelField(position, labelText);
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.singleLineHeight;
        }

        private string GetLabelText(SerializedProperty labelProperty)
        {
            switch (labelProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return labelProperty.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return labelProperty.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return labelProperty.floatValue.ToString();
                case SerializedPropertyType.String:
                    return labelProperty.stringValue;
                case SerializedPropertyType.ObjectReference:
                    return labelProperty.objectReferenceValue != null ? labelProperty.objectReferenceValue.name : "None";
                default:
                    return labelProperty.displayName;
            }
        }
    }
}
