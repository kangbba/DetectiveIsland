using UnityEngine;

public class AudioActionNode : Node
{
    public AudioFileID _audioFileID = AudioFileID.None;

    public AudioActionNode(string nodeID, string title, string parentNodeID) : base(nodeID, title, parentNodeID)
    {
    }

    public override float Width { get; set; }
    public override float Height { get; set; }

    public override Element ToElement()
    {
        return new AudioAction(_audioFileID.ToString());
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

        _audioFileID = (AudioFileID)JInterface.SimpleField
        (
           title: "AudioAction ID : ",
           value: _audioFileID,
           pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
           labelWidth: 100,
           fieldWidth: 80,
           fieldHeight: textFieldHeight
        );

        Height += textFieldHeight;
        Height += BOTTOM_MARGIN;
        SetNodeRectSize(new Vector2(Width, Height));
    }
}
