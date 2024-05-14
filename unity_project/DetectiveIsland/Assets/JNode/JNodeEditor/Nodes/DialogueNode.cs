using Aroka.ArokaUtils;
using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DialogueNode : Node
{
    public string CharacterID;
    public List<LineNode> LineNodes = new List<LineNode>();

    private CharacterPreviewer _characterPreviewer = new CharacterPreviewer();

    public const float SPACING_STANDARD = 30;
    public const float LINE_NODE_WIDTH = 360;
    public const float SPACING_TAIL = 30;

    private bool _isFolded;

    public override Element ToElement()
    {
        //_lineNodes 를 리스트화한것이 밑의 인풋
        List<Line> lines = new List<Line>();
        for(int i = 0 ; i < LineNodes.Count ; i++)
        {
            lines.Add(LineNodes[i].Line);
        }
        return new Dialogue(CharacterID, lines);
    }
    
    public DialogueNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        RefreshNodeSize();
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(LINE_NODE_WIDTH, _isFolded ? LineNode.LINE_NODE_HEIGHT : (LineNodes.Count + 1) * LineNode.LINE_NODE_HEIGHT);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        float y = 50;
        CharacterID = (string)CustomField("Character ID : ", CharacterID, Vector2.up * y);
        _characterPreviewer.CharacterPreview(CharacterID, 60, 60, NodeRect.position);
        
        y += 50;
        for(int i = 0 ; i < LineNodes.Count ; i++)
        {
            if(_isFolded && i > 0){
                break;
            }
            LineNode lineNode = LineNodes[i];
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

    }
    private void AddLineButton(Rect nodeRect)
    {
        float buttonSize = 80;
        float buttonYPosition = LineNodes.Count == 0 ? nodeRect.position.y + 50 : LineNodes.Last().NodeRect.min.y + LineNode.LINE_NODE_HEIGHT ;
        Rect buttonRect = new Rect(
            nodeRect.x + nodeRect.width * 0.5f - buttonSize * 0.5f,
            buttonYPosition,
            buttonSize,
            buttonSize * 0.5f
        );
        
        if (GUI.Button(buttonRect, "Add Line")){
            LineNode lineNode = new LineNode(Guid.NewGuid().ToString(), "", NodeID);
            LineNodes.Add(lineNode);

            RefreshNodeSize();
        }
    }
    private void DrawFoldingButton(Rect nodeRect)
    {
        float buttonWidth = 80;
        Rect buttonRect = new Rect(NodeRect.max.x - buttonWidth, NodeRect.min.y, buttonWidth, buttonWidth * 0.5f);

        if (GUI.Button(buttonRect, _isFolded ? "펼치기" : "접기")){
            _isFolded = !_isFolded;
             RefreshNodeSize();
        }
    }

}