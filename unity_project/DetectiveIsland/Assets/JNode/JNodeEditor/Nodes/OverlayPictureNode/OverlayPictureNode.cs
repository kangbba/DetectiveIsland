using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;
using System;

[System.Serializable]
public class OverlayPictureNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30;
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width { get; set; }
    public override float Height { get; set; }
  

    public EPictureID _pictureID = EPictureID.None;
    public EPictureEffectID _effectID = EPictureEffectID.None;
    public float _effectTime = 0;

    public const float FIELD_COMMON_HEIGHT = 20;

    public OverlayPictureNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    { 
    }
    public override Node Clone()
    {
        return new OverlayPictureNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }

    public override Element ToElement()
    {
        return new OverlayPicture(_pictureID, _effectID, _effectTime);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = 300;
        Height = UPPER_MARGIN;

        _pictureID = (EPictureID)JInterface.SimpleField
        (
            title: "Picture ID: ",
            value: _pictureID,
            pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
            labelWidth: 100,
            fieldWidth: 80,
            fieldHeight: FIELD_COMMON_HEIGHT
        );

        Height += FIELD_COMMON_HEIGHT;
        _effectTime = (float)JInterface.SimpleField
      (
          title: "Effect Time: ",
          value: _effectTime,
          pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
          labelWidth: 100,
          fieldWidth: 80,
          fieldHeight: FIELD_COMMON_HEIGHT
      );

        Height += FIELD_COMMON_HEIGHT;
        _effectID = (EPictureEffectID)JInterface.SimpleField
       (
           title: "Effect ID: ",
           value: _effectID,
           pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
           labelWidth: 100,
           fieldWidth: 80,
           fieldHeight: FIELD_COMMON_HEIGHT
       );

        Height += FIELD_COMMON_HEIGHT;

        Height += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, Height));
    }
}
