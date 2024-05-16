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

        float y = UPPER_MARGIN;
         // EmotionID 필드
        Line.EmotionID = (string)JInterface.SimpleField
        (
            value: Line.EmotionID,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + y),
            title: "Emotion ID : ",
            labelWidth: 70,
            fieldWidth: 80,
            fieldHeight: EMOTION_HEIGHT
        );

        y += EMOTION_HEIGHT;

        // Sentence 필드
        Line.Sentence = JInterface.SimpleTextArea
        (
            value: Line.Sentence,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + y),
            fieldWidth: DEFAULT_WIDTH,
            height: SENTENCE_HEIGHT // Adjust height as necessary
        );

        y += SENTENCE_HEIGHT;

        y += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, y));
    }


}
