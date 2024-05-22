using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    private static ItemCheckPanel _itemCheckPanel;
    private static ItemDemandPanel _itemDemandPanel;
    private static ItemOwnPanel _itemOwnPanel;
    private static DialoguePanel _dialoguePanel;
    private static ChoiceSetPanel _choiceSetPanel;
    private static EventTimeDisplayer _eventTimeDisplayer;
    private static PlaceUIPanel _placeUIPanel;

    private static UIMouseCursor _uiMouseCursor;

    private static OverlaySentenceDisplayer _overlaySentenceDisplayer;
    private static Button _itemCheckPanelEnterBtn;

    public static OverlaySentenceDisplayer OverlaySentenceDisplayer { get => _overlaySentenceDisplayer; }

    public static void Load(){
        GameObject uiPrefabToSpawn = Resources.Load<GameObject>("UIPrefabs/UISetPrefab");

        GameObject uiPrefab = GameObject.Instantiate(uiPrefabToSpawn);
        _itemCheckPanel = uiPrefab.GetComponentInChildren<ItemCheckPanel>();
        _itemDemandPanel = uiPrefab.GetComponentInChildren<ItemDemandPanel>();
        _itemOwnPanel = uiPrefab.GetComponentInChildren<ItemOwnPanel>();
        _dialoguePanel = uiPrefab.GetComponentInChildren<DialoguePanel>();
        _choiceSetPanel = uiPrefab.GetComponentInChildren<ChoiceSetPanel>();
        _eventTimeDisplayer = uiPrefab.GetComponentInChildren<EventTimeDisplayer>();
        _placeUIPanel = uiPrefab.GetComponentInChildren<PlaceUIPanel>();
        _uiMouseCursor = uiPrefab.GetComponentInChildren<UIMouseCursor>();
        _uiMouseCursor.Initialize(uiPrefab.GetComponent<Canvas>());
        _overlaySentenceDisplayer = uiPrefab.GetComponentInChildren<OverlaySentenceDisplayer>();

    }
    //아이템 체크 판넬
    public static void OpenItemOwnPanel(ItemData itemData){
        _itemOwnPanel.OpenPanel(itemData);
    }
    public static void CloseItemOwnPanel(){
        _itemOwnPanel.ClosePanel();
    }

    //아이템 소유 판넬
    public static void OpenItemDemandPanel(){
        _itemDemandPanel.OpenPanel();
    }
    public static void CloseItemDemandPanel(){
        _itemDemandPanel.ClosePanel();
    }

    //아이템 요구 판넬

    public static async UniTask<ItemData> OpenItemDemandPanelAndWait(){
        return await _itemDemandPanel.OpenItemDemandPanelAndWait();
    }


    //대화 출력 판넬
    public static void OpenDialoguePanel(float totalTime){
        _dialoguePanel.OpenPanel(totalTime);
    }

    public static void CloseDialoguePanel(float totalTime){
        _dialoguePanel.ClosePanel(totalTime);
    }

    public static void ClearDialoguePanel(){
        _dialoguePanel.ClearPanel();
    }
    public static async UniTask TypeLineTask(string str, Color c){
        await _dialoguePanel.TypeLineTask(str, c);
    }

    public static void SetDialogueCharacterText(string s, Color c){
        _dialoguePanel.SetCharacterText(s, c);
    }

    //선택지 판넬

    public static async UniTask<Choice> MakeChoiceBtnsAndWait(ChoiceSet choiceSet){
        return await _choiceSetPanel.MakeChoiceBtnsAndWait(choiceSet);
    }
    public static void SetPlaceUIState(EPlaceUIPanelState placeUIPanelState, float totalTime){
        _placeUIPanel.SetUIState(placeUIPanelState, totalTime);
    }

    //시간, 이벤트 판넬
    public static void SetEventTime(EventTime eventTime)
    {
        _eventTimeDisplayer.SetEventTime(eventTime);
    }

}
