using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Aroka.ArokaUtils;


[System.Serializable]
public class ItemDemandNode : Node
{
    private ItemDemand _itemDemand;

    public ItemDemandNode(string title, Node parentNode) : base(title, parentNode)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        return _itemDemand;
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }
    public override void DrawNode()
    {
        base.DrawNode();
        _itemDemand.ItemID = (string)CustomField("Item ID : ", _itemDemand.ItemID, Vector2.up * 0f);
        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColor.itemDemandColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColor.itemDemandColor);
    }
}
