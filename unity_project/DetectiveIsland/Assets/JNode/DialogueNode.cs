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
    public float LineVeritcalDist = 5;

    private float LineHeight => (_lineSentenceHeight + _emotionTextFieldHeight) * 1.5f;
    private const float _lineSentenceWidth = 300;
    private float _lineSentenceHeight = 60;
    private float _emotionTextFieldHeight = 20;
    public override Element ToElement()
    {
        return dialogue;
    }
    public DialogueNode(Vector2 pos, string title) : base(title)
    {
        UpdateNodeSize(CalNodeSize());
        UpdateNodePosition(pos);
        ParentConnectingPoint = new ConnectingPoint();
        ChildConnectingPoint = new ConnectingPoint();
    }
    public void DrawCharacterType(Rect nodeTotalRect)
    {

        GUIStyle textFieldStyle = new GUIStyle()
        {
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = Color.white, background = Texture.GetBoxTexture(Color.gray * 0.25f) },
            fontSize = 12
        };
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = 10,
            normal = { textColor = Color.white }
        };
        GUIContent labelContent = new GUIContent("Character Type:");
        Vector2 labelSize_CharacterName = labelStyle.CalcSize(labelContent);

        EditorGUI.LabelField(new Rect(nodeTotalRect.x, nodeTotalRect.y + 60, labelSize_CharacterName.x, 20), labelContent, labelStyle);

        // GUI.Label(new Rect(nodeTotalRect.x, nodeTotalRect.y + 60, labelSize_CharacterName.x, 20), labelContent, labelStyle);
        // Character Name Input Field
        dialogue.CharacterID = EditorGUI.TextField(new Rect(nodeTotalRect.x + labelSize_CharacterName.x + 5, nodeTotalRect.y + 60, nodeTotalRect.width - labelSize_CharacterName.x - 10, 20), dialogue.CharacterID, textFieldStyle);
    }


    private void DrawAddLineButton(Rect nodeRect)
    {
        float buttonSize = 80;
        float buttonYPosition = nodeRect.y + 105 + (dialogue.Lines.Count * (LineHeight + LineVeritcalDist));
        Rect buttonRect = new Rect(
            nodeRect.x + nodeRect.width * 0.5f - buttonSize * 0.5f,
            buttonYPosition,
            buttonSize,
            buttonSize * 0.5f
        );

        if (GUI.Button(buttonRect, "Add Line"))
            OnPlusButtonClicked();
    }

    private void DrawLines(Rect nodeRect)
    {
        float yPos = nodeRect.y + 105;
        for (int i = 0; i < dialogue.Lines.Count; i++)
        {
            // Calculate the rectangle for the line
            Rect lineRect = new Rect(nodeRect.x + (nodeRect.width - nodeRect.width) / 2, yPos + (i * (LineHeight + LineVeritcalDist)), nodeRect.width, LineHeight);

            // Create and style the label
            GUIStyle lineLabelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperCenter,
                fontSize = 10,
                normal = { textColor = Color.yellow, background = Texture.GetBoxTexture(Color.white * 0.2f)}
            };

            // Draw the box for the line text
            GUI.Box(lineRect, "Line", lineLabelStyle);

            // Draw the content for the line
            DrawLineContent(i, lineRect);

            // Calculate the rectangle for the minus button
            float buttonWidth = 20;
            float buttonHeight = 20;
            float buttonX = lineRect.xMax - buttonWidth - 5;
            float buttonY = lineRect.y - LineHeight * 0.5f + 15 + (lineRect.height - buttonHeight) / 2;
            Rect buttonRect = new Rect(buttonX, buttonY, buttonWidth, buttonHeight);

            // Draw the minus button and check if it's clicked
            if (GUI.Button(buttonRect, "X"))
            {
                OnMinusButtonClicked(dialogue.Lines[i]);
            }
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
        GUIStyle textFieldStyle = new GUIStyle(EditorStyles.textField)
        {
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = Color.white, background = Texture.GetBoxTexture(Color.gray * 0.25f) },
            fontSize = 10
        };
        GUIStyle textAreaFieldStyle = new GUIStyle(EditorStyles.textArea)
        {
            alignment = TextAnchor.UpperLeft,
            normal = { textColor = Color.white, background = Texture.GetBoxTexture(Color.gray * 0.25f) },
            fontSize = 10,
        };

        float initialLineContentsOffsetY = 20;

        Line line = dialogue.Lines[index];
        EditorGUI.LabelField(new Rect(lineRect.x + 5, lineRect.y + initialLineContentsOffsetY, 80, 20), "Emotion ID:", labelStyle);
        line.EmotionID = EditorGUI.TextField(new Rect(lineRect.x + 85, lineRect.y + initialLineContentsOffsetY, 150, 20), line.EmotionID, textFieldStyle);

        EditorGUI.LabelField(new Rect(lineRect.x + 5, lineRect.y + initialLineContentsOffsetY + 25, 80, 20), "Sentence:", labelStyle);
        // Vector2 calSize = NodeService.CalStringVisualSize(textFieldStyle, line.Sentence);
        // line.Sentence = EditorGUI.TextField(new Rect(lineRect.x + 85, lineRect.y + initialLineContentsOffsetY + 25, _lineSentenceWidth, _lineSentenceHeight), line.Sentence, textFieldStyle);
        
        // if(Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return){
        //     Debug.Log("엔터");
        //     Event.current.Use();
        // }
        Rect textFieldRect = new Rect(lineRect.x + 85, lineRect.y + initialLineContentsOffsetY + 25, _lineSentenceWidth, _lineSentenceHeight);
        line.Sentence = EditorGUI.TextArea(textFieldRect, line.Sentence, textAreaFieldStyle);
    }
    

    public override Vector2 CalNodeSize()
    {
        if (dialogue == null || dialogue.Lines == null)
        {
            return  new Vector2(600, 150);
        }
        return new Vector2(600, 150) + new Vector2(0, (dialogue.Lines.Count + 1) * (LineHeight + LineVeritcalDist));
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.dialogueColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);



        
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

    private void OnMinusButtonClicked(Line line)
    {
        dialogue.Lines.Remove(line);
    }
}