using System.Collections;
using System.Collections.Generic;
using Aroka.ArokaUtils;
using Aroka.CoroutineUtils;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public static class ItemService 
{
    private static ItemPanel _itemPanel;
    private static ItemOwnPanel _itemOwnPanel;
    private static List<ItemData> _itemDatas;
    private const string ItemOwnershipKeyPrefix = "item_owned_";  // 아이템 소유 정보의 키 접두어

    public static void Load()
    {       
        _itemPanel = UIManager.Instance.ItemPanel;
        _itemOwnPanel = UIManager.Instance.ItemOwnPanel;
        _itemDatas = ArokaUtils.LoadScriptableDatasFromFolder<ItemData>("ItemDatas");
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
    //<return string>
    public static IEnumerator ItemDemandRoutine(ItemDemand itemDemand){
        foreach(Dialogue dialogue in itemDemand.Dialogues){
            yield return CoroutineUtils.StartCoroutine(DialogueService.DialogueRoutine(dialogue));
        }

        var ownItems = GetOwnItemDatas();
        if(ownItems != null && ownItems.Count > 0){
            _itemPanel.Initialize(ownItems, false);
            SetOnPanel(true, 0.1f);
            yield return new WaitForSeconds(.5f);
            ItemData selectedItemData = null;
            yield return CoroutineUtils.AwaitCoroutine<ItemData>(_itemPanel.AwaitItemBtnSelectedRoutine(), result => {
                selectedItemData = result;
                Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");
            });
            SetOnPanel(false, 0.1f);
            bool isCorrect = selectedItemData != null && selectedItemData.ItemID == itemDemand.ItemID;
            yield return isCorrect;
        }
        else{
            Debug.LogError("ownItems 없어서 일단 맞았다고 치겠음");
            yield return true; //맞았다 치자.
        }
      
    }
    
    public static void LoseAllItems()
    {
        foreach(ItemData itemData in _itemDatas){
            OwnItem(itemData.ItemID, false);
        }
    }

    public static List<ItemData> GetOwnItemDatas()
    {
        // PlayerPrefs에서 아이템 소유 여부를 확인하여 소유한 아이템만 반환
        List<ItemData> itemDatasOwn = _itemDatas.FindAll(item => PlayerPrefs.GetInt(ItemOwnershipKeyPrefix + item.ItemID, 0) == 1);

        return itemDatasOwn ?? new List<ItemData>();
    }
    public static bool GetIfOwnItem(string itemID){
        
        ItemData item = GetItemData(itemID);
        if (item != null){
            return PlayerPrefs.GetInt(ItemOwnershipKeyPrefix + item.ItemID) == 1;
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
            PlayerPrefs.SetInt(ItemOwnershipKeyPrefix + item.ItemID, own ? 1 : 0);
            PlayerPrefs.Save();  // 변경 사항을 즉시 저장
        }
    }
 // 아이템 획득 후 정보를 보여주는 코루틴
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
                _itemOwnPanel.SetOn(true, 0.1f); // 패널을 활성화
                _itemOwnPanel.ShowItem(itemData); // SetMessage 메서드는 ItemPanel에 정의되어 있어야 함
                
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                // 패널 비활성화
                _itemOwnPanel.SetOn(false, 0.1f);
            }
            else if(assetChange.GainType == "Lose"){

            }

        }
       
    }
}
