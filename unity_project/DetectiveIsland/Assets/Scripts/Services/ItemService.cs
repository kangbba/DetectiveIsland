using System.Collections;
using System.Collections.Generic;
using Aroka.ArokaUtils;
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
    public static IEnumerator AwaitItemBtnSelectedRoutine(){
        _itemPanel.Initialize(_itemDatas, false);
        SetOnPanel(true, 1f);
        yield return new WaitForSeconds(1f);
        yield return _itemPanel.AwaitItemBtnSelectedRoutine();
        SetOnPanel(false, 1f);
        yield return _itemPanel.SelectedItemData;
    }

}
