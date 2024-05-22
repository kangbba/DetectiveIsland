using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;
using System;

[System.Serializable]
public class GainPlaceNode : Node
{
    public const float UPPER_MARGIN = 40;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public override float Width { get; set; }
    public override float Height { get; set; }

    public EPlaceID ID;
    public bool IsGain;

    public const float STANDARD_SPACING = 20;

    public GainPlaceNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }
    public override Node Clone()
    {
        return new GainPlaceNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }

    public override Element ToElement()
    {
        return new GainPlace(IsGain, ID);
    }
    public override void DrawNode()
    {
        base.DrawNode();
        Width = 300;//Default
        Height = UPPER_MARGIN;

        IsGain = (bool)JInterface.SimpleField
        (
            title: "IsGain : ",
            value: IsGain,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + Height),
            labelWidth: 100,
            fieldWidth: 80,
            fieldHeight: STANDARD_SPACING
        );

        Height += STANDARD_SPACING;
        ID = (EPlaceID)JInterface.SimpleField
        (
            title: "ID : ",
            value: ID,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + Height),
            labelWidth: 100,
            fieldWidth: 80,
            fieldHeight: STANDARD_SPACING
        );
        Height += STANDARD_SPACING;

        Height += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, Height));
    }
}
