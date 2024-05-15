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

        _itemModify.IsGain = (bool)CustomField("IsGain : ", _itemModify.IsGain, Vector2.down * 0);
        _itemModify.Id = (string)CustomField("Id : ", _itemModify.Id, Vector2.down * 20);
        _itemModify.Amount = (int)CustomField("Amount : ", _itemModify.Amount, Vector2.down * 40);
        
    }
}