using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using Aroka.JsonUtils;
using UnityEngine;

public static class EventProcessor
{
    public static IEnumerator ScenarioRoutine(Scenario scenario){

        Debug.Log("이벤트가 발동됨");
        //대화창 On
        DialogueService.ClearPanel();
        DialogueService.SetOnPanel(true, 1f);
        yield return new WaitForSeconds(1f);
        
        Debug.Log("이벤트 실행이 돌입");
        List<Element> elements = scenario.Elements;
        yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(elements));

        //이벤트 완료!
        var remainedEvents = EventService.GetEventPlansByDate(EventService.CurEventTime.Date).EventTimeFilter(EventService.CurEventTime, TimeRelation.Future);
        // 처리된 이벤트별 상세 정보 출력
        foreach (var plan in remainedEvents)
        {
            Debug.Log($"오늘 남은 이벤트 ID: {plan.PlaceID}, 시간: {plan.EventTime}");
        }

        EventService.SetCurEventTime(remainedEvents[0].EventTime);

        //대화창 Off
        DialogueService.SetOnPanel(false, 1f);
        yield return new WaitForSeconds(1f);

    }

    // EventCondition에 따라 적절한 이벤트를 처리하는 메소드
    public static IEnumerator EventConditionRoutine(EventCondition eventCondition)
    {
        if(eventCondition == null || eventCondition.ActionType == EActionType.AutoPlay){
            Debug.LogError("이 이벤트는 특정 조건이 필요하지않으므로 기다리지 않겠습니다");
            yield break;
        }
        switch (eventCondition.ActionType)
        {
            case EActionType.Talk:
                yield return WaitForTalk(eventCondition.TargetID);
                break;
            default:
                Debug.LogError("Unsupported action type!");
                break;
        }
    }
    
    public static IEnumerator WaitForTalk(string targetID)
    {
        bool isDone = false;

        // 이벤트 핸들러 정의
        void OnTalkHandler(string id)
        {
            if (id == targetID)
            {
                isDone = true;
            }
        }

        // 이벤트 구독
        CharacterService.OnCharacterTalk += OnTalkHandler;

        // 대화가 시작될 때까지 대기
        while (!isDone)
        {
            Debug.Log($"{targetID} 에게 말 걸기를 대기중");
            yield return null;
        }

        Debug.Log("말 걸기 감지됨 ");
        // 이벤트 해제
        CharacterService.OnCharacterTalk -= OnTalkHandler;
    }

    private static IEnumerator ProcessElementsRoutine(List<Element> elements){

        foreach(Element element in elements){
            yield return ProcessElementRoutine(element);
        }
    }
    public static IEnumerator InitializeScenarioRoutine(Scenario scenario){
        //임시로 첫 장면을 따오자.
        List<Element> elements = scenario.Elements;
        yield return ProcessElementRoutine(elements[0]);
    }

    public static IEnumerator ProcessElementRoutine(Element element){

        if(element is PositionChange){
            PositionChange positionChange = element as PositionChange;
            CharacterService.PositionChange(positionChange.CharacterID, positionChange.PositionID, 1f);
            yield return new WaitForSeconds(1f);
        }
        else if(element is Dialogue){
            Dialogue dialogue = element as Dialogue;
            yield return CoroutineUtils.StartCoroutine(DialogueService.DialogueRoutine(dialogue));
        }
        else if(element is ChoiceSet){

            ChoiceSet choiceSet = element as ChoiceSet;
            Choice selectedChoice = null;
            yield return CoroutineUtils.AwaitCoroutine<Choice>(ChoiceSetService.ChoiceSetRoutine(choiceSet), result => {
                selectedChoice = result;
            });
            Debug.Log($"{selectedChoice.Title}을 골랐다!");
            yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(selectedChoice.Elements));

        }
        else if(element is ItemDemand){

            ItemDemand itemDemand = element as ItemDemand;
            while(true){
                bool isCorrect = false;
                yield return CoroutineUtils.AwaitCoroutine<bool>(ItemService.ItemDemandRoutine(itemDemand), result => {
                    isCorrect = result;
                });
                if(isCorrect){
                    Debug.Log("정답이므로 elements 처리후 이 루프를 빠져나갈 예정");
                    yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(itemDemand.SuccessElements));
                    break;
                }
                else{
                    Debug.Log("오답이므로 elements 처리후 이 루프가 반복될 예정");
                    yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(itemDemand.FailElements));
                }
            }
        }
        else if(element is AssetChange){
            AssetChange assetChange = element as AssetChange;
            yield return CoroutineUtils.StartCoroutine(ItemService.AssetChangeRoutine(assetChange));
        }
        yield return null;
    }
    
}
