using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class FriendshipModifyNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public FriendshipModify FriendshipModify = new FriendshipModify(true, "", 10);
    public override float Width { get; set; }
    public override float Height { get; set; }


    public FriendshipModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {

    }
    public override Element ToElement()
    {
        return FriendshipModify;
    }
    public override void DrawNode()
    {
        base.DrawNode();

        Width = 300;
        Height = UPPER_MARGIN;
        
        FriendshipModify.IsGain = (bool)JInterface.SimpleField
        (
            value : FriendshipModify.IsGain,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "IsGain : ",
            labelWidth : 100,
            fieldWidth : 20,
            fieldHeight : 20
        );

        Height += 20;

        FriendshipModify.CharacterID = (string)JInterface.SimpleField
        (
            value : FriendshipModify.CharacterID,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "CharacterID : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : 20
        );

        Height += 20;

        FriendshipModify.Amount = (int)JInterface.SimpleField
        (
            value : FriendshipModify.Amount,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "Amount : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : 20
        );

        Height += 20;
        Height += BOTTOM_MARGIN;
        SetNodeRectSize(Width, Height);
    }
}
