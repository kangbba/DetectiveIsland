using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class AssetChangeNode : Node
{
    public AssetChange assetChange;
    public AssetChangeNode(Rect rect, string title) : base(title)
    {

    }

    public override void DrawNode(Vector2 offset)
    {

        Color representColor = NodeColor.assetChangeColor;


        DrawConnectionPoints(representColor, true, true);
    }
}