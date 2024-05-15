using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class FriendshipModifyNode : Node
{
    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    private FriendshipModify _friendshipModify = new FriendshipModify(true, "", 10);

    public override float Width => 300;

    public override float Height => 100;

    public FriendshipModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }
    public override Element ToElement()
    {
        return _friendshipModify;
    }

    public override void DrawNode()
    {
        base.DrawNode();

        float y = UPPER_MARGIN;

        _friendshipModify.IsGain = (bool)JInterface.SimpleField
        (
            value : _friendshipModify.IsGain,
            pos : new Vector2(NodeRect.position.x, y),
            title: "IsGain : ",
            labelWidth : 100,
            fieldWidth : 20,
            height : 20
        );

        y += 100;

        _friendshipModify.Id = (string)JInterface.SimpleField
        (
            value : _friendshipModify.Id,
            pos : new Vector2(NodeRect.position.x, y),
            title: "ID : ",
            labelWidth : 100,
            fieldWidth : 100,
            height : 20
        );

        y += 100;

        _friendshipModify.Amount = (int)JInterface.SimpleField
        (
            value : _friendshipModify.Amount,
            pos : new Vector2(NodeRect.position.x, y),
            title: "ID : ",
            labelWidth : 100,
            fieldWidth : 100,
            height : 20
        );

        y += 100;

        y += BOTTOM_MARGIN;
    }
}
