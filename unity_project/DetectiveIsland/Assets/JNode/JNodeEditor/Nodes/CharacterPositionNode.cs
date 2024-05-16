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

    private CharacterPosition _characterPosition = new CharacterPosition("CharacterID", "Middle");

    public override float Width => StackedWidth;

    public override float Height => StackedHeight;

    public CharacterPosition CharacterPosition { get => _characterPosition; }

    public const float CHARACTER_ID_HEIGHT = 20;
    public const float POSITION_ID_HEIGHT = 20;

    public const float DEFAULT_WIDTH = 300;
    public const float DEFAULT_HEIGHT = UPPER_MARGIN + CHARACTER_ID_HEIGHT + POSITION_ID_HEIGHT + BOTTOM_MARGIN;
    
    protected override float StackedWidth { get ; set; }
    protected override float StackedHeight { get ; set; }

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
        StackedWidth = DEFAULT_WIDTH;
        StackedHeight = UPPER_MARGIN;

        _characterPosition.CharacterID = (string)JInterface.SimpleField
        (
            value : _characterPosition.CharacterID,
            pos : new Vector2(LEFT_MARGIN + NodeRect.position.x, NodeRect.position.y + StackedHeight),
            title : "Character ID : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : CHARACTER_ID_HEIGHT
        );
        StackedHeight += CHARACTER_ID_HEIGHT;

        _characterPosition.PositionID = (string)JInterface.SimpleField
        (
            value : _characterPosition.PositionID,
            pos : new Vector2(LEFT_MARGIN + NodeRect.position.x, NodeRect.position.y + StackedHeight),
            title : "Position ID : ",
            labelWidth : 100,
            fieldWidth : 100,
            fieldHeight : POSITION_ID_HEIGHT
        );
        StackedHeight += POSITION_ID_HEIGHT;

        StackedHeight += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(StackedWidth, StackedHeight));
    }
}
