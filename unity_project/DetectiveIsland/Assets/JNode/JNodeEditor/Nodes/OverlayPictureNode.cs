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

    public override float Height => 200;

    private OverlayPicture _overlayPicture = new OverlayPicture("Black", "FadeIn", 1);

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
        _overlayPicture.PictureID = (string)CustomField("Picture ID: " , _overlayPicture.PictureID ,  Vector2.down * 40);
        _overlayPicture.EffectID = (string)CustomField("Effect ID: " , _overlayPicture.EffectID , Vector2.down * 80);
        _overlayPicture.EffectTime = (float)CustomField("Effect Time: " , _overlayPicture.EffectTime , Vector2.down * 120);

    }
}
