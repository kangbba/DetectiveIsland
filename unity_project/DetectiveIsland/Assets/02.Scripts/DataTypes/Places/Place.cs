using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class ButtonEventPlanPair
{
    [SerializeField] private PlacePointButton _button;
    [SerializeField] private EventPlan _eventPlan;

    public PlacePointButton Button => _button;
    public EventPlan EventPlan => _eventPlan;
}

public class Place : ArokaSpriteEffector
{
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private List<PlaceSection> _placeSections;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private List<ButtonEventPlanPair> _buttonEventPlans;
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
        Debug.Log($"{PlaceID}에 입장했습니다. Enter() 진행중");
        _isActive = true;
        SetPlaceSectionAndPlayEvent(initialPlaceSectionIndex, totalTime).Forget();
    }

    public void OnExit()
    {
        _isActive = false;
        SetAllButtonInteractable(false);
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
    }

    public void SetAllButtonInteractable(bool interactable)
    {
        foreach (var placePoint in _placePoints)
        {
            placePoint.SetButtonInteractable(interactable);
        }
    }

    public void OnPlacePointClicked(PlacePointButton placePoint)
    {
        Debug.Log($"PlacePoint clicked: {placePoint.name}");
        
        var eventPlanPair = _buttonEventPlans.FirstOrDefault(pair => pair.Button == placePoint);
        if (eventPlanPair != null)
        {
            EventProcessor.PlayEvent(EventService.LoadScenario(eventPlanPair.EventPlan.ScenarioFile), false).Forget();
        }
    }
}
