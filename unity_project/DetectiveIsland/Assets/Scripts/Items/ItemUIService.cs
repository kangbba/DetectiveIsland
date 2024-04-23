using System;
using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using Aroka.EaseUtils;
using UnityEngine;
using UnityEngine.UI;

public static class ItemUIService
{
    private static ItemCheckPanel _itemCheckPanel;
    private static ItemDemandPanel _itemDemandPanel;
    private static ItemOwnPanel _itemOwnPanel;

    public static ItemCheckPanel ItemCheckPanel { get => _itemCheckPanel; }
    public static ItemDemandPanel ItemDemandPanel { get => _itemDemandPanel; }

    public static void Load(){
        _itemCheckPanel = UIManager.Instance.ItemCheckPanel;
        _itemOwnPanel = UIManager.Instance.ItemOwnPanel;
    }
   
    // ItemCheckPanel을 통해 패널을 표시하는 메소드
    public static void ShowItemCheckPanel()
    {   
        List<ItemData> itemDatas = ItemService.GetOwnItemDatas();
        _itemCheckPanel.Initialize(itemDatas);
        _itemCheckPanel.ShowPanelOn(true, .3f);
    }

    // ItemCheckPanel을 통해 패널을 숨기는 메소드
    public static void HideItemCheckPanel()
    {
        _itemCheckPanel.ShowPanelOn(false, .3f);
    }

    // ItemDemandPanel을 통해 패널을 표시하는 메소드
    public static void ShowItemDemandPanel(List<ItemData> itemDatas)
    {
        _itemDemandPanel.Initialize(itemDatas);
        _itemDemandPanel.ShowPanelOn(true, .3f);
    }

    // ItemDemandPanel을 통해 패널을 숨기는 메소드
    public static void HideItemDemandPanel()
    {
        _itemDemandPanel.ShowPanelOn(false, 3f);
    }

    public static void ShowItemOwnPanel(ItemData itemData){
        if(itemData != null){
             _itemOwnPanel.ShowItem(itemData); 
        }
        _itemOwnPanel.SetOn(true, .3f);
    }
    public static void HideItemOwnPanel(){
        _itemOwnPanel.SetOn(false, .3f);
    }
    public static IEnumerator ItemDemandRoutine(string targetItemID, Func<IEnumerator> successAction, Func<IEnumerator> failAction)
    {
        var ownItems = ItemService.GetOwnItemDatas();
        ShowItemDemandPanel(ownItems); // 보여주는데 필요한 시간 매개변수 추가

        ItemData selectedItemData = null;
        yield return CoroutineUtils.AwaitCoroutine<ItemData>(_itemDemandPanel.AwaitItemDataSelectedRoutine(), result =>
        {
            selectedItemData = result;
            Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");
        });

        HideItemDemandPanel(); // 숨기는데 필요한 시간 매개변수 추가

        if (selectedItemData == null)
        {
            Debug.LogError("선택된 아이템이없는상태로 빠져나왔음.");
            yield break;
        }
        else if (selectedItemData.ItemID == targetItemID)
        {
            Debug.Log("정답이므로 elements 처리 후 이 루틴을 빠져나갈 예정");
            yield return CoroutineUtils.StartCoroutine(successAction());
        }
        else
        {
            Debug.Log("오답이므로 elements 처리 후 이 루틴이 반복될 예정");
            yield return CoroutineUtils.StartCoroutine(failAction());
        }
    }

}
