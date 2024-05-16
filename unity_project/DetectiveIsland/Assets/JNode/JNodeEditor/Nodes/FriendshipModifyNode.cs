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
    private bool _isGain = true;
    private string _characterID = "";
    private int _amount =10;
    public override float Width { get; set; }
    public override float Height { get; set; }


    public FriendshipModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {

    }
    public override Element ToElement()
    {
        return new FriendshipModify(_isGain, _characterID, _amount);
    }
    public override void DrawNode()
    {
        base.DrawNode();

        Width = 300;
        Height = UPPER_MARGIN;
        
        _isGain = (bool)JInterface.SimpleField
        (
            value : _isGain,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "IsGain : ",
            labelWidth : 100,
            fieldWidth : 20,
            fieldHeight : 20
        );

        Height += 20;

        _characterID = (string)JInterface.SimpleField
        (
            value : _characterID,
            pos : new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "CharacterID : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : 20
        );

        Height += 20;

        _amount = (int)JInterface.SimpleField
        (
            value : _amount,
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
