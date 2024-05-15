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
        _friendshipModify.IsGain = (bool)CustomField("IsGain : ", _friendshipModify.IsGain, Vector2.up * 200);
        _friendshipModify.Id = (string)CustomField("Id : ", _friendshipModify.Id, Vector2.up * 0);
        _friendshipModify.Amount = (int)CustomField("Amount : ", _friendshipModify.Amount, Vector2.up * -200);


    }
}
