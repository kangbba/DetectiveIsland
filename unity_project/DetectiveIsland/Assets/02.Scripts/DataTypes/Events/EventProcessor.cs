using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Aroka.ArokaUtils;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime = new EventTime("2024-04-01", 9, 0);
    [SerializeField] private TextAsset _scenarioFile;

    public TextAsset ScenarioFile => _scenarioFile;
    public EventTime EventTime => _eventTime;


}

public static class EventProcessor
{
    private static List<EventPlan> _allEventPlans = new List<EventPlan>();

    public static void Load(List<Place> places)
    {
        _allEventPlans.Clear();

        foreach (var place in places)
        {
            foreach (var section in place.PlaceSections){
                _allEventPlans.Add(section.EventPlan);
            }
        }

        _allEventPlans = _allEventPlans.OrderBy(plan => plan.EventTime.Date)
                                       .ThenBy(plan => plan.EventTime.Hour)
                                       .ThenBy(plan => plan.EventTime.Minute)
                                       .ToList();

        foreach (var plan in _allEventPlans)
        {
            Debug.Log($"EventTime: {plan.EventTime.Date} - {plan.EventTime.Hour}:{plan.EventTime.Minute}");
        }
    }
    
    public static async UniTask PlayEvent(EventPlan eventPlanToPlay)
    {
        Debug.Log($"이벤트 시작: {eventPlanToPlay.EventTime.Date} - {eventPlanToPlay.EventTime.Hour}:{eventPlanToPlay.EventTime.Minute}");
        Debug.Log($"시나리오 파일: {eventPlanToPlay.ScenarioFile.name}");

        UIManager.SetPlaceUIState(EPlaceUIPanelState.None, 1f);
        await ProcessScenario(EventService.LoadScenario(eventPlanToPlay.ScenarioFile));
        UIManager.SetPlaceUIState(EPlaceUIPanelState.NavigateMode, 1f);

        Debug.Log("이벤트 종료");

        // 이벤트 처리 후 다음 이벤트 시간을 설정
        EventTime nextEventTime = GetNextEventTime(EventTimeService.CurEventTime);
        if (nextEventTime != null)
        {
            Debug.Log($"다음 이벤트 시간: {nextEventTime.Date} - {nextEventTime.Hour}:{nextEventTime.Minute}");
            EventTimeService.SetCurEventTime(nextEventTime);
        }
        else{
            Debug.Log("다음 이벤트 없음");
        }
    }

    public static async UniTask PlaySectionScenario(Scenario scenario){

        UIManager.SetPlaceUIState(EPlaceUIPanelState.None, 1f);
        UIManager.OpenDialoguePanel(0f);
        List<Element> elements = scenario.Elements;
        await ProcessElementsTask(elements);
        UIManager.CloseDialoguePanel(0f);
        UIManager.SetPlaceUIState(EPlaceUIPanelState.NavigateMode, 1f);
    }

    private static EventTime GetNextEventTime(EventTime currentEventTime)
    {
        var futureEvents = _allEventPlans.Where(plan => EventTimeService.CompareTime(plan.EventTime, currentEventTime) == TimeRelation.Future).ToList();
        return futureEvents.FirstOrDefault()?.EventTime;
    }


    //시나리오 Task 구조
    private static async UniTask ProcessScenario(Scenario scenario){

        UIManager.OpenDialoguePanel(1f);
        await UniTask.WaitForSeconds(1f);
        //대화창 On
        
        //Elements 출력
        List<Element> elements = scenario.Elements;
        await ProcessElementsTask(elements);

        UIManager.CloseDialoguePanel(1f);
        await UniTask.WaitForSeconds(1f);
    }

    private static async UniTask ProcessElementsTask(List<Element> elements){

        foreach(Element element in elements){
            await ProcessElementTask(element);
        }
    }
    
    public static async UniTask ProcessElementTask(Element element)
    {
        Debug.Log(element.GetType());
        switch (element)
        {
            case ModifyPosition modifyPosition:
                await ProcessModifyPosition(modifyPosition);
                break;
            case Dialogue dialogue:
                await ProcessDialogue(dialogue);
                break;
            case ChoiceSet choiceSet:
                await ProcessChoiceSet(choiceSet);
                break;
            case ItemDemand itemDemand:
                await ProcessItemDemand(itemDemand);
                break;
            case GainItem gainItem:
                await GainItemTask(gainItem);
                break;
            case GainFriendship gainFriendship:
                await GainFriendshipTask(gainFriendship);
                break;
            case GainPlace gainPlace:
                await GainPlaceTask(gainPlace);
                break;
            case OverlayPicture overlayPicture:
                await OverlayPictureTask(overlayPicture);
                break;
            case CameraAction cameraAction:
                await CameraActionTask(cameraAction);
                break;
            case AudioAction audioAction:
                await AudioActionTask(audioAction);
                break;
            case OverlaySentence overlaySentence:
                await OverlaySentenceTask(overlaySentence);
                break;
            default:
                Debug.LogWarning($"Unsupported element type: {element.GetType()}");
                break;
        }
    }

    
    public static async UniTask ProcessModifyPosition(ModifyPosition modifyPosition)
    {
        // 등장하는 캐릭터들을 저장하는 HashSet
        HashSet<ECharacterID> charactersToAppear = new HashSet<ECharacterID>();

        // modifyPosition.CharacterPositions 리스트를 순회하며 등장 캐릭터를 HashSet에 추가
        foreach (CharacterPosition characterPosition in modifyPosition.CharacterPositions)
        {
            charactersToAppear.Add(characterPosition.CharacterID);
        }

        // 모든 ECharacterID를 순회하면서
        for (int i = 0; i < typeof(ECharacterID).EnumCount(); i++)
        {
            ECharacterID characterID = (ECharacterID)i;
            Character instancedCharacter = CharacterService.GetInstancedCharacter(characterID);

            // 캐릭터가 등장 리스트에 없으면 제거
            if (!charactersToAppear.Contains(characterID) && instancedCharacter != null)
            {
                CharacterService.FadeOutCharacterThenDestroy(characterID, 1f);
            }
        }

        // modifyPosition.CharacterPositions 리스트를 순회하며 등장 캐릭터를 생성
        foreach (CharacterPosition characterPosition in modifyPosition.CharacterPositions)
        {
            Vector3 targetLocalPos = CharacterService.GetLocalPosByPositionID(characterPosition.PositionID) + Vector3.right * PlaceService.CurPlace.CurPlaceSection.SectionCenterX;
            if(CharacterService.GetInstancedCharacter(characterPosition.CharacterID) == null){
                CharacterService.MakeCharacter(characterPosition.CharacterID, EChacterEmotion.Noraml, targetLocalPos, 1f);
            }
        }

        await UniTask.WaitForSeconds(1f);
    }


    public static async UniTask ProcessChoiceSet(ChoiceSet choiceSet){

        foreach(Dialogue dialogue in choiceSet.Dialogues){
            await ProcessDialogue(dialogue);

            
        }
        Choice selectedChoice = await UIManager.MakeChoiceBtnsAndWait(choiceSet);
        await ProcessElementsTask(selectedChoice.Elements);
    }

    public static async UniTask ProcessDialogue(Dialogue dialogue)
    {
        ECharacterID characterID = dialogue.CharacterID;
        bool isRyan = characterID == ECharacterID.Ryan;
        bool isMono = characterID == ECharacterID.Mono;
        CharacterData characterData = CharacterService.GetCharacterData(characterID);
        string[] delimiters = { @"\.", @"\,", @"\!", @"\?", @"\.\.\." }; // 구분할 문자열 패턴 정의
        foreach (var line in dialogue.Lines)
        {
            Debug.Log("Line 시작");
            UIManager.ClearDialoguePanel();
            UIManager.SetDialogueCharacterText(characterData.CharacterNameForUser, characterData.CharacterColor);
            if(!(isRyan || isMono)){
               CharacterService.SetCharacterEmotion(characterID, line.EmotionID, .3f);
               CharacterService.StartCharacterTalking(characterID);
            }
            
            await UIManager.TypeLineTask(line.Sentence.Trim(), Color.white); // 문장 출력
            CharacterService.StopCharacterTalking(characterID);
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0)); // 마우스 클릭 대기
            Debug.Log("Line 끝");
        }

    }

    public static async UniTask ProcessItemDemand(ItemDemand itemDemand)
    {
        foreach(Dialogue dialogue in itemDemand.Dialogues){
            await ProcessDialogue(dialogue);
        }

        while(true){
            UIManager.OpenItemDemandPanel(); 

            ItemData selectedItemData = await UIManager.OpenItemDemandPanelAndWait();

            UIManager.CloseItemDemandPanel(); 

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
                Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");
                Debug.Log("오답이므로 elements 처리 후 이 루틴이 반복될 예정");
                await ProcessElementsTask(itemDemand.FailElements);
            }
        }
    }

    public static async UniTask GainItemTask(GainItem gainItem)
    {
        ItemData itemData = ItemService.GetItemData(gainItem.ID);
        if (itemData == null)
        {
            Debug.LogError("AssetChangeTask called with null ItemData");
            return;
        }
        if(gainItem.IsGain){
            EventAction eventAction = new EventAction(new CollectItemAction(itemID : gainItem.ID));
            UIManager.OpenItemOwnPanel(itemData);
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
            UIManager.CloseItemOwnPanel();
                
        }
        else {
            EventAction eventAction = new EventAction(new GiveItemAction(itemID : gainItem.ID));
        }
       
    }
    public static async UniTask GainFriendshipTask(GainFriendship gainFriendship)
    {
        await UniTask.WaitForSeconds(1f);
    }
    public static async UniTask GainPlaceTask(GainPlace gainPlace)
    {
        await UniTask.WaitForSeconds(1f);
    }
    public static async UniTask OverlayPictureTask(OverlayPicture overlayPicture)
    {
        PictureService.SetOverlayPictureEffect(overlayPicture);
        await UniTask.WaitForSeconds(1f);
    }
    public static async UniTask CameraActionTask(CameraAction cameraAction)
    { 
        switch (cameraAction.CameraActionID)
        {
            case ECameraActionID.ShakeNormal:
                // ShakeNormal 액션 실행
                CameraController.ShakeCamera(5f, cameraAction.CameraActionTime);
                break;
            case ECameraActionID.ShakeStrong:
                // ShakeStrong 액션 실행
                CameraController.ShakeCamera(10f, cameraAction.CameraActionTime);
                break;
            case ECameraActionID.GoLeftRight:
                // GoLeftRight 액션 실행
                break;
            case ECameraActionID.ZoomIn:
                break;
            case ECameraActionID.ZoomOut:
                break;
            default:
                // None 또는 정의되지 않은 액션 처리
                Debug.LogWarning("Undefined camera action.");
                break;
        }
        if(cameraAction.WaitForFinish){
            await UniTask.WaitForSeconds(cameraAction.CameraActionTime);
        }
        else{
            await UniTask.WaitForSeconds(0f);
        }
    }
    public static async UniTask AudioActionTask(AudioAction audioAction)
    {
        await UniTask.WaitForSeconds(1f);
    }
    public static async UniTask OverlaySentenceTask(OverlaySentence overlaySentence)
    {
        OverlaySentenceDisplayer displayer = UIManager.OverlaySentenceDisplayer;
        await displayer.DisplayOverlaySentence(overlaySentence);
    }
    public static EventPlan GetEventPlan(EventTime eventTime)
    {
        return _allEventPlans.FirstOrDefault(plan => plan.EventTime.Equals(eventTime));
    }

}
