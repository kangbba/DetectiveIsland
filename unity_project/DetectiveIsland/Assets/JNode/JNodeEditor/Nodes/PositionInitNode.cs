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

    public List<CharacterPositionNode> CharacterPositionNodes = new List<CharacterPositionNode>();

    public PositionInitNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {

    }
    public override float Width { get; set; }
    public override float Height { get; set; }
    public override Element ToElement()
    {   
        List<CharacterPosition> characterPositions = new List<CharacterPosition>();
        foreach (CharacterPositionNode characterPositionNode in CharacterPositionNodes)
        {
            characterPositions.Add(characterPositionNode.CharacterPosition);
        }
        // PositionInit element creation
        // return new PositionInit(characterPositions); // Assuming PositionInit is a class that takes a list of CharacterPositions
        throw new System.NotImplementedException();
    }

    public void AddCharacterPosition()
    {
        CharacterPositionNode characterPositionNode = new CharacterPositionNode(Guid.NewGuid().ToString(), "", NodeID);
        CharacterPositionNodes.Add(characterPositionNode);
    }

    public void DeleteCharacterPositionNode(string nodeId)
    {
        if (string.IsNullOrEmpty(nodeId))
        {
            Debug.LogWarning("Node Id Error");
            return;
        }
        CharacterPositionNodes.RemoveAll(node => node.NodeID == nodeId);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = 350;
        Height = UPPER_MARGIN;
        for (int i = 0; i < CharacterPositionNodes.Count; i++)
        {
            CharacterPositionNode characterPositionNode = CharacterPositionNodes[i];
            characterPositionNode.SetRectPos(new Vector2(NodeRect.center.x, NodeRect.position.y + Height), JAnchor.CenterTop);
            characterPositionNode.DrawNode();
            Height += characterPositionNode.Height + 10;
        }

        Height += BOTTOM_MARGIN;

        JButton addCharacterPositionButton = new JButton(
            pos: new Vector2(NodeRect.center.x, NodeRect.max.y - BOTTOM_MARGIN * 0.5f),
            size: new Vector2(40, 30),
            title: "+",
            action: AddCharacterPosition,
            anchor: JAnchor.CenterBottom);
        addCharacterPositionButton.Draw();

        SetNodeRectSize(new Vector2(Width, Height));


        JInterface.AttachDeleteButtons(CharacterPositionNodes);
        JInterface.AttachArrowButtons(CharacterPositionNodes);


    }
}
