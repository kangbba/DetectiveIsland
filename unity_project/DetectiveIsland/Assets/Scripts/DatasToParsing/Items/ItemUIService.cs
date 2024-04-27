using System;
using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using Aroka.EaseUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class ItemUIService
{
    private static ItemCheckPanel _itemCheckPanel;
    private static ItemDemandPanel _itemDemandPanel;
    private static ItemOwnPanel _itemOwnPanel;
    private static Button _itemCheckPanelEnterBtn;

    public static ItemCheckPanel ItemCheckPanel { get => _itemCheckPanel; }
    public static ItemDemandPanel ItemDemandPanel { get => _itemDemandPanel; }

    public static void Load(){

        _itemCheckPanel = UIManager.Instance.ItemCheckPanel;
        _itemDemandPanel = UIManager.Instance.ItemDemandPanel;
        _itemOwnPanel = UIManager.Instance.ItemOwnPanel;
        _itemCheckPanelEnterBtn = UIManager.Instance.ItemCheckPanelEnterBtn;

        _itemCheckPanelEnterBtn.onClick.RemoveAllListeners();
        _itemCheckPanelEnterBtn.onClick.AddListener(ShowItemCheckPanel);
    }
   
    // ItemCheckPanel을 통해 패널을 표시하는 메소드
    public static void ShowItemCheckPanel()
    {   
        List<ItemData> itemDatas = ItemService.GetOwnItemDatas();
        _itemCheckPanel.Initialize(itemDatas);
        _itemCheckPanel.OpenPanel(true, .3f);
    }
    public static void HideItemCheckPanel()
    {
        _itemCheckPanel.OpenPanel(false, .3f);
    }

    // ItemDemandPanel을 통해 패널을 표시하는 메소드
    public static void ShowItemDemandPanel(List<ItemData> itemDatas)
    {
        _itemDemandPanel.Initialize(itemDatas);
        _itemDemandPanel.OpenPanel(true, .3f);
    }
    public static void HideItemDemandPanel()
    {
        _itemDemandPanel.OpenPanel(false, .3f);
    }

    // ItemCheckPanel을 통해 ItemCheckPanelButton을 표시하는 메소드
    public static void ShowItemCheckPanelEnterButton()
    {   
        _itemCheckPanelEnterBtn.GetComponent<ArokaAnim>().SetAnim(true, .3f);
    }

    public static void HideItemCheckPanelEnterButton()
    {   
        _itemCheckPanelEnterBtn.GetComponent<ArokaAnim>().SetAnim(false, .3f);
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
    public static async UniTask ItemDemandTask(string targetItemID, Func<UniTask> successTask, Func<UniTask> failTask, Func<UniTask> cancelTask)
    {
        while(true){
            var ownItems = ItemService.GetOwnItemDatas();
            ShowItemDemandPanel(ownItems); // 보여주는데 필요한 시간 매개변수 추가

            ItemData selectedItemData = await _itemDemandPanel.AwaitItemDataSelectedTask();
            HideItemDemandPanel(); // 숨기는데 필요한 시간 매개변수 추가

            if (selectedItemData == null)
            {
                Debug.Log("취소 되었음 .");
                await cancelTask;
                break;
            }
            else if (selectedItemData.ItemID == targetItemID)
            {
                Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");
                Debug.Log("정답이므로 elements 처리 후 이 루틴을 빠져나갈 예정");
                await successTask;
                break;
            }
            else
            {
                Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");
                Debug.Log("오답이므로 elements 처리 후 이 루틴이 반복될 예정");
                await failTask;
            }
        }
        
    }

}
