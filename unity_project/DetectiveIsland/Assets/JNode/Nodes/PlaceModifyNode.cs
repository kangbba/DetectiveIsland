using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class PlaceModifyNode : Node
{
    private PlaceModify _placeModify = new PlaceModify(true, "");

    public PlaceModifyNode(string title, Node parentNode): base(title, parentNode)  // Node 클래스의 생성자 호출
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        return _placeModify;
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }
    public override void DrawNode()
    {
        base.DrawNode();

        _placeModify.IsGain = (bool)CustomField("IsGain : ", _placeModify.IsGain, Vector2.up * 20);
        _placeModify.Id = (string)CustomField("ID : ", _placeModify.Id, Vector2.up * 0);
        
        ParentConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.min.y), NodeColor.placeModifyColor);
        ChildConnectingPoint.DrawSingleConnectionPoint(NodeRect.center.ModifiedY(NodeRect.max.y), NodeColor.placeModifyColor);
    }
}
