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

    private bool _isGain = true;
    private string _id = "";
    private int _amount = 1;
    public override float Width => stackedWidth;

    public override float Height => stackedHeight;

    public ItemModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return new ItemModify(_isGain, _id, _amount);
    }
    private float stackedHeight;

    private float stackedWidth;
    public override void DrawNode()
    {
        base.DrawNode();
        stackedWidth = 300;
        stackedHeight = UPPER_MARGIN;
        float standardFieldHeight = 20;
        _isGain = (bool)JInterface.SimpleField(
            value: _isGain,
            pos: new Vector2(NodeRect.position.x + 10 , NodeRect.position.y + stackedHeight),
            title: "IsGain : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        stackedHeight += standardFieldHeight;

        _id = (string)JInterface.SimpleField(
            value: _id,
            pos: new Vector2(NodeRect.position.x + 10, NodeRect.position.y + stackedHeight),
            title: "Id : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        stackedHeight += standardFieldHeight;

        _amount = (int)JInterface.SimpleField(
            value: _amount,
            pos: new Vector2(NodeRect.position.x + 10, NodeRect.position.y + stackedHeight),
            title: "Amount : ",
            labelWidth: 100,
            fieldWidth: 100,
            fieldHeight: standardFieldHeight
        );
        stackedHeight += standardFieldHeight;
        stackedHeight += BOTTOM_MARGIN;
        SetNodeRectSize(Width, Height);

    }
}