using UnityEditor;
using UnityEngine;
using ArokaInspector.Attributes;

namespace ArokaInspector.Editors
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (ShouldHide(property))
            {
                return -EditorGUIUtility.standardVerticalSpacing; // 간격을 0으로 설정하여 요소를 숨김
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ShouldHide(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private bool ShouldHide(SerializedProperty property)
        {
            HideIfAttribute hideIf = (HideIfAttribute)attribute;

            // 상대 경로를 통해 조건 프로퍼티 찾기
            string conditionPath = property.propertyPath.Replace(property.name, hideIf.ConditionName);
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionPath);

            if (conditionProperty != null)
            {
                return conditionProperty.boolValue;
            }
            else
            {
                Debug.LogWarning($"Cannot find condition property with name: {hideIf.ConditionName}");
                return false;
            }
        }
    }
}
