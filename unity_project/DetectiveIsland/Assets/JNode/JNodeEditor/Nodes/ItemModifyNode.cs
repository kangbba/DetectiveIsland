using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class ItemModifyNode : Node
{
    public const float UPPER_MARGIN = 30;
    public const float BOTTOM_MARGIN = 30; 
    public const float LEFT_MARGIN = 30;
    public const float RIGHT_MARGIN = 30;
    private ItemModify _itemModify = new ItemModify(true, "", 1);

    public override float Width => throw new System.NotImplementedException();

    public override float Height => throw new System.NotImplementedException();

    public ItemModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
    }

    public override Element ToElement()
    {
        return _itemModify;
    }
    public override void DrawNode()
    {
        base.DrawNode();

            
       _itemModify.IsGain = (bool)JInterface.SimpleField(
            value: _itemModify.IsGain,
            pos: new Vector2(NodeRect.position.x, 0),
            title: "IsGain : ",
            labelWidth: 100,
            fieldWidth: 100,
            height: 20
        );

        _itemModify.Id = (string)JInterface.SimpleField(
            value: _itemModify.Id,
            pos: new Vector2(NodeRect.position.x, 20),
            title: "Id : ",
            labelWidth: 100,
            fieldWidth: 100,
            height: 20
        );

        _itemModify.Amount = (int)JInterface.SimpleField(
            value: _itemModify.Amount,
            pos: new Vector2(NodeRect.position.x, 40),
            title: "Amount : ",
            labelWidth: 100,
            fieldWidth: 100,
            height: 20
        );
        
    }
}