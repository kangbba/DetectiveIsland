using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DialogueNode : Node
{
    private Dialogue _dialogue = new Dialogue("Mono", new List<Line>());
    private List<LineNode> _lineNodes = new List<LineNode>();
    
    private bool _isFolded;

    public override Element ToElement()
    {
        return _dialogue;
    }
    
    public DialogueNode(string title, Node parentNode): base(title, parentNode)  // Node 클래스의 생성자 호출
    {
        _lineNodes = new List<LineNode>(){};
        SetNodeRectSize(CalNodeSize());
    }

    public override Vector2 CalNodeSize()
    {
        if (_dialogue == null || _dialogue.Lines == null || _isFolded)
        {
            return  new Vector2(500, 220);
        }
        return new Vector2(500, 150) + new Vector2(0, (_lineNodes.Count + 1) * 100);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        _dialogue.CharacterID = (string)CustomField("Character ID : ", _dialogue.CharacterID, Vector2.down * 40);
        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColor.dialogueColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColor.dialogueColor);

        for(int i = 0 ; i < _lineNodes.Count ; i++)
        {
            if(_isFolded && i > 0){
                break;
            }
            LineNode lineNode = _lineNodes[i];
            lineNode.DrawNode();
            lineNode.SetRectPos(NodeRect.position + Vector2.up * (i * 100 + 80));
        }

        if(!_isFolded){
            DrawAddLineButton(NodeRect);
        }
        DrawFoldingButton(NodeRect);
    }
    private void DrawAddLineButton(Rect nodeRect)
    {
        float buttonSize = 80;
        float buttonYPosition = _lineNodes.Count == 0 ? nodeRect.position.y + 100 : _lineNodes.Last().NodeRect.position.y + 100;
        Rect buttonRect = new Rect(
            nodeRect.x + nodeRect.width * 0.5f - buttonSize * 0.5f,
            buttonYPosition,
            buttonSize,
            buttonSize * 0.5f
        );
        
        if (GUI.Button(buttonRect, "Add Line")){
            LineNode lineNode = new LineNode("", this);
            _lineNodes.Add(lineNode);

            _dialogue.Lines.Add(lineNode.Line);
            SetNodeRectSize(CalNodeSize());
        }
    }
    private void DrawFoldingButton(Rect nodeRect)
    {
        float buttonWidth = 80;
        Rect buttonRect = new Rect(NodeRect.max.x - buttonWidth, NodeRect.min.y, buttonWidth, buttonWidth * 0.5f);

        if (GUI.Button(buttonRect, _isFolded ? "펼치기" : "접기")){
            _isFolded = !_isFolded;
            SetNodeRectSize(CalNodeSize());
        }
    }

}