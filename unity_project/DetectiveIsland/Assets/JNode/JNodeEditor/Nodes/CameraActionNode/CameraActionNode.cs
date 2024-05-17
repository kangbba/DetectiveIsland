using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraActionNode : Node
{
    public CameraActionID _cameraActionID = CameraActionID.ShakeNormal;
    public float _cameraActionTime = 0.3f;

    public CameraActionNode(string nodeID, string title, string parentNodeID) : base(nodeID, title, parentNodeID)
    {
    }

    public override float Width { get; set; }
    public override float Height { get; set; }

    public override Element ToElement()
    {
        return new CameraAction(_cameraActionID.ToString(), _cameraActionTime);
    }

    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30;
    public const float LEFT_MARGIN = 20;
    public const float RIGHT_MARGIN = 30;

    public override void DrawNode()
    {
        base.DrawNode();

        Width = 300;
        Height = UPPER_MARGIN;
        float textFieldHeight = 20;

        _cameraActionID = (CameraActionID)JInterface.SimpleField
        (
           title: "CameraAction ID : ",
           value: _cameraActionID,
           pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
           labelWidth: 110,
           fieldWidth: 100,
           fieldHeight: textFieldHeight
        );

        Height += textFieldHeight + 5;

        _cameraActionTime = (float)JInterface.SimpleField
        (
            title: "CameraAction Time : ",
            value: _cameraActionTime,
            pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
            labelWidth: 128,
            fieldWidth: 100,
            fieldHeight: textFieldHeight
        );

        Height += textFieldHeight;
        Height += BOTTOM_MARGIN;
        SetNodeRectSize(new Vector2(Width, Height));
    }
}
