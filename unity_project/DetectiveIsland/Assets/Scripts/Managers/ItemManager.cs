using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Manager<ItemData>
{
    public override void Initialize(string folderName, GameObject mainPanel)
    {
        base.Initialize(folderName, mainPanel);
        Debug.Log("PlaceManager initialized with place panel: " + MainPanel.name);
    }
    public ItemData GetItemData(string itemID)
    {
        foreach (ItemData item in _dataList)
        {
            if (item.ItemID == itemID)
            {
                return item;
            }
        }
        return null;
    }
}
