using UnityEditor;
using UnityEngine;
using ArokaInspector.Attributes;

namespace ArokaInspector.Editors
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing; // 간격을 0으로 설정하여 요소를 숨김
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private bool ShouldShow(SerializedProperty property)
        {
            ShowIfAttribute showIf = (ShowIfAttribute)attribute;

            // 상대 경로를 통해 조건 프로퍼티 찾기
            string conditionPath = GetConditionPath(property, showIf.ConditionName);
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionPath);

            if (conditionProperty != null)
            {
                switch (conditionProperty.propertyType)
                {
                    case SerializedPropertyType.Boolean:
                        return showIf.HasConditionValue ? conditionProperty.boolValue.Equals(showIf.ConditionValue) : conditionProperty.boolValue;
                    case SerializedPropertyType.Enum:
                        return showIf.HasConditionValue ? conditionProperty.enumValueIndex.Equals((int)showIf.ConditionValue) : true;
                    default:
                        Debug.LogWarning($"Unsupported property type: {conditionProperty.propertyType}");
                        return true;
                }
            }
            else
            {
                Debug.LogWarning($"Cannot find condition property with name: {showIf.ConditionName}");
                return true;
            }
        }

        private string GetConditionPath(SerializedProperty property, string conditionName)
        {
            string[] propertyPath = property.propertyPath.Split('.');
            propertyPath[propertyPath.Length - 1] = conditionName;
            return string.Join(".", propertyPath);
        }
    }
}
