using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



[System.Serializable]
public class ChoiceSetNode : Node
{
    public ChoiceSet choiceSet;

    public ChoiceSetNode(Vector2 pos, string title) : base(title)
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
        Color representColor = NodeColor.choiceSetColor;

        base.DrawNode(offset);
        base.DrawNodeLayout(representColor);

        DrawConnectionPoints(representColor, true, true);
    }



}