using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class PositionInitNode : Node
{
    public PositionInit positionInit;
    public PositionInitNode(Rect rect, string title) : base(title)
    {
    }

    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.positionInitColor;

        DrawConnectionPoints(representColor, true, true);
    }
}