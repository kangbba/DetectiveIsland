using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using Aroka.JsonUtils;
using System.Text.RegularExpressions;
using Aroka.ArokaUtils;

public static class EventProcessor
{
    private static ScenarioData _curScenarioData;
    private static bool _isMoving = false;


    public static async void MoveToPlace(EPlaceID placeID)
    {
        if (_isMoving)
        {
            Debug.LogWarning("이동중 또다른 Move 호출");
            return;
        }
        _isMoving = true;
        await MoveToPlaceUniTask(placeID);
    }

    private static async UniTask MoveToPlaceUniTask(EPlaceID placeID)
    {
        EventPlan eventPlan = EventService.GetEventPlan(EventTimeService.CurEventTime);
        ScenarioData scenarioData = eventPlan.GetScenarioData(placeID);
        bool isEventExist = scenarioData != null;
        Debug.Log($"----------------------------------------LOOP START----------------------------------------");
        Debug.Log($"현재 시간 : {EventTimeService.CurEventTime}");
        Debug.Log($"장소 섹션 이동 : {placeID}");

        PlaceService.MakePlace(placeID, 1f);
        UIManager.SetPlaceUIState(EPlaceUIPanelState.None, .5f);

        CharacterService.AllCharacterFadeOutAndDestroy(.5f);
        await UniTask.WaitForSeconds(1f);

        // AwaitChoices의 결과를 직접 받아 처리
        if (isEventExist)
        {
            Debug.Log("장소 섹션으로 이동 중 (이벤트가 있는 곳)");
            Scenario scenario = ArokaJsonUtils.LoadScenario(scenarioData.ScenarioFile);
            _curScenarioData = scenarioData;
            if (!scenarioData.IsSolvedAndExited)
            {
                Debug.Log("미 해결 이벤트인 경우");
                if (scenarioData.IsEntered || scenarioData.IsExited)
                { // 이미 본 이벤트 일 때는 상호작용을 통해 이벤트 진입
                    Debug.Log("말걸기 상호작용 필요");
                    await ProcessModifyPosition(_curScenarioData.RecentModifyPosition);
                    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                }
                Debug.Log("이 이벤트를 IsEntered 성공");
                scenarioData.IsEntered = true;
                await ScenarioTask(scenario);
                Debug.Log("이 이벤트를 IsExited 성공");
                scenarioData.IsExited = true;

                // 이 시나리오 태스크가 끝난 후 해결 상태가 되었다면 시간 흘려보내기
                if (scenarioData.IsSolved)
                {
                    Debug.Log("시나리오를 통해 시나리오의 통과 조건을 완수했다!");
                    CharacterService.AllCharacterFadeOutAndDestroy(1f);
                    await UniTask.WaitForSeconds(1f);
                    if (eventPlan.IsAllSolved())
                    {
                        Debug.Log("이 시간대의 모든 이벤트가 해결되었으므로 시간을 흘려보내겠다.");
                        EventTimeService.SetEventTimeToNext(EventTimeService.CurEventTime);
                    }
                    else
                    {
                        Debug.Log("하지만 모든 이벤트가 해결된 것은 아닌 것 같다");
                    }
                }
            }
            else
            {
                Debug.Log("이미 해결되고 시나리오를 다 본 장소임");
            }
        }
        else
        {
            Debug.Log("장소 섹션으로 이동 중 (이벤트가 없는 곳)");
        }

        Debug.Log("장소 버튼 ON");
        UIManager.SetPlaceUIState(EPlaceUIPanelState.NavigatelMode, .5f);
        await UniTask.WaitForSeconds(.5f);

        _isMoving = false;
        Debug.Log($"----------------------------------------LOOP END {placeID}----------------------------------------");
    }


    
    //시나리오 Task 구조
    public static async UniTask ScenarioTask(Scenario scenario){

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
                _curScenarioData.RecentModifyPosition = modifyPosition;
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
               
                EventAction eventAction = new EventAction(new GiveItemAction(itemID : selectedItemData.ItemID));
                _curScenarioData.ExecuteActionThenAdd(eventAction);

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
            _curScenarioData.ExecuteActionThenAdd(eventAction);
            UIManager.OpenItemOwnPanel(itemData);
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
            UIManager.CloseItemOwnPanel();
                
        }
        else {
            EventAction eventAction = new EventAction(new GiveItemAction(itemID : gainItem.ID));
            _curScenarioData.ExecuteActionThenAdd(eventAction);
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

}
