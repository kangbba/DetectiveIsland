using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class LineNode : Node
{
    public const float LINE_NODE_HEIGHT = 150;
    public const float LINE_SENTENCE_HEIGHT = 80;

    private Line _line = new Line("Smile", "");

    public Line Line { get => _line;  }

    public LineNode(string title, Node parentNode): base(title, parentNode)  // Node 클래스의 생성자 호출
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(ParentNode.NodeRect.width, LINE_NODE_HEIGHT);
    }
    public override void DrawNode(){

        base.DrawNode();
        _line.EmotionID = (string)CustomField("Emotion ID : ", _line.EmotionID, Vector2.down * 0);
        _line.Sentence = CustomTextArea(_line.Sentence, Vector2.up * 60 * .5f, width : DialogueNode.LINE_NODE_WIDTH, height : LINE_SENTENCE_HEIGHT);

        GUI.Box(NodeRect, new GUIContent());
    }


}
