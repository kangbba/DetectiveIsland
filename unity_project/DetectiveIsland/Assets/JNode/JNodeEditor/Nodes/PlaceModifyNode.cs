using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class PlaceModifyNode : Node
{
    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width => 300;

    public override float Height => UPPER_MARGIN + STANDARD_SPACING * 3 + BOTTOM_MARGIN;

    private PlaceModify _placeModify = new PlaceModify(true, "");


    public const float STANDARD_SPACING = 20;

    public PlaceModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return _placeModify;
    }
    public override void DrawNode()
    {
        base.DrawNode();
        float y = UPPER_MARGIN;

        _placeModify.IsGain = (bool)JInterface.SimpleField
        (
            title: "IsGain : ",
            value: _placeModify.IsGain,
            pos: new Vector2(NodeRect.position.x, y),
            labelWidth: 100,
            fieldWidth: 80,
            height: STANDARD_SPACING
        );

        y += STANDARD_SPACING;
        _placeModify.Id = (string)JInterface.SimpleField
        (
            title: "ID : ",
            value: _placeModify.Id,
            pos: new Vector2(NodeRect.position.x, y),
            labelWidth: 100,
            fieldWidth: 80,
            height: STANDARD_SPACING
        );
        y += STANDARD_SPACING;
        
        y += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, y));
    }
}
