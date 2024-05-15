using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class OverlayPictureNode : Node
{
    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width => 300;

    public override float Height => UPPER_MARGIN + STANDARD_SPACING * 3 + BOTTOM_MARGIN;

    private OverlayPicture _overlayPicture = new OverlayPicture("Black", "FadeIn", 1);

    public const float STANDARD_SPACING = 20;
    public OverlayPictureNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return _overlayPicture;
    }
    public override void DrawNode()
    {
        base.DrawNode();
       
        float y = UPPER_MARGIN;

        _overlayPicture.PictureID = (string)JInterface.SimpleField
        (
            title: "Picture ID: ",
            value: _overlayPicture.PictureID,
            pos: new Vector2(NodeRect.position.x, y),
            labelWidth: 100,
            fieldWidth: 80,
            height: STANDARD_SPACING
        );

        y += STANDARD_SPACING;
        _overlayPicture.EffectID = (string)JInterface.SimpleField
        (
            title: "Effect ID: ",
            value: _overlayPicture.EffectID,
            pos: new Vector2(NodeRect.position.x, y),
            labelWidth: 100,
            fieldWidth: 80,
            height: STANDARD_SPACING
        );

        y += STANDARD_SPACING;
        _overlayPicture.EffectTime = (float)JInterface.SimpleField
        (
            title: "Effect Time: ",
            value: _overlayPicture.EffectTime,
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
