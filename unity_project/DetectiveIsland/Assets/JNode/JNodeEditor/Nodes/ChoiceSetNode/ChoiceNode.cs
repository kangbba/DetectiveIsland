using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ChoiceNode : Node
{

    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width { get; set; }
    public override float Height { get; set; }

    public const float DEFAULT_WIDTH = 500;
    public const float CONTENT_LABLE_WIDTH = 60;
    public const float CONTENT_FIELD_WIDTH = 300;

    public const float CONTENT_FIELD_HEIGHT = 80;
    public const float CONTENT_UPPER_MARGIN = 30;
    public const float CONTENT_BOTTOM_MARGIN = 30;


    public string Content = "선택지 내용";
    public List<Node> Nodes = new List<Node>();
    
    public ChoiceNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
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
        Height += CONTENT_UPPER_MARGIN;
        Content = (string)JInterface.SimpleField
        (
            value : Content,
            pos : new Vector2(NodeRect.position.x + 100, NodeRect.position.y + Height),
            title : "Content : ",
            labelWidth : CONTENT_LABLE_WIDTH,
            fieldWidth : CONTENT_FIELD_WIDTH,
            fieldHeight : CONTENT_FIELD_HEIGHT
        );
        Height += CONTENT_FIELD_HEIGHT;
        Height += CONTENT_BOTTOM_MARGIN;
        for (int i = 0 ; i < Nodes.Count ; i++){
            Node node = Nodes[i];
            float xPos = NodeRect.center.x;
            float yPos = NodeRect.position.y + Height;
            Vector2 dialogue_I_Pos = new Vector2(xPos, yPos);
            node.SetRectPos(dialogue_I_Pos, JAnchor.CenterTop);
            node.DrawNode();
            Height += node.Height + 10; 
        }
        AttachInterface.AttachDeleteButtons(Nodes);
        AttachInterface.AttachArrowButtons(Nodes);
        AttachInterface.AttachBtnGroups(NodeRect.min, new Vector2(55, 20), Nodes, NodeID);
   // Add buttons using JButton

        Height += BOTTOM_MARGIN;
        SetNodeRectSize(new Vector2(Width, Height));
    }
}
