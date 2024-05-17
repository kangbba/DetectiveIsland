using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;
using System;

[System.Serializable]
public class GainFriendshipNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    public GainFriendship GainFriendship = new GainFriendship(true, "", 10);
    public override float Width { get; set; }
    public override float Height { get; set; }

    public override Node Clone()
    {
        return new GainFriendshipNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }

    public GainFriendshipNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {

    }
    public override Element ToElement()
    {
        return GainFriendship;
    }
    public override void DrawNode()
    {
        base.DrawNode();

        Width = 300;
        Height = UPPER_MARGIN;
        
        GainFriendship.IsGain = (bool)JInterface.SimpleField
        (
            value : GainFriendship.IsGain,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "IsGain : ",
            labelWidth : 100,
            fieldWidth : 20,
            fieldHeight : 20
        );

        Height += 20;

        GainFriendship.CharacterID = (string)JInterface.SimpleField
        (
            value : GainFriendship.CharacterID,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "CharacterID : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : 20
        );

        Height += 20;

        GainFriendship.Amount = (int)JInterface.SimpleField
        (
            value : GainFriendship.Amount,
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
