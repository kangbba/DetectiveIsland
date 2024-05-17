using System;
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

    public EChacterEmotion EmotionID = EChacterEmotion.Smile;
    public string Sentence = "";
    
    public override float Width { get; set; }
    public override float Height { get; set; }

    private const float EMOTION_HEIGHT = 20;
    private const float SENTENCE_HEIGHT = 80;

    public const float DEFAULT_WIDTH = 400;
    public const float DEFAULT_HEIGHT = UPPER_MARGIN + EMOTION_HEIGHT + SENTENCE_HEIGHT + BOTTOM_MARGIN;


    public LineNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }
    public override Node Clone()
    {
        return new LineNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawNode(){

        base.DrawNode();

        Width = DEFAULT_WIDTH;
        Height = UPPER_MARGIN;
         // EmotionID 필드
        EmotionID = (EChacterEmotion)JInterface.SimpleField
        (
            value: EmotionID,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + Height),
            title: "Emotion ID : ",
            labelWidth: 70,
            fieldWidth: 80,
            fieldHeight: EMOTION_HEIGHT
        );

        Height += EMOTION_HEIGHT;

        // Sentence 필드
        Sentence = JInterface.SimpleTextArea
        (
            value: Sentence,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + Height),
            fieldWidth: DEFAULT_WIDTH,
            height: SENTENCE_HEIGHT // Adjust height as necessary
        );

        Height += SENTENCE_HEIGHT;

        Height += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, Height));
    }


}
