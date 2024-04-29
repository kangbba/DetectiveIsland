using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;




[System.Serializable]
public class StartNode : Node
{
    public StartNode(Vector2 pos, string title) : base(title)
    {
        UpdateNodeSize(new Vector2(200, 100));
        UpdateNodePosition(pos);
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.startNodeColor;

        base.DrawNode(offset);

        Rect rect = Rect;

        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);

        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(Rect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = representColor;
        GUI.Box(rect.AdjustSize(2, 2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(rect, "", boxGS);

        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(rect.ModifiedY(rect.y + 30), title, titleGS);

        GUI.color = Color.white;



        DrawConnectionPoints(representColor, false, true);
    }
}
