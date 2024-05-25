using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class QuestNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30;
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;

    public override float Width { get; set; }
    public override float Height { get; set; }

    public QuestID QuestID;

    public const float FIELD_COMMON_HEIGHT = 20;

    public QuestNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        QuestID = QuestID.HospitalDoorPassword; // Default value, change as necessary
    }

    public override Node Clone()
    {
        return new QuestNode(Guid.NewGuid().ToString(), this.Title, this.ParentNodeID)
        {
            QuestID = this.QuestID
        };
    }

    public override Element ToElement()
    {
        return new QuestData(QuestID);
    }

    public override void DrawNode()
    {
        base.DrawNode();
        Width = 300;
        Height = UPPER_MARGIN;

        QuestID = (QuestID)JInterface.SimpleField
        (
            title: "Quest ID: ",
            value: QuestID,
            pos: new Vector2(NodeRect.position.x + LEFT_MARGIN, NodeRect.position.y + Height),
            labelWidth: 100,
            fieldWidth: 150,
            fieldHeight: FIELD_COMMON_HEIGHT
        );

        Height += FIELD_COMMON_HEIGHT;
        Height += BOTTOM_MARGIN;

        SetNodeRectSize(new Vector2(Width, Height));
    }
}
