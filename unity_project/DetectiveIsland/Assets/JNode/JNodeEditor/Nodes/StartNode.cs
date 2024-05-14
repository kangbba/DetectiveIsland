using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class StartNode : Node
{
    public StartNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override void DrawNode()
    {
        base.DrawNode();

        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColors.startNodeColor);
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
