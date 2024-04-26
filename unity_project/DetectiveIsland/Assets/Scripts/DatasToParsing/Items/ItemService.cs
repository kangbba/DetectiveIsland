using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Aroka.CoroutineUtils;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public enum EItemDemandResult{
    Correct,
    NotCorrect,
    Canceled
}
public static class ItemService 
{
    private static List<ItemData> _itemDatas;
    private const string ITEM_OWNED_KEY = "item_owned_";  // 아이템 소유 정보의 키 접두어

    public static void Load()
    {       
        _itemDatas = ArokaUtils.LoadScriptableDatasFromFolder<ItemData>("ItemDatas");
    }
    public static ItemData GetItemData(string itemID)
    {
        return _itemDatas.FirstOrDefault(itemData => itemData.ItemID == itemID);
    }
    
    public static void LoseAllItems()
    {
        foreach(ItemData itemData in _itemDatas){
            OwnItem(itemData.ItemID, false);
        }
    }

    public static List<ItemData> GetOwnItemDatas()
    {
        List<ItemData> itemDatasOwn = _itemDatas.FindAll(item => PlayerPrefs.GetInt(ITEM_OWNED_KEY + item.ItemID, 0) == 1);
        return itemDatasOwn ?? new List<ItemData>();
    }
    
    public static bool IsOwnItem(string itemID){
        
        ItemData item = GetItemData(itemID);
        if (item != null){
            return PlayerPrefs.GetInt(ITEM_OWNED_KEY + item.ItemID) == 1;
        }
        else{
            Debug.LogError("해당 아이템 아이디의 아이템 찾을수없음");
            return false;
        }
    }

    public static void OwnItem(string itemID, bool own)
    {
        // 특정 아이템의 소유 여부를 설정
        ItemData item = GetItemData(itemID);
        if (item != null)
        {
            PlayerPrefs.SetInt(ITEM_OWNED_KEY + item.ItemID, own ? 1 : 0);
            PlayerPrefs.Save();  // 변경 사항을 즉시 저장
        }
    }
    
    public static IEnumerator AssetChangeRoutine(AssetChange assetChange)
    {
        ItemData itemData = GetItemData(assetChange.ItemID);
        if (itemData == null)
        {
            Debug.LogError("AssetChangeRoutine called with null ItemData");
            yield break;
        }
        // 아이템 획득 정보를 UI 패널에 설정
        string message = $"{itemData.ItemNameForUser}을(를) 획득했습니다!";

        if(itemData.ItemID == "FriendShip"){

            if(assetChange.GainType == "Gain"){
                
            }
            else if(assetChange.GainType == "Lose"){
            }

        }
        else{ 
            OwnItem(itemData.ItemID, true);
            if(assetChange.GainType == "Gain"){
                ItemUIService.ShowItemOwnPanel(itemData);
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                ItemUIService.HideItemOwnPanel();
            }
            else if(assetChange.GainType == "Lose"){

            }

        }
       
    }
}
