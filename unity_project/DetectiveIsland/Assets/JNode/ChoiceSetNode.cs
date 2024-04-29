using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[System.Serializable]
public class ChoiceSetNode : Node
{
    public ChoiceSet choiceSet;

    public ChoiceSetNode(Vector2 pos, string title) : base(title)
    {
        UpdateNodePosition(pos);
        UpdateNodeSize(Vector2.one * 100);
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.choiceSetColor;
        base.DrawNode(offset);

        DrawSelectionBox(Rect, representColor);
        DrawBackground(Rect);
        DrawTitle(Rect, title);

        DrawConnectionPoints(representColor, true, true);
    }


    private void DrawSelectionBox(Rect nodeRect, Color representColor)
    {
        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = representColor;
        GUI.Box(nodeRect.AdjustSize(2, 2), "", boxGS);
    }

    private void DrawBackground(Rect nodeRect)
    {
        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeRect, "", boxGS);
        GUI.color = Color.white;
    }

    private void DrawTitle(Rect nodeRect, string title)
    {
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeRect.ModifiedY(nodeRect.y + 30), title, titleGS);
    }

}