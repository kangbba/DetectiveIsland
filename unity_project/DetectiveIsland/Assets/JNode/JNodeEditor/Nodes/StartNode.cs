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

    public override float Width => stackedWidth;
    public override float Height => stackedHeight;

    public StartNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        base.IsStartNode = true;
    }

    private float stackedWidth = 0;
    private float stackedHeight = 0;
    public override void DrawNode()
    {
        base.DrawNode();
        stackedWidth = 300;
        stackedHeight = 100;

        SetNodeRectSize(new Vector2(Width, Height));
    }

    public override Element ToElement()
    {
        return null;
    }
}
