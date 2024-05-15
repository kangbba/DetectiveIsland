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

    public const float UPPER_MARGIN = 100;
    public const float BOTTOM_MARGIN = 100; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public string CharacterID;
    public List<LineNode> LineNodes = new List<LineNode>();

    private CharacterPreviewer _characterPreviewer = new CharacterPreviewer();


    private bool _isFolded;

    public override float Width => LineNode.DEFAULT_WIDTH;

    public override float Height => UPPER_MARGIN + LineNodes.Cast<Node>().GetNodesHeight() + BOTTOM_MARGIN;

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
    }

    
    public override void DrawNode()
    {
        base.DrawNode();
        float y = UPPER_MARGIN;
        CharacterID = (string)CustomField("Character ID : ", CharacterID, Vector2.up * y);
        _characterPreviewer.CharacterPreview(CharacterID, 60, 60, NodeRect.position);

        if (!_isFolded)
        {
            for (int i = 0; i < LineNodes.Count; i++)
            {
                LineNode lineNode = LineNodes[i];
                lineNode.DrawNode();
                lineNode.SetRectPos(NodeRect.position + Vector2.up * y);
                y += lineNode.Height;
            }
        }
        else{
            JImage previewText = new JImage(
                pos: new Vector2(NodeRect.center.x, NodeRect.min.y + UPPER_MARGIN * .5f),
                size: new Vector2(100, 30),
                title: LineNodes.Count > 0 ? LineNodes.First().Line.Sentence : "",
                anchor: JAnchor.CenterTop);
                
            previewText.DrawButton();
        }
        y += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, y));


        if (!_isFolded)
        {
            JButton addLineButton = new JButton(
                pos: new Vector2(NodeRect.center.x, NodeRect.max.y - BOTTOM_MARGIN * .5f),
                size: new Vector2(40, 30),
                title: "+",
                action: AddLine,
                anchor: JAnchor.CenterBottom);
            addLineButton.DrawButton();
        }
        else{
            JImage dotText = new JImage(
                pos: new Vector2(NodeRect.center.x, NodeRect.max.y - BOTTOM_MARGIN * .5f),
                size: new Vector2(40, 30),
                title: "...",
                anchor: JAnchor.CenterBottom);
            dotText.DrawButton();
        }

        JButton foldButton = new JButton(
            pos: new Vector2(NodeRect.min.x, NodeRect.min.y),
            size: new Vector2(40, 30),
            title: _isFolded ? "Expand" : "Fold",
            action: ToggleFold,
            anchor: JAnchor.TopLeft);
        foldButton.DrawButton();
    }

    private void AddLine()
    {
        LineNode lineNode = new LineNode(Guid.NewGuid().ToString(), "", NodeID);
        LineNodes.Add(lineNode);
        AddNodeRectSize(Vector2.up * LineNode.DEFAULT_HEIGHT);
    }

    private void ToggleFold()
    {
        _isFolded = !_isFolded;
    }

}