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
    public override float Width { get; set; }
    public override float Height { get; set; }

    public bool IsGain;
    public ECharacterID ID;
    public int Amount;

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
        return new GainFriendship(IsGain, ID, Amount);
    }
    public override void DrawNode()
    {
        base.DrawNode();

        Width = 300;
        Height = UPPER_MARGIN;
        
        IsGain = (bool)JInterface.SimpleField
        (
            value : IsGain,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "IsGain : ",
            labelWidth : 100,
            fieldWidth : 20,
            fieldHeight : 20
        );

        Height += 20;

        ID = (ECharacterID)JInterface.SimpleField
        (
            value : ID,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "CharacterID : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : 20
        );

        Height += 20;

        Amount = (int)JInterface.SimpleField
        (
            value : Amount,
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
