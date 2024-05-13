using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class OverlayPictureNode : Node
{
    private OverlayPicture _overlayPicture = new OverlayPicture("Black", "FadeIn", 1);

    public OverlayPictureNode(string title, Node parentNode): base(title, parentNode)  // Node 클래스의 생성자 호출
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        return _overlayPicture;
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }
    public override void DrawNode()
    {
        base.DrawNode();
        _overlayPicture.PictureID = (string)CustomField("Picture ID: " , _overlayPicture.PictureID ,  Vector2.down * 40);
        _overlayPicture.EffectID = (string)CustomField("Effect ID: " , _overlayPicture.EffectID , Vector2.down * 80);
        _overlayPicture.EffectTime = (float)CustomField("Effect Time: " , _overlayPicture.EffectTime , Vector2.down * 120);

        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColor.overlayPictureColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColor.overlayPictureColor);
    }
}
