using System.Collections;
using System.Collections.Generic;
using ArokaUtil;
using UnityEngine;

public static class ItemService 
{
    private static ItemPanel _itemPanel;
    private static List<ItemData> _itemDatas;

    public static void Initialize()
    {       
        _itemPanel = UIManager.Instance.ItemPanel;
        _itemDatas = ArokaUtils.LoadDatasFromFolder<ItemData>("ItemDatas");
    }
    public static ItemData GetItemData(string itemID)
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
    public static void SetOnPanel(bool b, float totalTime){
        _itemPanel.SetAnim(b, totalTime);
    }
}
