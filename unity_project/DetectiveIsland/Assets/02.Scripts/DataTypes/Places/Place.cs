using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ArokaInspector.Attributes;

public enum ButtonActionType
{
    None,
    MovePlace,
    GetItem,
    PlayEvent
}

[System.Serializable]
public class ButtonActionPair
{
    [SerializeField] private PlacePointButton _button;
    [SerializeField] private ButtonActionType _actionType;

    [ShowIf("_actionType", ButtonActionType.MovePlace)]
    [SerializeField] private EPlaceID _placeID;

    [ShowIf("_actionType", ButtonActionType.GetItem)]
    [SerializeField] private EItemID _itemID;

    [ShowIf("_actionType", ButtonActionType.PlayEvent)]
    [SerializeField] private EventPlan _eventPlan;

    public PlacePointButton Button => _button;

    public void Execute()
    {
        switch (_actionType)
        {
            case ButtonActionType.MovePlace:
                Debug.Log($"Moving to place: {_placeID}");
                EventProcessor.MoveToPlace(_placeID, 0);
                // 여기에 실제 동작 로직 추가
                break;
            case ButtonActionType.GetItem:
                Debug.Log($"Getting item: {_itemID}");
                EventProcessor.ProcessGainItem(new GainItem(true, _itemID, 1)).Forget();
                // 여기에 실제 동작 로직 추가
                break;
            case ButtonActionType.PlayEvent:
                Debug.Log("Playing event...");
                PlayEvent();
                break;
            default:
                Debug.LogWarning("Unsupported action type.");
                break;
        }
    }

    private async void PlayEvent()
    {
        if (_eventPlan == null)
        {
            Debug.LogWarning("EventPlan is null.");
            return;
        }

        Scenario scenario = EventService.LoadScenario(_eventPlan.ScenarioFile);
        if (scenario == null)
        {
            Debug.LogWarning("Scenario is null.");
            return;
        }

        await EventProcessor.PlayEvent(scenario);
    }
}

public class Place : ArokaSpriteEffector
{
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private List<PlaceSection> _placeSections;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private List<ButtonActionPair> _buttonActionPairs;
    [SerializeField] private List<PlacePointButton> _placePoints;

    private int _curSectionIndex;
    private bool _isActive;

    public PlaceSection CurPlaceSection => (_curSectionIndex >= 0 && _curSectionIndex < _placeSections.Count) ? _placeSections[_curSectionIndex] : null;
    public List<PlaceSection> PlaceSections => _placeSections;
    public EPlaceID PlaceID => _placeID;
    public string PlaceNameForUser => _placeNameForUser;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        SpriteRenderer.sortingOrder = -1;
        _placePoints = GetComponentsInChildren<PlacePointButton>().ToList();

        foreach (var placePoint in _placePoints)
        {
            placePoint.Initialize(this);
        }
    }

    public void OnEnter(int initialPlaceSectionIndex, float totalTime)
    {
        Debug.Log($"{PlaceID}에 입장했습니다. Enter() 진행시작");
        _isActive = true;
        SetPlaceSectionAndPlayEvent(initialPlaceSectionIndex, totalTime).Forget();
    }

    public void OnExit()
    {
        UIManager.SetMouseCursorMode(EMouseCursorMode.Normal);
        _isActive = false;
        SetAllButtonInteractable(false);
        Debug.Log($"{PlaceID}에서 퇴장합니다. Exit() 진행완료");
        // 이벤트 처리 후 다음 이벤트 시간을 설정
        EventTime nextEventTime = EventTimeService.GetNextEventTime();
        if (nextEventTime != null)
        {
            Debug.Log($"다음 이벤트 시간: {nextEventTime.Date} - {nextEventTime.Hour}:{nextEventTime.Minute}");
            EventTimeService.SetCurEventTime(nextEventTime);
        }
        else
        {
            Debug.Log("다음 이벤트 없음");
        }
    }

    public void SetNextPlaceSection()
    {
        if (_curSectionIndex < _placeSections.Count - 1)
        {
            SetPlaceSectionAndPlayEvent(_curSectionIndex + 1, .5f).Forget();
        }
        else
        {
            Debug.LogWarning("다음 페이지가 없습니다");
        }
    }

    public void SetPreviousPlaceSection()
    {
        if (_curSectionIndex > 0)
        {
            SetPlaceSectionAndPlayEvent(_curSectionIndex - 1, .5f).Forget();
        }
        else
        {
            Debug.LogWarning("이전 페이지가 없습니다");
        }
    }

    private async UniTask SetPlaceSectionAndPlayEvent(int placeSectionIndex, float totalTime)
    {
        Debug.Log($"{placeSectionIndex}로 설정 시도");
        if (placeSectionIndex < 0 || placeSectionIndex >= _placeSections.Count)
        {
            Debug.LogWarning($"Index {placeSectionIndex}에 해당하는 섹션을 찾을 수 없습니다");
            return;
        }
        _curSectionIndex = placeSectionIndex;
        PlaceSection placeSection = CurPlaceSection;
        EventPlan placeEventPlan = placeSection.EventPlan;
        CameraController.MoveX(placeSection.SectionCenterX, totalTime);
        await UniTask.WaitForSeconds(totalTime);

        if (placeEventPlan != null && EventTimeService.IsCurrentTimeEquals(placeEventPlan.EventTime))
        {
            Debug.Log($"이 섹션에 포함된 시나리오 시간 : {placeEventPlan.EventTime}");
            await CurPlaceSection.PlayEnterEvent();
        }
        Debug.Log("EventTime null or time does not match");
        SetAllButtonInteractable(true);
        UIManager.SetMouseCursorMode(EMouseCursorMode.Detect);
    }

    public void OnPlacePointClicked(PlacePointButton placePointButton)
    {
        var actionPair = _buttonActionPairs.FirstOrDefault(pair => pair.Button == placePointButton);
        if (actionPair != null)
        {
            actionPair.Execute();
        }
    }

    public void SetAllButtonInteractable(bool interactable)
    {
        foreach (var placePoint in _placePoints)
        {
            placePoint.SetButtonInteractable(interactable);
        }
    }
}
