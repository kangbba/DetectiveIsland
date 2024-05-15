using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PositionInitNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 70; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    private List<CharacterPositionNode> _characterPositionNodes = new List<CharacterPositionNode>();

    public PositionInitNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override float Width => 300;

    public override float Height => UPPER_MARGIN + _characterPositionNodes.Count * CharacterPositionNode.DEFAULT_HEIGHT + BOTTOM_MARGIN;

    public override Element ToElement()
    {   
        List<CharacterPosition> characterPositions = new List<CharacterPosition>();
        foreach (CharacterPositionNode characterPositionNode in _characterPositionNodes)
        {
            characterPositions.Add(characterPositionNode.CharacterPosition);
        }
        // PositionInit element creation
        // return new PositionInit(characterPositions); // Assuming PositionInit is a class that takes a list of CharacterPositions
        throw new System.NotImplementedException();
    }

    public void AddCharacterPosition()
    {
        CharacterPositionNode characterPositionNode = new CharacterPositionNode(Guid.NewGuid().ToString(), "Character Position", NodeID);
        _characterPositionNodes.Add(characterPositionNode);
    }

    public void DeleteCharacterPositionNode(string nodeId)
    {
        if (string.IsNullOrEmpty(nodeId))
        {
            Debug.LogWarning("Node Id Error");
            return;
        }
        _characterPositionNodes.RemoveAll(node => node.NodeID == nodeId);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        float y = UPPER_MARGIN;
        for (int i = 0; i < _characterPositionNodes.Count; i++)
        {
            CharacterPositionNode characterPositionNode = _characterPositionNodes[i];
            characterPositionNode.SetRectPos(new Vector2(NodeRect.center.x, NodeRect.position.y + y), JAnchor.CenterTop);
            characterPositionNode.DrawNode();
            JButton deleteBtn = new JButton(
                pos: new Vector2(characterPositionNode.NodeRect.max.x, characterPositionNode.NodeRect.position.y),
                size: Vector2.one * 20,
                title: "X",
                anchor: JAnchor.TopRight,
                action: () => DeleteCharacterPositionNode(characterPositionNode.NodeID)
            );
            deleteBtn.Draw();
            y += characterPositionNode.Height;
        }

        y += BOTTOM_MARGIN;
        SetNodeRectSize(new Vector2(Width, y));

        JButton addCharacterPositionButton = new JButton(
            pos: new Vector2(NodeRect.center.x, NodeRect.max.y - BOTTOM_MARGIN * 0.5f),
            size: new Vector2(40, 30),
            title: "+",
            action: AddCharacterPosition,
            anchor: JAnchor.CenterBottom);
        addCharacterPositionButton.Draw();
    }
}
