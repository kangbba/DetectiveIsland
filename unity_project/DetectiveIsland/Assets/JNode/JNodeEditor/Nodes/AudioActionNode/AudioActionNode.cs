using System;
using UnityEngine;

public class AudioActionNode : Node
{
    public EAudioFileID AudioFileID = EAudioFileID.None;

    public AudioActionNode(string nodeID, string title, string parentNodeID) : base(nodeID, title, parentNodeID)
    {
    }

    public override float Width { get; set; }
    public override float Height { get; set; }

    public override Element ToElement()
    {
        return new AudioAction(AudioFileID.ToString());
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

        AudioFileID = (EAudioFileID)JInterface.SimpleField
        (
           title: "AudioAction ID : ",
           value: AudioFileID,
           pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
           labelWidth: 100,
           fieldWidth: 80,
           fieldHeight: textFieldHeight
        );

        Height += textFieldHeight;
        Height += BOTTOM_MARGIN;
        SetNodeRectSize(new Vector2(Width, Height));
    }
    public override Node Clone()
    {
        return new AudioActionNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
        };
    }
}
