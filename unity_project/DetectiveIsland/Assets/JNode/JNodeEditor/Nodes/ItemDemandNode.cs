using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Aroka.ArokaUtils;
using System;

[System.Serializable]
public class ItemDemandNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public string _itemID;
    public List<DialogueNode> _dialogueNodes = new List<DialogueNode>();
    public List<ItemDemandResponseNode> itemDemandResponseNodes = new List<ItemDemandResponseNode>() 
    { 
        new(Guid.NewGuid().ToString(),"Success", ""), 
        new(Guid.NewGuid().ToString(),"Fail"   , ""),
        new(Guid.NewGuid().ToString(),"Cancel" , "")
    };

    public override float Width { get; set; }
    public override float Height { get; set; }

    public ItemDemandNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        itemDemandResponseNodes[0].ParentNodeID = NodeID;
        itemDemandResponseNodes[1].ParentNodeID = NodeID;
        itemDemandResponseNodes[2].ParentNodeID = NodeID;
    }


    private float AddDialogueBtn_UpperMargin = 50;
    private float AddDialogueBtn_BottomMargin = 70;

    public override Element ToElement()
    {
        return new ItemDemand(_itemID, _dialogueNodes.Cast<Node>().ToList().ToElements().Cast<Dialogue>().ToList(),
            itemDemandResponseNodes[0].Nodes.ToElements(), itemDemandResponseNodes[1].Nodes.ToElements(), itemDemandResponseNodes[2].Nodes.ToElements());
    }
    public override void DrawNode()
    {
        base.DrawNode();
        Width = 0;
        Height = UPPER_MARGIN;
        for (int i = 0; i < _dialogueNodes.Count; i++)
        {
            Node node = _dialogueNodes[i];
            float xPos = NodeRect.position.x + GetCenterLocalPosX(node, this);
            float yPos = NodeRect.position.y + Height;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos);
            node.DrawNode();
            Height += node.Height + 10;
        }

        Height += AddDialogueBtn_UpperMargin;

        JButton addDialogueButton = new JButton
        (
            pos: new Vector2(NodeRect.center.x, NodeRect.position.y + Height),
            size: new Vector2(80, 30),
            title: "Add Dialogue",
            action: AddDialogue,
            anchor: JAnchor.Center
        );
        addDialogueButton.Draw();
        Height += AddDialogueBtn_BottomMargin;
        Width += LEFT_MARGIN;
        for (int i = 0; i < itemDemandResponseNodes.Count; i++)
        {
            Node node = itemDemandResponseNodes[i];
            node.DrawNode();
            float node_i_Width = node.Width + 20;
            Vector2 pos = NodeRect.position + new Vector2(LEFT_MARGIN + (i * node_i_Width), Height);
            node.SetRectPos(pos);
            Width += node_i_Width;
        }
        Height += itemDemandResponseNodes.GetMaxHeight();
        Height += BOTTOM_MARGIN;
        Width += RIGHT_MARGIN;
        SetNodeRectSize(itemDemandResponseNodes.Count == 0 ? 600 : Width, Height);
    }
    private void AddDialogue()
    {
        DialogueNode dialogueNode = new DialogueNode(Guid.NewGuid().ToString(), "DialogueNode", NodeID);
        _dialogueNodes.Add(dialogueNode);
        Debug.Log("Dialogue node added.");
    }



    private float GetCenterLocalPosX(Node node, Node parentNode)
    {
        return parentNode.NodeRect.width * 0.5f - node.NodeRect.width * 0.5f;
    }

}
