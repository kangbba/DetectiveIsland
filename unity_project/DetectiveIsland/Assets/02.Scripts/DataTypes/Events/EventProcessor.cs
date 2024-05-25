using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Aroka.ArokaUtils;


public static class EventProcessor
{

    public static void Load(){

    }
    public static void MoveToPlace(EPlaceID placeID, int sectionIndex){
        if(PlaceService.CurPlace != null){
           PlaceService.CurPlace.OnExit();
           PlaceService.CurPlace.FadeOutAndDestroy(1f);
        }
        Place place = PlaceService.MakePlaceAndRegister(placeID, 1f);
        place.OnEnter(sectionIndex, 1f);
    }
    
    public static async UniTask PlayEvent(Scenario scenario)
    {
        Debug.Log("PlayEvent!");
        UIManager.SetPlaceUIState(EPlaceUIPanelState.None, 1f);
        UIManager.OpenDialoguePanel(1f);
        await UniTask.WaitForSeconds(1f);

        //Elements 출력
        List<Element> elements = scenario.Elements;
        await ProcessElementsTask(elements);

        UIManager.CloseDialoguePanel(1f);

        UIManager.SetPlaceUIState(EPlaceUIPanelState.NavigateMode, 1f);
        await UniTask.WaitForSeconds(1f);

        
    }


    private static async UniTask ProcessElementsTask(List<Element> elements){

        foreach(Element element in elements){
            Debug.Log($"{element.GetType()}");
            await ProcessElementTask(element);
        }
    }
    
    public static async UniTask ProcessElementTask(Element element)
    {
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
                await ProcessGainItem(gainItem);
                break;
            case GainFriendship gainFriendship:
                await ProcessGainFriendship(gainFriendship);
                break;
            case GainPlace gainPlace:
                await ProcessGainPlace(gainPlace);
                break;
            case OverlayPicture overlayPicture:
                await ProcessOverlayPicture(overlayPicture);
                break;
            case CameraAction cameraAction:
                await ProcessCameraAction(cameraAction);
                break;
            case AudioAction audioAction:
                await ProcessAudioAction(audioAction);
                break;
            case OverlaySentence overlaySentence:
                await ProcessOverlaySentence(overlaySentence);
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
                CharacterService.MakeCharacter(characterPosition.CharacterID, EEmotionID.Noraml, targetLocalPos, 1f);
            }
        }

        await UniTask.WaitForSeconds(1f);
    }

    public static async UniTask ProcessDialogue(Dialogue dialogue)
    {
        await UIManager.TypeDialogueTask(dialogue);
    }

    public static async UniTask ProcessChoiceSet(ChoiceSet choiceSet){

        foreach(Dialogue dialogue in choiceSet.Dialogues){
             await UIManager.TypeDialogueTask(dialogue);
        }
        Choice selectedChoice = await UIManager.MakeChoiceBtnsAndWait(choiceSet);
        await ProcessElementsTask(selectedChoice.Elements);
    }

    public static async UniTask ProcessItemDemand(ItemDemand itemDemand)
    {
        foreach(Dialogue dialogue in itemDemand.Dialogues){
            await UIManager.TypeDialogueTask(dialogue);
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

    public static async UniTask ProcessGainItem(GainItem gainItem)
    {
        ItemData itemData = ItemService.GetItemData(gainItem.ID);
        if (itemData == null)
        {
            Debug.LogError("AssetChangeTask called with null ItemData");
            return;
        }
        if(gainItem.IsGain){
            OwnershipService.SetHasItem(gainItem.ID, true);
            UIManager.OpenItemOwnPanel(itemData);
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
            UIManager.CloseItemOwnPanel();
                
        }
        else {
            OwnershipService.SetHasItem(gainItem.ID, false);
        }
       
    }
    public static async UniTask ProcessGainFriendship(GainFriendship gainFriendship)
    {
        await UniTask.WaitForSeconds(1f);
    }
    public static async UniTask ProcessGainPlace(GainPlace gainPlace)
    {
        OwnershipService.SetPlaceOwnership(gainPlace.ID, true);
        await UniTask.WaitForSeconds(1f);
    }
    public static async UniTask ProcessOverlayPicture(OverlayPicture overlayPicture)
    {
        PictureService.SetOverlayPictureEffect(overlayPicture);
        await UniTask.WaitForSeconds(1f);
    }
    public static async UniTask ProcessCameraAction(CameraAction cameraAction)
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
    public static async UniTask ProcessAudioAction(AudioAction audioAction)
    {
        await UniTask.WaitForSeconds(1f);
    }
    public static async UniTask ProcessOverlaySentence(OverlaySentence overlaySentence)
    {
        await UIManager.DisplayOverlaySentence(overlaySentence);
    }


}
