using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;


public enum EPlaceID
{
    None = 0,
    HospitalBedroom = 1,
    HospitalDoor = 2,
    Town1 = 3,
    Town2 = 4,
    Town3 = 5,
    Town4 = 6,
    Town5 = 7,
    Town6 = 8,
    CafeSeabreeze = 9,
    // 다른 장소들...
}

public static class PlaceService
{
    enum EventCase {
        PresetOverlayScenario,
        NormalScenario,
        WithoutScenario,
        NoEvent
    }
    private static Transform _placePanel;
    private static List<Place> _placePrefabs = new List<Place>();

    private static Place _curPlace;
    public static Place CurPlace { get => _curPlace; }
    public static List<Place> PlacePrefabs { get => _placePrefabs; }

    private static bool _isMoving;

    public static void Load()
    {
        _placePanel = new GameObject("Place Panel").transform;
        _placePrefabs = ArokaUtils.LoadResourcesFromFolder<Place>("PlacePrefabs");
    }


    private static EventCase GetEventCase(EventPlan plan, Scenario scenario) {
        if (plan == null) return EventCase.NoEvent;
        if (scenario != null && scenario.Elements != null) {
            if (scenario.Elements.Count > 0 && scenario.Elements[0] is OverlayPicture overlayPicture && overlayPicture.IsPreset) {
                return EventCase.PresetOverlayScenario;
            }
            return EventCase.NormalScenario;
        }
        return EventCase.WithoutScenario;
    }

    public static void MoveToPlace(EPlaceID placeID, int sectionIndex)
    {
        MoveToPlaceTask(placeID, sectionIndex).Forget();
    }
    public static async UniTask MoveToPlaceTask(EPlaceID placeID, int sectionIndex)
    {
        _isMoving = true;


        //기존 Place 퇴장
        if (CurPlace != null)
        {
            const float exitTotalTime = 1f;
            Place placeToDestroy = CurPlace;
            _curPlace = null;
            placeToDestroy.DestroyPlacePointBtns();
            placeToDestroy.BlackOutAndDestroy(exitTotalTime);
            CharacterService.AllCharacterFadeOutAndDestroy(exitTotalTime);
            await UniTask.WaitForSeconds(exitTotalTime);
        }   


        //화면은 어두운 상태
        CameraController.MoveX(0, 0f);



        //새로운 Place 생성한다.
        const float enterTotalTime = 1f;
        Place place = MakePlaceAndRegister(placeID);
        place.Initialize(sectionIndex, UIManager.UIParent.PlaceUIPanel);
        _curPlace = place;
        place.SetPlaceMode(EPlaceUIPanelState.None, 0f);

        // Scenario 파일을 로드하고 이벤트를 실행하는 로직
        PlaceSection placeSection = place.CurPlaceSection;
        EventPlan properEventPlan = EventPlanManager.GetCurEventPlanOfPlace(placeID, sectionIndex);

        Scenario scenario = properEventPlan != null ? EventService.LoadScenario(properEventPlan.ScenarioFile) : null;
        EventCase eventCase = GetEventCase(properEventPlan, scenario);
        Debug.Log("이 이벤트 종류 : " + eventCase.ToString());
        switch (eventCase) {
            case EventCase.PresetOverlayScenario:
                place.FadeInFromStart(0);
                await EventProcessor.ProcessElementTask(scenario.Elements[0]);
                scenario.Elements.RemoveAt(0);
                Debug.Log("첫 OverlayPicture가 Preset 이므로, UI를 띄우기전 Overlay Picture 하겠음.");
                break;
            case EventCase.NormalScenario:
            case EventCase.WithoutScenario:
            case EventCase.NoEvent:
                break;
        }
        CameraController.MoveX(placeSection.SectionCenterX, enterTotalTime);
        place.FadeInFromStart(enterTotalTime);
        await UniTask.WaitForSeconds(enterTotalTime);

        if(properEventPlan != null){
            await EventProcessor.PlayScenarioWithDialougePanel(scenario);
            properEventPlan.SetCleared(true);
            if (EventPlanManager.IsAllSolvedInEventTime(EventPlanManager.CurEventTime))
            {
                EventPlanManager.TimePassesToNext();
            }
        }
        //시나리오가 끝나고, Navigate모드 인지, 자동 이동일지를 결정한다. 
        _isMoving = false;
        if (properEventPlan != null && properEventPlan.PlaceIDToAutoMoveAfter != EPlaceID.None)
        {
            UIManager.SetMouseCursorMode(EMouseCursorMode.Normal);
            place.SetPlaceMode(EPlaceUIPanelState.None, 1f);
            MoveToPlace(properEventPlan.PlaceIDToAutoMoveAfter, 0);
        }
        else{
            UIManager.SetMouseCursorMode(EMouseCursorMode.Detect);
            place.SetPlaceMode(EPlaceUIPanelState.NavigateMode, 1f);
            place.MakeBtnsWithPlacePoints();
        }


    }

    private static Place MakePlaceAndRegister(EPlaceID placeID)
    {
        Place placePrefab = GetPlacePrefab(placeID);
        if (placePrefab == null)
        {
            Debug.LogWarning($"{placeID}에 해당하는 Place Prefab을 찾을 수 없음");
            return null;
        }
        Place instancedPlace = GameObject.Instantiate(placePrefab, _placePanel.transform);
        instancedPlace.transform.localPosition = Vector3.zero;
        return instancedPlace;
    }

    private static Place GetPlacePrefab(EPlaceID placeID)
    {
        Place place = _placePrefabs.FirstOrDefault(placePrefab => placePrefab.PlaceID == placeID);
        if (place == null)
        {
            Debug.LogWarning($"{placeID}에 해당하는 Place를 찾을 수 없음");
        }
        return place;
    }

    public static PlaceSection GetPlaceSection(EPlaceID placeID, int placeSectionIndex)
    {
        Place place = _placePrefabs.FirstOrDefault(p => p.PlaceID == placeID);
        if (place != null)
        {
            if (placeSectionIndex >= 0 && placeSectionIndex < place.PlaceSections.Count)
            {
                return place.PlaceSections[placeSectionIndex];
            }
            else
            {
                Debug.LogWarning($"Invalid place section index: {placeSectionIndex} for placeID: {placeID}");
            }
        }
        else
        {
            Debug.LogWarning($"Place with ID {placeID} not found");
        }
        return null;
    }

}
