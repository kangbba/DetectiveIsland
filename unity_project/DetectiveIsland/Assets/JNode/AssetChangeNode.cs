using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;


[System.Serializable]
public class AssetChangeNode : Node
{
    public AssetChange assetChange;


    public override Element ToElement()
    {
        return assetChange;
    }

    public AssetChangeNode(Vector2 pos, string title) : base(title)
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
        Color representColor = NodeColor.assetChangeColor;
        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);
        UpdateNodeSize(CalNodeSize());



        DrawConnectionPoints(representColor, true, true);
    }

}