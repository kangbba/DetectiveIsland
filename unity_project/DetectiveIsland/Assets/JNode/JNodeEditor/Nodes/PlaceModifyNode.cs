using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class PlaceModifyNode : Node
{
    public const float UPPER_MARGIN = 40;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public override float Width { get; set; }
    public override float Height { get; set; }



    public bool _isGain = true;
    public string _id = "";


    public const float STANDARD_SPACING = 20;

    public PlaceModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return new PlaceModify(_isGain, _id);
    }
    public override void DrawNode()
    {
        base.DrawNode();
        Width = 300;//Default
        Height = UPPER_MARGIN;

        _isGain= (bool)JInterface.SimpleField
        (
            title: "IsGain : ",
            value: _isGain,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + Height),
            labelWidth: 100,
            fieldWidth: 80,
            fieldHeight: STANDARD_SPACING
        );

        Height += STANDARD_SPACING;
        _id = (string)JInterface.SimpleField
        (
            title: "ID : ",
            value: _id,
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
