using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using Aroka.JsonUtils;

public static class EventProcessor
{
    private static bool _isMoving = false;

    public static async void Move(string placeID){
        if(_isMoving){
            Debug.LogWarning("이동중 또다른 Move 호출");
            return;
        }
        _isMoving = true;
        await MoveToPlaceUniTask(placeID);
        // 이동하는 로직 작성
    }
    private static async UniTask MoveToPlaceUniTask(string placeID)
    {
        UIManager uiManager = UIManager.Instance;
        Debug.Log($"----------------------------------------LOOP START----------------------------------------");
        Debug.Log($"현재 시간 : {EventTimeService.CurEventTime.ToString()}");
        Debug.Log($"장소 이동 : {placeID})");
        
        PlaceService.SetPlace(placeID, .5f);
        PlaceService.CurPlace.HidePlaceMoveBtns(0f);
        uiManager.PlaceUIPanel.ClosePanel(.5f);

        await UniTask.WaitForSeconds(.5f);

        // AwaitChoices의 결과를 직접 받아 처리
        EventPlan eventPlan = EventService.GetEventPlan(EventTimeService.CurEventTime);
        ScenarioData scenarioData = eventPlan.GetScenarioData(placeID);
        bool isEventExist = scenarioData != null;

        if(isEventExist)
        {
            Debug.Log("장소로 이동 중 (이벤트가 있는 곳)");
            Scenario scenario = ArokaJsonUtils.LoadScenario(scenarioData.ScenarioFile);
            if(!scenarioData.IsAllSolved()) // 미 해결 이벤트인경우
            {
                await ProcessPositionInit(GetFirstPositionInit(scenario));
                if(scenarioData.IsViewed)
                { // 이미 본 이벤트 일때는 상호작용을 통해 이벤트 진입
                    Debug.Log("말걸기 상호작용 필요");
                    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                }
                Debug.Log("이 이벤트를 열람 성공");
                scenarioData.SetViewed(true);
                await ScenarioTask(scenario);

                //이 시나리오 태스크가 끝난 후 해결상태가 되었다면 시간 흘려보내기
                if(scenarioData.IsAllSolved())
                {
                    Debug.Log("시나리오를 통해 시나리오의 통과조건을 완수했다!");
                    if(eventPlan.IsAllSolved()){
                        Debug.Log("이 시간대의 모든 이벤트가 해결 되었으므로 시간을 흘려보내겠다.");
                        EventTimeService.SetEventTimeToNext(EventTimeService.CurEventTime);
                    }
                    else{
                        Debug.Log("하지만 모든 이벤트가 해결된것은 아닌것같다");

                    }
                }
            }
        }
        else{
            Debug.Log("장소로 이동 중 (이벤트가 없는 곳)");
        }

        _isMoving = false;

        Debug.Log("장소버튼 ON");
        PlaceService.CurPlace.ShowPlaceMoveBtns(.5f);
        uiManager.PlaceUIPanel.SetCurPlaceText(placeID);
        uiManager.PlaceUIPanel.OpenPanel(.5f);
        await UniTask.WaitForSeconds(.5f);

        Debug.Log($"----------------------------------------LOOP END {placeID}----------------------------------------");
    }

    
    //시나리오 Task 구조
    public static async UniTask ScenarioTask(Scenario scenario){

        UIManager.Instance.DialoguePanel.ClearPanel();
        UIManager.Instance.DialoguePanel.OpenPanel(1f);
        await UniTask.WaitForSeconds(1f);
        //대화창 On
        
        //Elements 출력
        List<Element> elements = scenario.Elements;
        await ProcessElementsTask(elements);

        UIManager.Instance.DialoguePanel.ClosePanel(1f);
        await UniTask.WaitForSeconds(1f);

    }

    private static async UniTask ProcessElementsTask(List<Element> elements){

        foreach(Element element in elements){
            await ProcessElementTask(element);
        }
    }
    
    public static async UniTask ProcessElementTask(Element element){

        Debug.Log(element.GetType());
        if(element is PositionInit)
        {
            PositionInit positionInit = element as PositionInit;
            await ProcessPositionInit(positionInit);
        }
        else if(element is Dialogue)
        {
            Dialogue dialogue = element as Dialogue;
            await ProcessDialogue(dialogue);
        }
        else if(element is ChoiceSet)
        {
            ChoiceSet choiceSet = element as ChoiceSet;
            await ProcessChoiceSet(choiceSet);

        }
        else if(element is ItemDemand)
        {
            ItemDemand itemDemand = element as ItemDemand;
            await ProcessItemDemand(itemDemand);
        }
        else if(element is AssetChange)
        {
            AssetChange assetChange = element as AssetChange;
            await ItemService.AssetChangeTask(assetChange);
        }
    }
    
    public static async UniTask ProcessPositionInit(PositionInit positionInit){
        foreach(CharacterPosition characterPosition in positionInit.CharacterPositions){
            Character instancedCharacter = CharacterService.GetInstancedCharacter(characterPosition.CharacterID);
            bool isPositionChangeOnly = instancedCharacter != null;
            if(isPositionChangeOnly){
                instancedCharacter.SetPos(characterPosition.PositionID, 1f);
            }
            else{
               CharacterService.FadeOutCharacterThenDestroy(characterPosition.CharacterID, .3f);
               CharacterService.InstantiateCharacterThenFadeIn(characterPosition.CharacterID, characterPosition.PositionID, "Smile", 1f);
            }

        }
        await UniTask.WaitForSeconds(1f);
    }

    public static async UniTask ProcessChoiceSet(ChoiceSet choiceSet){

        ChoiceSetPanel choiceSetPanel = UIManager.Instance.ChoiceSetPanel;

        foreach(Dialogue dialogue in choiceSet.Dialogues){
            await ProcessDialogue(dialogue);
        }
        Choice selectedChoice = await choiceSetPanel.MakeChoiceBtnsAndWait(choiceSet);
        await ProcessElementsTask(selectedChoice.Elements);
    }

    public static async UniTask ProcessDialogue(Dialogue dialogue)
    {
        DialoguePanel dialoguePanel = UIManager.Instance.DialoguePanel;

        string characterID = dialogue.CharacterID;
        CharacterData characterData = CharacterService.GetCharacterData(characterID);
        Character instancedCharacter = CharacterService.GetInstancedCharacter(characterID);
        if (instancedCharacter != null)
        {
            CameraController.MoveX(instancedCharacter.transform.position.x / 10f, 1f);
        }

        foreach (var line in dialogue.Lines)
        {
            if (instancedCharacter != null)
            {
                instancedCharacter.SetEmotion(line.EmotionID, .3f);
                instancedCharacter.StartTalking();
            }

            dialoguePanel.SetCharacterText(characterData.CharacterNameForUser, characterData.CharacterColor);
            await dialoguePanel.TypeLineTask(line.Sentence, Color.white);

            if (instancedCharacter != null)
            {
                instancedCharacter.StopTalking();
            }

            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        }
    }

    public static async UniTask ProcessItemDemand(ItemDemand itemDemand)
    {
        foreach(Dialogue dialogue in itemDemand.Dialogues){
            await ProcessDialogue(dialogue);
        }

        while(true){
            UIManager.Instance.ItemDemandPanel.OpenPanel(); 

            ItemData selectedItemData = await  UIManager.Instance.ItemDemandPanel.OpenItemDemandPanelAndWait();
            Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");

            UIManager.Instance.ItemDemandPanel.ClosePanel(); 

            if (selectedItemData == null)
            {
                Debug.Log("취소 되었음.");
                await ProcessElementsTask(itemDemand.CancelElements);
                break;
            }
            else if (selectedItemData.ItemID == itemDemand.ItemID)
            {
                Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");
                Debug.Log("정답이므로 elements 처리 후 이 루틴을 빠져나갈 예정");
                await ProcessElementsTask(itemDemand.SuccessElements);
                break;
            }
            else
            {
                Debug.Log("오답이므로 elements 처리 후 이 루틴이 반복될 예정");
                await ProcessElementsTask(itemDemand.FailElements);
            }
        }
    }

    public static PositionInit GetFirstPositionInit(Scenario scenario)
    {
        return scenario.Elements.FirstOrDefault( element => element is PositionInit ) as PositionInit;
    }

    public static PositionInit GetLastPositionInit(Scenario scenario)
    {
        return scenario.Elements.LastOrDefault( element => element is PositionInit ) as PositionInit;
    }
}
