using Aroka.ArokaUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class StartNode : Node
{
    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public override float Width { get; set; }
    public override float Height { get; set; }
    public StartNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        base.IsStartNode = true;
    }
    
    public override Node Clone()
    {
        return new StartNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }

    public override void DrawNode()
    {
        base.DrawNode();

        Width = 300;
        Height = 100;

        SetNodeRectSize(new Vector2(Width, Height));
    }

    public override Element ToElement()
    {
        return null;
    }
}
