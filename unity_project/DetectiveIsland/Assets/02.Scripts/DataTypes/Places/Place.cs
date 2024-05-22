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
    [SerializeField] private Transform _placePointsParent; 

    public float SectionCenterX => _sectionCenterX;

    public EventPlan EventPlan { get => _eventPlan; }

    public void StartDetectingPlacePoints(bool isDetecting)
    {
        List<PlacePoint> placePoints = _placePointsParent.GetComponentsInChildren<PlacePoint>().ToList();
        foreach (PlacePoint placePoint in placePoints)
        {
            placePoint.StartDetecting(isDetecting);
        }
    }
}

public class Place : SpriteEffector
{
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private List<PlaceSection> _sections = new List<PlaceSection>();

    private PlaceSection CurSection => _sections[_curSectionIndex];

    private int _curSectionIndex;

    public bool IsPreviousSectionExist => _curSectionIndex > 0;
    public bool IsNextSectionExist => _curSectionIndex < _sections.Count - 1;

    public EPlaceID PlaceID => _placeID;
    public string PlaceNameForUser => _placeNameForUser;

    public List<PlaceSection> PlaceSections => _sections;
    public PlaceSection CurPlaceSection => _sections[_curSectionIndex];


    private void Start()
    {
        SpriteRenderer.sortingOrder = -1;
    }

    public void Initialize(int initialPlaceSectionIndex)
    {
        SetPlaceSection(initialPlaceSectionIndex);
    }

    public void SetNextPlaceSection()
    {
        if (!IsNextSectionExist)
        {
            Debug.LogWarning("다음 페이지가 없습니다");
            return;
        }
        SetPlaceSection(_curSectionIndex + 1);
    }

    public void SetPreviousPlaceSection()
    {
        if (!IsPreviousSectionExist)
        {
            Debug.LogWarning("이전 페이지가 없습니다");
            return;
        }
        SetPlaceSection(_curSectionIndex - 1);
    }

    private async void SetPlaceSection(int placeSectionIndex)
    {
        if (placeSectionIndex < 0 || placeSectionIndex >= _sections.Count)
        {
            Debug.LogWarning($"Index {placeSectionIndex}에 해당하는 섹션을 찾을 수 없습니다");
            return;
        }
        if(CurSection != null){
            CurSection.StartDetectingPlacePoints(false);
        }

        _curSectionIndex = placeSectionIndex;
        CameraController.MoveX(CurPlaceSection.SectionCenterX, 1f);

        EventPlan eventPlanToPlay = CurSection.EventPlan;
        if (eventPlanToPlay != null && EventTimeService.IsCurrentTimeEquals(eventPlanToPlay.EventTime))
        {
            await EventProcessor.PlayEvent(eventPlanToPlay);
            CurSection.StartDetectingPlacePoints(true);
        }
    }
}
