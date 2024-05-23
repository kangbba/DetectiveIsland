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

    public async UniTask PlaySectionEvent(){
        CameraController.MoveX(_sectionCenterX, 1f);

        EventPlan eventPlanToPlay = _eventPlan;
        if (eventPlanToPlay == null)
        {
            Debug.Log("eventPlanToPlay null or time does not match");
            return;
        }
        if(!EventTimeService.IsCurrentTimeEquals(eventPlanToPlay.EventTime)){
            Debug.Log("EventTime null or time does not match");
            return;
        }

        Scenario scenario = EventService.LoadScenario(eventPlanToPlay.ScenarioFile);
        if(scenario == null){
            Debug.Log("해당 eventplan엔 시나리오가 없으므로 생략");
            return;
        }
        await EventProcessor.PlayEvent(scenario);
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

    public async void Initialize(int initialPlaceSectionIndex)
    {
        await SetPlaceSection(initialPlaceSectionIndex);
    }

    public async void SetNextPlaceSection()
    {
        if (_curSectionIndex < _placeSections.Count - 1)
        {
            await SetPlaceSection(_curSectionIndex + 1);
        }
        else
        {
            Debug.LogWarning("다음 페이지가 없습니다");
        }
    }

    public async void SetPreviousPlaceSection()
    {
        if (_curSectionIndex > 0)
        {
            await SetPlaceSection(_curSectionIndex - 1);
        }
        else
        {
            Debug.LogWarning("이전 페이지가 없습니다");
        }
    }

    private async UniTask SetPlaceSection(int placeSectionIndex)
    {
        if (placeSectionIndex < 0 || placeSectionIndex >= _placeSections.Count)
        {
            Debug.LogWarning($"Index {placeSectionIndex}에 해당하는 섹션을 찾을 수 없습니다");
            return;
        }

        _curSectionIndex = placeSectionIndex;

        // Set the section and play the scenario
        if(CurPlaceSection != null){
            Debug.Log($"현재 시간 : {EventTimeService.CurEventTime}");
            Debug.Log($"이 섹션에 포함된 시나리오 시간 : {CurPlaceSection.EventPlan.EventTime}");
            await CurPlaceSection.PlaySectionEvent();
        }

        // Start detecting place points
        StartDetectingPlacePoints(true);
    }

    private void StartDetectingPlacePoints(bool isDetecting)
    {
        List<PlacePoint> _placePoints = _placePointsParent.GetComponentsInChildren<PlacePoint>().ToList();
        foreach (var placePoint in _placePoints)
        {
            placePoint.StartDetecting(isDetecting);
        }
    }
}
