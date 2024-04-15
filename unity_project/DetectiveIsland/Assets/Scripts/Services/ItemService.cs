using System.Collections;
using System.Collections.Generic;
using ArokaUtil;
using UnityEngine;

public class ItemService 
{
    private ItemPanel _itemPanel;
    private List<ItemData> _itemDatas;

    public void Initialize()
    {       
        _itemPanel = UIManager.Instance.ItemPanel;
        _itemDatas = Utils.LoadDatasFromFolder<ItemData>("ItemDatas");
    }
    public ItemData GetItemData(string itemID)
    {
        foreach (ItemData item in _itemDatas)
        {
            if (item.ItemID == itemID)
            {
                return item;
            }
        }
        return null;
    }
    public void SetOnPanel(bool b, float totalTime){
        _itemPanel.SetAnim(b, totalTime);
    }
}
