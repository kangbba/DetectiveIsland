using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDemandResponseNode : Node
{

    public const float UPPER_MARGIN = 0;
    public const float BOTTOM_MARGIN = 30;
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width { get; set; }
    public override float Height { get; set; }

    public const float DEFAULT_WIDTH = 500;



    public List<Node> Nodes = new List<Node>();

    public ItemDemandResponseNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }
    public override Node Clone()
    {
        return new ItemDemandResponseNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = DEFAULT_WIDTH;
        Height = UPPER_MARGIN;
        Height += 80;

        for (int i = 0; i < Nodes.Count; i++)
        {
            Node node = Nodes[i];
            float xPos = NodeRect.center.x;
            float yPos = NodeRect.position.y + Height;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos, JAnchor.CenterTop);
            node.DrawNode();
            Height += node.Height + 10;
        }

        Height += BOTTOM_MARGIN;
        SetNodeRectSize(new Vector2(Width, Height));


        AttachInterface.AttachDeleteButtons(Nodes);
        AttachInterface.AttachArrowButtons(Nodes);
        AttachInterface.AttachBtnGroups(NodeRect.min, new Vector2(55, 20), Nodes, NodeID);
    }
}
