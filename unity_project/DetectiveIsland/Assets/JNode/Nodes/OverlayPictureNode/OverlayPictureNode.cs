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
  

    public EPictureID PictureID = EPictureID.None;
    public EPictureEffectID EffectID = EPictureEffectID.None;
    public bool IsPreset = false;
    public float EffectTime = 1;

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
        return new OverlayPicture(PictureID, EffectID, IsPreset, EffectTime);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = 300;
        Height = UPPER_MARGIN;

        PictureID = (EPictureID)JInterface.SimpleField
        (
            title: "Picture ID: ",
            value: PictureID,
            pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
            labelWidth: 100,
            fieldWidth: 80,
            fieldHeight: FIELD_COMMON_HEIGHT
        );
        Height += FIELD_COMMON_HEIGHT;

        EffectTime = (float)JInterface.SimpleField
      (
          title: "Effect Time: ",
          value: EffectTime,
          pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
          labelWidth: 100,
          fieldWidth: 80,
          fieldHeight: FIELD_COMMON_HEIGHT
      );
        Height += FIELD_COMMON_HEIGHT;

        EffectID = (EPictureEffectID)JInterface.SimpleField
       (
           title: "Effect ID: ",
           value: EffectID,
           pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
           labelWidth: 100,
           fieldWidth: 80,
           fieldHeight: FIELD_COMMON_HEIGHT
       );
        Height += FIELD_COMMON_HEIGHT;

        IsPreset = (bool)JInterface.SimpleField
        (
           title: "IsPreset: ",
           value: IsPreset,
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
