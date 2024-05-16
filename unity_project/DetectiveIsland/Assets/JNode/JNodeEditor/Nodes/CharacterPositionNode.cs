using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class CharacterPositionNode : Node
{
    public const float UPPER_MARGIN = 20;
    public const float BOTTOM_MARGIN = 20; 
    public const float LEFT_MARGIN = 10;
    public const float RIGHT_MARGIN = 10;

    public CharacterPosition CharacterPosition = new CharacterPosition("Mono", "Middle");

    public override float Width { get; set; }
    public override float Height { get; set; }
    


    public const float CHARACTER_ID_HEIGHT = 20;
    public const float POSITION_ID_HEIGHT = 20;

    public const float DEFAULT_WIDTH = 300;
    public const float DEFAULT_HEIGHT = UPPER_MARGIN + CHARACTER_ID_HEIGHT + POSITION_ID_HEIGHT + BOTTOM_MARGIN;
    private CharacterPreviewer _characterPreviewer = new CharacterPreviewer();
    

    public CharacterPositionNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = DEFAULT_WIDTH;
        Height = UPPER_MARGIN;

        CharacterPosition.CharacterID = (string)JInterface.SimpleField
        (
            value : CharacterPosition.CharacterID,
            pos : new Vector2(LEFT_MARGIN + NodeRect.position.x + 50, NodeRect.position.y + Height),
            title : "Character ID : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : CHARACTER_ID_HEIGHT
        );
        Height += CHARACTER_ID_HEIGHT;

        CharacterPosition.PositionID = (string)JInterface.SimpleField
        (
            value : CharacterPosition.PositionID,
            pos : new Vector2(LEFT_MARGIN + NodeRect.position.x + 50, NodeRect.position.y + Height),
            title : "Position ID : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : POSITION_ID_HEIGHT
        );
        Height += POSITION_ID_HEIGHT;

        Height += BOTTOM_MARGIN;


        _characterPreviewer.CharacterPreview(CharacterPosition.CharacterID, Vector2.one * 40f , new Vector2(LEFT_MARGIN + NodeRect.position.x, NodeRect.center.y).GetAnchoredPos(Vector2.one * 40f, JAnchor.CenterLeft));

        SetNodeRectSize(new Vector2(Width, Height));
    }
}
