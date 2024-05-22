using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public enum EItemID{
    None = 0,
    Coffee = 1,
}
public enum EItemDemandResult{
    Correct,
    NotCorrect,
    Canceled
}
public static class ItemService
{
    private static List<ItemData> _itemDatas;

    public static void Load()
    {       
        _itemDatas = ArokaUtils.LoadScriptableDatasFromFolder<ItemData>("ItemDatas");
    }
    public static ItemData GetItemData(EItemID itemID)
    {
        return _itemDatas.FirstOrDefault(itemData => itemData.ItemID == itemID);
    }
}
