using UnityEngine;
using UnityEditor;
using Aroka.ArokaUtils;

[System.Serializable]
public class ItemModifyNode : Node
{
    private ItemModify _itemModify = new ItemModify(true, "", 1);
   
    public ItemModifyNode(string id, string title, string parentNodeID) : base(id, title, parentNodeID)
    {
        SetNodeRectSize(CalNodeSize());
    }

    public override Element ToElement()
    {
        return _itemModify;
    }

    public override Vector2 CalNodeSize()
    {
        return new Vector2(200, 100);
    }
    public override void DrawNode()
    {
        base.DrawNode();

        _itemModify.IsGain = (bool)CustomField("IsGain : ", _itemModify.IsGain, Vector2.down * 0);
        _itemModify.Id = (string)CustomField("Id : ", _itemModify.Id, Vector2.down * 20);
        _itemModify.Amount = (int)CustomField("Amount : ", _itemModify.Amount, Vector2.down * 40);
        
    }
}