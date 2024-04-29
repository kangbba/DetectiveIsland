using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class ItemDemandNode : Node
{
    public ItemDemand itemDemand;
    public ItemDemandNode(Rect rect, string title) : base(title)
    {

    }

    public override void DrawNode(Vector2 offset)
    {
        base.DrawNode(offset);
        Color representColor = NodeColor.itemDemandColor;


        DrawConnectionPoints(representColor, true, true);
    }
}
