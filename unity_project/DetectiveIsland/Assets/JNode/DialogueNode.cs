using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[System.Serializable]
public class DialogueNode : Node
{
    public Dialogue dialogue;
    public float LineWidth => nodeSize.x;
    public float LineHeight = 70;
    public float LineVeritcalDist = 5;

    public DialogueNode(Vector2 initialPos, string title) : base(title)
    {
        base.UpdateNodePosition(initialPos);
        base.UpdateNodeSize(CalNodeSize());
    }
    public void DrawBackground(Rect nodeTotalRect)
    {
        GUIStyle boxGS = new GUIStyle();
        boxGS.normal.background = EditorGUIUtility.whiteTexture;
        boxGS.alignment = TextAnchor.UpperCenter;
        boxGS.padding = new RectOffset(10, 10, 10, 10);
        if (isSelected)
        {
            GUI.color = NodeColor.selectedColor;
            GUI.Box(nodeTotalRect.AdjustSize(10, 10), "", boxGS);
        }

        GUI.color = NodeColor.dialogueColor;
        GUI.Box(nodeTotalRect.AdjustSize(2, 2), "", boxGS);

        GUI.color = NodeColor.nodeBackgroundColor;
        GUI.Box(nodeTotalRect, "", boxGS);
    }
    public void DrawTitle(Rect nodeTotalRect)
    {

        //타이틀
        GUI.color = Color.white;
        GUIStyle titleGS = new GUIStyle();
        titleGS.alignment = TextAnchor.UpperCenter;
        titleGS.normal.textColor = Color.white;
        GUI.Label(nodeTotalRect.ModifiedY(nodeTotalRect.y + 30), title, titleGS);
    }

    public void DrawCharacterType(Rect nodeTotalRect)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = 10,
            normal = { textColor = Color.white }
        };
        GUIContent labelContent = new GUIContent("Character Type:");
        Vector2 labelSize_CharacterName = labelStyle.CalcSize(labelContent);
        GUI.Label(new Rect(nodeTotalRect.x, nodeTotalRect.y + 60, labelSize_CharacterName.x, 20), labelContent, labelStyle);
        // Character Name Input Field
        dialogue.CharacterID = GUI.TextField(new Rect(nodeTotalRect.x + labelSize_CharacterName.x + 5, nodeTotalRect.y + 60, nodeTotalRect.width - labelSize_CharacterName.x - 10, 20), dialogue.CharacterID);
    }


    private void DrawAddLineButton(Rect nodeRect)
    {
        float buttonYPosition = nodeRect.y + 105 + (dialogue.Lines.Count * (LineHeight + LineVeritcalDist));
        Rect buttonRect = new Rect(
            nodeRect.x + (nodeRect.width - LineWidth) / 2,
            buttonYPosition,
            LineWidth,
            LineHeight
        );

        if (GUI.Button(buttonRect, "Add Line"))
            OnPlusButtonClicked();
    }

    private void DrawLines(Rect nodeRect)
    {
        float yPos = nodeRect.y + 105;
        for (int i = 0; i < dialogue.Lines.Count; i++)
        {
            Rect lineRect = new Rect(nodeRect.x + (nodeRect.width - LineWidth) / 2, yPos + (i * (LineHeight + LineVeritcalDist)), LineWidth, LineHeight);
            GUIStyle lineLabelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = 10,
                normal = { textColor = Color.yellow, background = Texture.GetBoxTexture(Color.yellow * Color.gray * Color.gray) }
            };
            GUI.Box(lineRect, "Line", lineLabelStyle);
            DrawLineContent(i, lineRect);
        }
    }

    private void DrawLineContent(int index, Rect lineRect)
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperCenter,
            fontSize = 10,
            normal = { textColor = Color.white }
        };
        GUIStyle textFieldStyle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = Color.white, background = Texture.GetBoxTexture(Color.gray * 0.25f) },
            fontSize = 10
        };
        float initialLineContentsOffsetY = 20;

        Line line = dialogue.Lines[index];
        EditorGUI.LabelField(new Rect(lineRect.x + 5, lineRect.y + initialLineContentsOffsetY, 80, 20), "Emotion ID:", labelStyle);
        line.EmotionID = EditorGUI.TextField(new Rect(lineRect.x + 85, lineRect.y + initialLineContentsOffsetY, 150, 20), line.EmotionID);

        EditorGUI.LabelField(new Rect(lineRect.x + 5, lineRect.y + +initialLineContentsOffsetY + 25, 80, 20), "Sentence:", labelStyle);
        float calLength = NodeService.CalStringVisualSize(textFieldStyle, line.Sentence).x;
        line.Sentence = EditorGUI.TextField(new Rect(lineRect.x + 85, lineRect.y + initialLineContentsOffsetY + 25, 50 + calLength, 20), line.Sentence, textFieldStyle);
    }

    public Vector2 CalNodeSize()
    {
        if (dialogue == null || dialogue.Lines == null)
        {
            return new Vector2(600, 150);
        }
        return new Vector2(600, 150) + new Vector2(0, (dialogue.Lines.Count + 1) * (LineHeight + LineVeritcalDist));
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.dialogueColor;

        Vector2 caledSize = CalNodeSize();
        UpdateNodeSize(caledSize);

        base.DrawNode(offset);

        DrawBackground(Rect);
        DrawTitle(Rect);
        DrawCharacterType(Rect);
        DrawAddLineButton(Rect);
        DrawLines(Rect);

        GUI.color = Color.white;
        DrawConnectionPoints(representColor, true, true);
    }


    private void OnPlusButtonClicked()
    {
        dialogue.Lines.Add(new Line("Smile", ""));
    }
}