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

    public override float Height => 200;

    private PlaceModify _placeModify = new PlaceModify(true, "");

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

        _placeModify.IsGain = (bool)CustomField("IsGain : ", _placeModify.IsGain, Vector2.up * 20);
        _placeModify.Id = (string)CustomField("ID : ", _placeModify.Id, Vector2.up * 0);
    }
}
