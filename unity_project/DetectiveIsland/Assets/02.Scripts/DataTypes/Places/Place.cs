using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Place : ArokaSpriteEffector
{
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private EventTime _eventTime;

    private List<PlaceSection> _placeSections = new List<PlaceSection>();
    private List<PlacePointButton> _placePoints;
    private int _curSectionIndex = -1;
    private bool _isActive;

    public PlaceSection CurPlaceSection => (_curSectionIndex >= 0 && _curSectionIndex < _placeSections.Count) ? _placeSections[_curSectionIndex] : null;
    public List<PlaceSection> PlaceSections => _placeSections;
    public EPlaceID PlaceID => _placeID;
    public string PlaceNameForUser => _placeNameForUser;
    public EventTime EventTime => _eventTime;

    private void Initialize()
    {
        SpriteRenderer.sortingOrder = -1;

        _placePoints = GetComponentsInChildren<PlacePointButton>().ToList();
        _placeSections = GetComponentsInChildren<PlaceSection>()
                            .OrderBy(section => section.SectionCenterX)
                            .ToList();

        foreach (var placePoint in _placePoints)
        {
            placePoint.Initialize(this);
        }
    }

    public void OnEnter(int initialPlaceSectionIndex, float totalTime)
    {
        Initialize();
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
        TextAsset scenarioFile = placeSection.ScenarioFile;
        CameraController.MoveX(placeSection.SectionCenterX, totalTime);
        await UniTask.WaitForSeconds(totalTime);

        if (scenarioFile != null && EventTimeService.IsCurrentTimeEquals(_eventTime))
        {
            Debug.Log($"Time match {_eventTime}");
            await CurPlaceSection.PlayEnterEvent();
        }
        else
        {
            Debug.Log($"Time does not match {_eventTime}  // curTime : {EventTimeService.CurEventTime}");
        }
        SetAllButtonInteractable(true);
        UIManager.SetMouseCursorMode(EMouseCursorMode.Detect);
    }

    public void OnPlacePointClicked(PlacePointButton placePointButton)
    {
        placePointButton.Execute();
    }

    public void SetAllButtonInteractable(bool interactable)
    {
        foreach (var placePoint in _placePoints)
        {
            placePoint.SetButtonInteractable(interactable);
        }
    }
}
