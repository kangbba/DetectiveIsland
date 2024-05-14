using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class LineNode : Node
{
    public const float LINE_NODE_HEIGHT = 100;
    public const float LINE_SENTENCE_HEIGHT = 80;

    public Line Line = new Line("Smile", "");

    public LineNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(300, LINE_NODE_HEIGHT);
    }
    public override void DrawNode(){

        base.DrawNode();
        Line.EmotionID = (string)CustomField("Emotion ID : ", Line.EmotionID, Vector2.down * 0);
        Line.Sentence = CustomTextArea(Line.Sentence, Vector2.up * 60 * .5f, width : DialogueNode.LINE_NODE_WIDTH, height : LINE_SENTENCE_HEIGHT);

        GUI.Box(NodeRect, new GUIContent());
    }


}
