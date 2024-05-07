using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public class PositionInitNode : Node
{
    public PositionInit positionInit;

    public override Element ToElement()
    {
        return positionInit;
    }

    public PositionInitNode(Vector2 pos, string title) : base(title)
    {
        UpdateNodeSize(CalNodeSize());
        UpdateNodePosition(pos);
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }


    public override void DrawNode(Vector2 offset)
    {
        Color representColor = NodeColor.positionInitColor;

        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        DrawConnectionPoints(representColor, true, true);
    }
}