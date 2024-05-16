using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class OverlayPictureNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width => stackedWidth;

    public override float Height => stackedHeight;

    private OverlayPicture _overlayPicture = new OverlayPicture("Black", "FadeIn", 1);

    public const float STANDARD_SPACING = 20;
    public OverlayPictureNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return _overlayPicture;
    }

    private float stackedHeight = 0;

    private float stackedWidth = 0;

    public override void DrawNode()
    {
        base.DrawNode();

        stackedWidth = 300;
        stackedHeight = UPPER_MARGIN;

        _overlayPicture.PictureID = (string)JInterface.SimpleField
        (
            title: "Picture ID: ",
            value: _overlayPicture.PictureID,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + stackedHeight),
            labelWidth: 100,
            fieldWidth: 80,
            fieldHeight: STANDARD_SPACING
        );

        stackedHeight += STANDARD_SPACING;
        _overlayPicture.EffectID = (string)JInterface.SimpleField
        (
            title: "Effect ID: ",
            value: _overlayPicture.EffectID,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + stackedHeight),
            labelWidth: 100,
            fieldWidth: 80,
            fieldHeight: STANDARD_SPACING
        );

        stackedHeight += STANDARD_SPACING;
        _overlayPicture.EffectTime = (float)JInterface.SimpleField
        (
            title: "Effect Time: ",
            value: _overlayPicture.EffectTime,
            pos: new Vector2(NodeRect.position.x, NodeRect.position.y + stackedHeight),
            labelWidth: 100,
            fieldWidth: 80,
            fieldHeight: STANDARD_SPACING
        );
        stackedHeight += STANDARD_SPACING;

        stackedHeight += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, stackedHeight));

    }
}
