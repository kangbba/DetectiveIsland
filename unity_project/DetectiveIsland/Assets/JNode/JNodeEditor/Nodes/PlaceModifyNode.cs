using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class PlaceModifyNode : Node
{
    private PlaceModify _placeModify = new PlaceModify(true, "");

    public PlaceModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
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
    }
}
