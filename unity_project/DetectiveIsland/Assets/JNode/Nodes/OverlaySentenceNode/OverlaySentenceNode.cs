using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;
using System;

[System.Serializable]
public class OverlaySentenceNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30;
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width { get; set; }
    public override float Height { get; set; }
  
    public string Sentence = "";
    public float SentenceTime = 1;
    public float AfterDelayTime = 3;

    public const float FIELD_COMMON_HEIGHT = 20;

    public OverlaySentenceNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    { 
    }
    public override Node Clone()
    {
        return new OverlaySentenceNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }

    public override Element ToElement()
    {
        return new OverlaySentence(Sentence, SentenceTime, AfterDelayTime);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = 300;
        Height = UPPER_MARGIN;

        Sentence = JInterface.SimpleTextArea
       (
           value: Sentence,
           pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
           fieldWidth: 200,
           height: FIELD_COMMON_HEIGHT * 2
       );
        Height += FIELD_COMMON_HEIGHT * 2;

        SentenceTime = (float)JInterface.SimpleField
       (
           title: "Sentence Time: ",
           value: SentenceTime,
           pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
           labelWidth: 100,
           fieldWidth: 80,
           fieldHeight: FIELD_COMMON_HEIGHT
       );
        Height += FIELD_COMMON_HEIGHT;

          AfterDelayTime = (float)JInterface.SimpleField
       (
           title: "After Delay Time: ",
           value: AfterDelayTime,
           pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
           labelWidth: 100,
           fieldWidth: 80,
           fieldHeight: FIELD_COMMON_HEIGHT
       );
        Height += FIELD_COMMON_HEIGHT;

        Height += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, Height));
    }
}
