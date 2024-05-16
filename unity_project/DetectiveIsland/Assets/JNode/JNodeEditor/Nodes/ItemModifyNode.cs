using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class ItemModifyNode : Node
{
    public const float UPPER_MARGIN = 50;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;


    public ItemModify ItemModify = new ItemModify(true, "", 1);

    public override float Width { get; set; }
    public override float Height { get; set; }


    public ItemModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return ItemModify;
    }
    public override void DrawNode()
    {
        base.DrawNode();
        Width = 300;
        Height = UPPER_MARGIN;
        float standardFieldHeight = 20;
        ItemModify.IsGain = (bool)JInterface.SimpleField(
            value: ItemModify.IsGain,
            pos: new Vector2(NodeRect.position.x + 10 , NodeRect.position.y + Height),
            title: "IsGain : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        Height += standardFieldHeight;

        ItemModify.ID = (string)JInterface.SimpleField(
            value: ItemModify.ID,
            pos: new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "Id : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        Height += standardFieldHeight;

        ItemModify.Amount = (int)JInterface.SimpleField(
            value: ItemModify.Amount,
            pos: new Vector2(NodeRect.position.x + 10, NodeRect.position.y + Height),
            title: "Amount : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        Height += standardFieldHeight;
        Height += BOTTOM_MARGIN;
        SetNodeRectSize(Width, Height);

    }
}