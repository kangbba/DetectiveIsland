using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;
using System;

[System.Serializable]
public class GainItemNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public bool IsGain;
    public EItemID ID;
    public int Amount;

    public override float Width { get; set; }
    public override float Height { get; set; }


    public GainItemNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Node Clone()
    {
        return new GainItemNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }

    public override Element ToElement()
    {
        return new GainItem(IsGain, ID, Amount);
    }
    public override void DrawNode()
    {
        base.DrawNode();
        Width = 300;
        Height = UPPER_MARGIN;
        float standardFieldHeight = 20;
        IsGain = (bool)JInterface.SimpleField(
            value: IsGain,
            pos: new Vector2(NodeRect.position.x + 10 , NodeRect.position.y + Height),
            title: "IsGain : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        Height += standardFieldHeight;

        ID = (EItemID)JInterface.SimpleField(
            value: ID,
            pos: new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "Id : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        Height += standardFieldHeight;

        Amount = (int)JInterface.SimpleField(
            value: Amount,
            pos: new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "Amount : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        Height += standardFieldHeight;
        Height += BOTTOM_MARGIN;
        SetNodeRectSize(Width, Height);

    }
}