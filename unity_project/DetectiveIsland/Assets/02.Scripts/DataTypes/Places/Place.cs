using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using Cysharp.Threading.Tasks;


[System.Serializable]
public class PlaceSection
{
    [SerializeField] private float _sectionCenterX;
    [SerializeField] private EventPlan _eventPlan;

    public float SectionCenterX { get => _sectionCenterX; }
    public EventPlan EventPlan { get => _eventPlan;  }

    public async UniTask PlaySectionEventIfValid(){
        EventPlan eventPlanToPlay = _eventPlan;
        if (eventPlanToPlay == null)
        {
            Debug.Log("eventPlanToPlay null or time does not match");
            return;
        }
        Scenario scenario = EventService.LoadScenario(eventPlanToPlay.ScenarioFile);
        if(scenario == null){
            Debug.Log("해당 eventplan엔 시나리오가 없으므로 생략");
            return;
        }
        await EventProcessor.PlayEvent(scenario, true);
    }
}
public class Place : ArokaSpriteEffector
{
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private List<PlaceSection> _placeSections = new List<PlaceSection>();
    [SerializeField] private Transform _placePointsParent;

    private int _curSectionIndex;

    public EPlaceID PlaceID => _placeID;
    public string PlaceNameForUser => _placeNameForUser;

    public PlaceSection CurPlaceSection => (_curSectionIndex >= 0 && _curSectionIndex < _placeSections.Count) ? _placeSections[_curSectionIndex] : null;

    public List<PlaceSection> PlaceSections { get => _placeSections; }

    private void Start()
    {
        SpriteRenderer.sortingOrder = -1;
    }

    public void OnEnter(int initialPlaceSectionIndex, float totalTime)
    {
        Debug.Log($"{_placeID}에 입장했습니다. Enter() 진행중");
        Debug.Log($"{initialPlaceSectionIndex}에 입장했습니다. Enter() 진행중");
        SetPlaceSectionAndPlayEvent(initialPlaceSectionIndex, totalTime).Forget();
    }
    public void OnExit()
    {
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

        if(EventTimeService.IsCurrentTimeEquals(placeEventPlan.EventTime)){
            Debug.Log($"이 섹션에 포함된 시나리오 시간 : {placeEventPlan.EventTime}");
            await CurPlaceSection.PlaySectionEventIfValid();
        }
        Debug.Log("EventTime null or time does not match");

        SetAllButtonInteractable(true);
    }

    public void SetAllButtonInteractable(bool interactable)
    {
        List<PlacePoint> _placePoints = _placePointsParent.GetComponentsInChildren<PlacePoint>().ToList();
        foreach (var placePoint in _placePoints)
        {
            placePoint.SetButtonInteractable(interactable);
        }
    }
}
