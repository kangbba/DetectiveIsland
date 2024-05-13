using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class LineNode : Node
{
    
    GUIStyle _labelStyle = new GUIStyle(GUI.skin.label)
    {
        alignment = TextAnchor.UpperCenter,
        fontSize = 10,
        normal = { textColor = Color.white }
    };
    GUIStyle _textAreaFieldStyle = new GUIStyle(EditorStyles.textArea)
    {
        alignment = TextAnchor.UpperLeft,
        normal = { textColor = Color.white, background = Texture.GetBoxTexture(Color.gray * 0.25f) },
        fontSize = 10,
        fixedWidth = 300,
        fixedHeight = 50,
    };


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
        return new Vector2(300, 100);
    }
    public override void DrawNode(){

        base.DrawNode();
        _line.EmotionID = (string)CustomField("Emotion ID : ", _line.EmotionID, Vector2.down * 0);
        _line.Sentence = (string)CustomField("Sentence : ",_line.Sentence, Vector2.down * 20, width : 300, height : 80);

        GUI.Box(NodeRect, new GUIContent());
    }


}
