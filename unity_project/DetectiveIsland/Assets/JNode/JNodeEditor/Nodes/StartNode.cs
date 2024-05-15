using Aroka.ArokaUtils;
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

    public override float Width => 300;

    public override float Height => 100;

    public StartNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        base.IsStartNode = true;
    }

    public override void DrawNode()
    {
        base.DrawNode();

        SetNodeRectSize(new Vector2(Width, Height));
    }

    public override Element ToElement()
    {
        return null;
    }
}
