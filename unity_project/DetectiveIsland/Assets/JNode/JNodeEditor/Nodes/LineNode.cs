using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class LineNode : Node
{
    public const float UPPER_MARGIN = 0;
    public const float BOTTOM_MARGIN = 0; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public Line Line = new Line("Smile", "");

    public override float Width => DEFAULT_WIDTH;

    public override float Height => DEFAULT_HEIGHT;

    private const float EMOTION_HEIGHT = 20;
    private const float SENTENCE_HEIGHT = 80;

    public const float DEFAULT_WIDTH = 400;
    public const float DEFAULT_HEIGHT = UPPER_MARGIN + EMOTION_HEIGHT + SENTENCE_HEIGHT + BOTTOM_MARGIN;

    public LineNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawNode(){

        base.DrawNode();

        Line.EmotionID = (string)CustomField("Emotion ID : ", Line.EmotionID, Vector2.up * UPPER_MARGIN,labelWidth : 70 , fieldHeight: EMOTION_HEIGHT);

        Line.Sentence = CustomTextArea(Line.Sentence, Vector2.up * (EMOTION_HEIGHT + UPPER_MARGIN), width : Width, height : SENTENCE_HEIGHT);

        SetNodeRectSize(new Vector2(Width, Height));

        GUI.Box(NodeRect, new GUIContent());
    }


}
