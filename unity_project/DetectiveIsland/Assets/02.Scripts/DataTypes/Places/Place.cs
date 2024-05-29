using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

public class Place : ArokaSpriteEffector
{
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private string _placeNameForUser;
    private List<PlaceSection> _placeSections = new List<PlaceSection>();
    private List<PlacePointButton> _placePoints = new List<PlacePointButton>();
    private int _curSectionIndex = -1;
    private bool _isActive;

    public EPlaceID PlaceID => _placeID;
    public string PlaceNameForUser => _placeNameForUser;
    public List<PlaceSection> PlaceSections => _placeSections;

    public PlaceSection CurPlaceSection 
    {
        get => (_curSectionIndex >= 0 && _curSectionIndex < _placeSections.Count) ? _placeSections[_curSectionIndex] : null;
        set
        {
            if (_curSectionIndex != value.SectionIndex)
            {
                _curSectionIndex = value.SectionIndex;
                OnCurPlaceSectionChanged?.Invoke(value);
            }
        }
    }

    public delegate void CurPlaceSectionChangedHandler(PlaceSection newSection);
    public event CurPlaceSectionChangedHandler OnCurPlaceSectionChanged;

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

    public void OnEnter(int initialPlaceSectionIndex)
    {
        Debug.Log($"{EventTimeService.CurEventTime} 시간에 {PlaceID}에 입장했습니다. Enter() 진행시작");
        SetAllButtonInteractable(false);
        Initialize();
        _isActive = true;
        SetPlaceSection(initialPlaceSectionIndex, 1f);
    }

    private void SetPlaceSection(int placeSectionIndex, float totalTime)
    {
        Debug.Log($"{placeSectionIndex}로 설정 시도");
        if (placeSectionIndex < 0 || placeSectionIndex >= _placeSections.Count)
        {
            Debug.LogWarning($"Index {placeSectionIndex}에 해당하는 섹션을 찾을 수 없습니다");
            return;
        }
        _curSectionIndex = placeSectionIndex;
        PlaceSection placeSection = CurPlaceSection;
        CameraController.MoveX(placeSection.SectionCenterX, totalTime);
        EventProcessor.CheckAndPlayEvent(this, totalTime).Forget();
    }


    public void OnExit()
    {
        SetAllButtonInteractable(false);
        _isActive = false;
        Debug.Log($"{PlaceID}에서 퇴장합니다. Exit() 진행완료");
    }

    public void SetNextPlaceSection()
    {
        if (_curSectionIndex < _placeSections.Count - 1)
        {
            SetPlaceSection(_curSectionIndex + 1, 1f);
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
            SetPlaceSection(_curSectionIndex - 1, 1f);
        }
        else
        {
            Debug.LogWarning("이전 페이지가 없습니다");
        }
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
