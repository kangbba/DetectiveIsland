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
    public const float BOTTOM_MARGIN = 80; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public string CharacterID = "Mono";
    public List<LineNode> LineNodes = new List<LineNode>();
    private CharacterPreviewer _characterPreviewer = new CharacterPreviewer();


    private bool _isFolded;

    public override float Width  { get; set; }
    public override float Height { get; set; }

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

    public void DeleteLineNode(string nodeId)
    {
        if (nodeId == null || nodeId == "")
        {
            Debug.LogWarning("Node Id Error");
            return;
        }
        LineNodes.Remove(LineNodes.FirstOrDefault(node => node.NodeID == nodeId));
    }
    public void MoveListOrder(string nodeId, int direction)
    {
        if (string.IsNullOrEmpty(nodeId))
        {
            Debug.LogWarning("Node Id Error");
            return;
        }

        int index = LineNodes.FindIndex(node => node.NodeID == nodeId);
        if (index == -1)
        {
            Debug.LogWarning("Node not found");
            return;
        }

        int newIndex = index + direction;

        // Ensure newIndex is within valid range
        if (newIndex < 0 || newIndex >= LineNodes.Count)
        {
            Debug.LogWarning("Invalid move direction");
            return;
        }

        // Release focus from the current text area
        GUI.FocusControl(null);

        // Swap the elements to change the order
        LineNode nodeToMove = LineNodes[index];
        LineNodes.RemoveAt(index);
        LineNodes.Insert(newIndex, nodeToMove);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = LineNode.DEFAULT_WIDTH + 50;
        Height = UPPER_MARGIN;
        // _characterPreviewer.CharacterPreview(CharacterID, 60, 60, NodeRect.position);

        if (!_isFolded)
        {
            for (int i = 0; i < LineNodes.Count; i++)
            {
                LineNode lineNode = LineNodes[i];
                lineNode.DrawNode();

                Vector2 lineNodePos = new Vector2(NodeRect.center.x, NodeRect.position.y + Height);
                lineNode.SetRectPos(lineNodePos, JAnchor.CenterTop);

                Vector2 miniBtnSize = Vector2.one * 20;

                Height += lineNode.Height + 10;
            }
            JInterface.AttachDeleteButtons(LineNodes);
            JInterface.AttachArrowButtons(LineNodes);
        }
        else
        {
            JTextRect previewText = new JTextRect(
                pos: new Vector2(NodeRect.center.x, NodeRect.min.y + UPPER_MARGIN * .5f),
                size: new Vector2(100, 30),
                title: LineNodes.Count > 0 ? LineNodes.First().Line.Sentence : "",
                anchor: JAnchor.CenterTop);
                
            previewText.Draw();
        }
        Height += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, Height));


        CharacterID = (string)JInterface.SimpleField
        (
            value : CharacterID,
            pos: new Vector2(NodeRect.min.x, NodeRect.position.y + UPPER_MARGIN * .5f),
            title : "Character ID : ",
            labelWidth : 80,
            fieldWidth : 80,
            fieldHeight : 20
        );
        Vector2 characterPos = new Vector2(NodeRect.center.x,  NodeRect.position.y + UPPER_MARGIN * .5f);
        Vector2 characterSize = new Vector2(40, 40);
        _characterPreviewer.CharacterPreview(CharacterID, characterSize, characterPos.GetAnchoredPos(characterSize, JAnchor.CenterTop));

        if (!_isFolded)
        {
            JButton addLineButton = new JButton(
                pos: new Vector2(NodeRect.center.x, NodeRect.max.y - BOTTOM_MARGIN * .5f),
                size: new Vector2(40, 30),
                title: "+",
                action: AddLine,
                anchor: JAnchor.CenterBottom);
            addLineButton.Draw();
        }
        else{
            JTextRect dotText = new JTextRect(
                pos: new Vector2(NodeRect.center.x, NodeRect.max.y - BOTTOM_MARGIN * .5f),
                size: new Vector2(40, 30),
                title: "...",
                anchor: JAnchor.CenterBottom);
            dotText.Draw();
        }


        JButton foldButton = new JButton(
            pos: new Vector2(NodeRect.min.x, NodeRect.min.y),
            size: new Vector2(40, 30),
            title: _isFolded ? "←→" : "→←",
            action: ToggleFold,
            anchor: JAnchor.TopLeft);
        foldButton.Draw();
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