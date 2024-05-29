using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using Aroka.ArokaUtils;

public static class EventProcessor
{

    public static async UniTaskVoid CheckAndPlayEvent(Place place, float delayTime)
    {
        await UniTask.WaitForSeconds(delayTime);
        EventPlan eventPlan = EventTimeService.GetEventPlan(EventTimeService.CurEventTime, place.PlaceID, place.CurPlaceSection.SectionIndex);
        if (eventPlan != null && eventPlan.ScenarioFile != null)
        {
            Debug.Log("작동?");
            await PlayScenarioFile(eventPlan.ScenarioFile);
        }
        Debug.Log("버튼 활성화!");
        place.SetAllButtonInteractable(true);
    }

    public static async UniTask PlayScenarioFile(TextAsset scenarioFile)
    {
        // Scenario 파일을 로드하고 이벤트를 실행하는 로직
        Scenario scenario = EventService.LoadScenario(scenarioFile);
        if (scenario == null)
        {
            Debug.Log("Scenario 로드 실패");
            return;
        }

        Debug.Log("PlayScenarioFile!");
        UIManager.SetPlaceUIState(EPlaceUIPanelState.None, 1f);
        UIManager.OpenDialoguePanel(1f);
        await UniTask.WaitForSeconds(1f);

        List<Element> elements = scenario.Elements;
        await ProcessElementsTask(elements);

        UIManager.CloseDialoguePanel(1f);
        UIManager.SetPlaceUIState(EPlaceUIPanelState.NavigateMode, 1f);
        EventTimeService.SetCurEventTime(EventTimeService.GetNextEventTime());
        
        await UniTask.WaitForSeconds(1f);
    }

    private static async UniTask ProcessElementsTask(List<Element> elements)
    {
        foreach (Element element in elements)
        {
           // Debug.Log($"{element.GetType()}");
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

    // Implement the individual Process methods for each element type here.
    public static async UniTask ProcessModifyPosition(ModifyPosition modifyPosition)
    {
        HashSet<ECharacterID> charactersToAppear = new HashSet<ECharacterID>();

        foreach (CharacterPosition characterPosition in modifyPosition.CharacterPositions)
        {
            charactersToAppear.Add(characterPosition.CharacterID);
        }

        for (int i = 0; i < typeof(ECharacterID).EnumCount(); i++)
        {
            ECharacterID characterID = (ECharacterID)i;
            Character instancedCharacter = CharacterService.GetInstancedCharacter(characterID);

            if (!charactersToAppear.Contains(characterID) && instancedCharacter != null)
            {
                CharacterService.FadeOutCharacterThenDestroy(characterID, 1f);
            }
        }

        foreach (CharacterPosition characterPosition in modifyPosition.CharacterPositions)
        {
            Vector3 targetLocalPos = CharacterService.GetLocalPosByPositionID(characterPosition.PositionID) + Vector3.right * PlaceService.CurPlace.CurPlaceSection.SectionCenterX;
            if (CharacterService.GetInstancedCharacter(characterPosition.CharacterID) == null)
            {
                CharacterService.MakeCharacter(characterPosition.CharacterID, EEmotionID.Noraml, targetLocalPos, 1f);
            }
        }

        await UniTask.WaitForSeconds(1f);
    }

    public static async UniTask ProcessDialogue(Dialogue dialogue)
    {
        await UIManager.TypeDialogueTask(dialogue);
    }

    public static async UniTask ProcessChoiceSet(ChoiceSet choiceSet)
    {
        foreach (Dialogue dialogue in choiceSet.Dialogues)
        {
            await UIManager.TypeDialogueTask(dialogue);
        }
        Choice selectedChoice = await UIManager.MakeChoiceBtnsAndWait(choiceSet);
        await ProcessElementsTask(selectedChoice.Elements);
    }

    public static async UniTask ProcessItemDemand(ItemDemand itemDemand)
    {
        foreach (Dialogue dialogue in itemDemand.Dialogues)
        {
            await UIManager.TypeDialogueTask(dialogue);
        }

        while (true)
        {
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
        if (gainItem.IsGain)
        {
            OwnershipService.SetHasItem(gainItem.ID, true);
            UIManager.OpenItemOwnPanel(itemData);
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
            UIManager.CloseItemOwnPanel();
        }
        else
        {
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
                CameraController.ShakeCamera(5f, cameraAction.CameraActionTime);
                break;
            case ECameraActionID.ShakeStrong:
                CameraController.ShakeCamera(10f, cameraAction.CameraActionTime);
                break;
            case ECameraActionID.GoLeftRight:
                break;
            case ECameraActionID.ZoomIn:
                break;
            case ECameraActionID.ZoomOut:
                break;
            default:
                Debug.LogWarning("Undefined camera action.");
                break;
        }
        if (cameraAction.WaitForFinish)
        {
            await UniTask.WaitForSeconds(cameraAction.CameraActionTime);
        }
        else
        {
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
