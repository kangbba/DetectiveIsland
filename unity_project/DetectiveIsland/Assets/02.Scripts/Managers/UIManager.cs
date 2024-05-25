using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{   
    public static UIParent _uiParent;
    public static Canvas MainCanvas => _uiParent.MainCanvas;
    public static CanvasRenderer MainCanvasRenderer => _uiParent.MainCanvasRenderer;

    public static void Load(){
        GameObject uiParentPrefab = Resources.Load<GameObject>("UIPrefabs/UIParentPrefab");
        _uiParent = GameObject.Instantiate(uiParentPrefab).GetComponent<UIParent>();
        if(_uiParent == null){
            Debug.LogError("_uiParent is null");
            return;
        }
        _uiParent.Init();
    }

    //아이템 체크 판넬
    public static void OpenItemOwnPanel(ItemData itemData){
        _uiParent.ItemOwnPanel.OpenPanel(itemData);
    }
    public static void CloseItemOwnPanel(){
        _uiParent.ItemOwnPanel.ClosePanel();
    }

    //아이템 소유 판넬
    public static void OpenItemDemandPanel(){
        _uiParent.ItemDemandPanel.OpenPanel();
    }
    public static void CloseItemDemandPanel(){
        _uiParent.ItemDemandPanel.ClosePanel();
    }

    //아이템 요구 판넬

    public static async UniTask<ItemData> OpenItemDemandPanelAndWait(){
        return await _uiParent.ItemDemandPanel.OpenItemDemandPanelAndWait();
    }


    //대화 출력 판넬
    public static void OpenDialoguePanel(float totalTime){
        _uiParent.DialoguePanel.OpenPanel(totalTime);
    }

    public static void CloseDialoguePanel(float totalTime){
         _uiParent.DialoguePanel.ClosePanel(totalTime);
    }

    public static void ClearDialoguePanel(){
         _uiParent.DialoguePanel.ClearPanel();
    }
    public static async UniTask TypeDialogueTask(Dialogue dialogue){

    
        await  _uiParent.DialoguePanel.TypeDialogueTask(dialogue);
    } 
    public static async UniTask WaitForDialogueExitButton(){
        await  _uiParent.DialoguePanel.WaitForDialogueExitButton();
    }
    

    public static void SetDialogueCharacterText(string s, Color c){
        _uiParent.DialoguePanel.SetCharacterText(s, c);
    }

    //선택지 판넬

    public static async UniTask<Choice> MakeChoiceBtnsAndWait(ChoiceSet choiceSet){
        return await  _uiParent.ChoiceSetPanel.MakeChoiceBtnsAndWait(choiceSet);
    }
    public static void SetPlaceUIState(EPlaceUIPanelState placeUIPanelState, float totalTime){
        _uiParent.PlaceUIPanel.SetUIState(placeUIPanelState, totalTime);
    }

    //시간, 이벤트 판넬
    public static void SetEventTime(EventTime eventTime)
    {
        _uiParent.EventTimeDisplayer.SetEventTime(eventTime);
    }

    // 오버레이 문장 출력
    public static async UniTask DisplayOverlaySentence(OverlaySentence overlaySentence)
    {
        await _uiParent.OverlaySentenceDisplayer.DisplayOverlaySentence(overlaySentence);
    }

    // 오버레이 문장 출력
    public static async UniTask ShowSimpleDialogue(string[] sentences)
    {
        await _uiParent.SimpleDialoguePanel.ShowSimpleDialogue(sentences);
    }

    // 마우스 커서 모드 설정
    public static void SetMouseCursorMode(EMouseCursorMode mode)
    {
        _uiParent.UIMouseCursor.SetState(mode);
    }

}
