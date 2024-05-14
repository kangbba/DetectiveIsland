using Aroka.ArokaUtils;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DialogueNode : Node
{
    private string _characterID = "Mono";
    private List<LineNode> _lineNodes = new List<LineNode>();
    private CharacterPreviewer _characterPreviewer = new CharacterPreviewer();

    public const float SPACING_STANDARD = 30;
    public const float LINE_NODE_WIDTH = 360;
    public const float SPACING_TAIL = 30;

    private bool _isFolded;

    public override Element ToElement()
    {
        //_lineNodes 를 리스트화한것이 밑의 인풋
        return new Dialogue(_characterID, null);
    }
    
    public DialogueNode(string title, Node parentNode): base(title, parentNode)  // Node 클래스의 생성자 호출
    {
        _lineNodes = new List<LineNode>(){};
        SetNodeRectSize(CalNodeSize());
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(LINE_NODE_WIDTH, (_lineNodes.Count + 1) * LineNode.LINE_NODE_HEIGHT);
    }

    public override void DrawNode()
    {
        base.DrawNode();


        float y = 50;
        _characterID = (string)CustomField("Character ID : ", _characterID, Vector2.up * y);
        _characterPreviewer.CharacterPreview(_characterID, 60, 60, _nodeRect.position);
        
        y += 50;
        for(int i = 0 ; i < _lineNodes.Count ; i++)
        {
            if(_isFolded && i > 0){
                break;
            }
            LineNode lineNode = _lineNodes[i];
            lineNode.DrawNode();
            lineNode.SetRectPos(NodeRect.position + Vector2.up * y);
            y += LineNode.LINE_NODE_HEIGHT;
        }

        y += SPACING_STANDARD;
        if(!_isFolded){
            AddLineButton(NodeRect);
        }
        y += SPACING_STANDARD;
        DrawFoldingButton(NodeRect);
        y += SPACING_STANDARD;
        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColors.dialogueColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColors.dialogueColor);
    }
    private void AddLineButton(Rect nodeRect)
    {
        float buttonSize = 80;
        float buttonYPosition = _lineNodes.Count == 0 ? nodeRect.position.y + 50 : _lineNodes.Last().NodeRect.min.y + LineNode.LINE_NODE_HEIGHT ;
        Rect buttonRect = new Rect(
            nodeRect.x + nodeRect.width * 0.5f - buttonSize * 0.5f,
            buttonYPosition,
            buttonSize,
            buttonSize * 0.5f
        );
        
        if (GUI.Button(buttonRect, "Add Line")){
            LineNode lineNode = new LineNode("", this);
            _lineNodes.Add(lineNode);

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