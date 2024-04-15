using System.Drawing;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArokaAnim))]
public class UIAnimEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        // 버튼의 너비를 현재 에디터의 반으로 할당
        float buttonWidth = EditorGUIUtility.currentViewWidth / 2 - 10; // 10은 여유 공간
        ArokaAnim uIPositionSetter = (ArokaAnim)target;
        RectTransform rect = uIPositionSetter.GetComponent<RectTransform>();
        if (GUILayout.Button("Register On State", GUILayout.Width(buttonWidth)))
        {

            uIPositionSetter.RegisterStateWithCurrent(true);
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Register Off State", GUILayout.Width(buttonWidth)))
        {
            uIPositionSetter.RegisterStateWithCurrent(false);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Preview On State", GUILayout.Width(buttonWidth)))
        {
            PreviewState(true);
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Preview Off State", GUILayout.Width(buttonWidth)))
        {
            PreviewState(false);
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    private void PreviewState(bool isOn)
    {
        if (Application.isPlaying)
        {
            // 플레이 모드일 때는 애니메이션 시간을 1초로 설정
            ((ArokaAnim)target).SetAnim(isOn, 1f);
        }
        else
        {
            // 플레이 모드가 아닐 때는 즉시 상태 변경 (애니메이션 시간을 0초로 설정)
            ((ArokaAnim)target).SetAnim(isOn, 0f);
        }

        // 변경 사항을 마크하여 에디터가 변경을 인지하도록 함
        EditorUtility.SetDirty(target);
        // 씬 뷰를 다시 그리기 요청
        SceneView.RepaintAll();
    }

}
