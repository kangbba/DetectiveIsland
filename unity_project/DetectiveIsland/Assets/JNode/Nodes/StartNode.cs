using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class StartNode : Node
{
    public StartNode(string title, Node parentNode) : base(title, parentNode)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override void DrawNode()
    {
        base.DrawNode();
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColor.startNodeColor);
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }

    public override Element ToElement()
    {
        return null;
    }
}
