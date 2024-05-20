using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;


[System.Serializable]
public class PlaceSection
{
    [SerializeField] private float _sectionCenterX;

    public float SectionCenterX => _sectionCenterX;
}
public class Place : SpriteEffector
{
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private List<PlaceSection> _placeSections = new List<PlaceSection>();
    private List<PlacePoint> _placePoints;
    private PlaceSection _curPlaceSection;

    public bool IsPreviousSectionExist => _placeSections.Count > 1 && (_curPlaceSection != _placeSections.First());
    public bool IsNextSectionExist => _placeSections.Count > 1 && (_curPlaceSection != _placeSections.Last());

    public EPlaceID PlaceID => _placeID;
    public string PlaceNameForUser => _placeNameForUser;

    public List<PlaceSection> PlaceSections => _placeSections;
    public PlaceSection CurPlaceSection => _curPlaceSection;
    public List<PlacePoint> PlacePoints => _placePoints;

    private void Start()
    {
        _placePoints = transform.GetComponentsInChildren<PlacePoint>().ToList();
        _placeSections = _placeSections.OrderBy(section => section.SectionCenterX).ToList();
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
        SetPlaceSection(_placeSections.IndexOf(_curPlaceSection) + 1);
    }

    public void SetPreviousPlaceSection()
    {
        if (!IsPreviousSectionExist)
        {
            Debug.LogWarning("이전 페이지가 없습니다");
            return;
        }
        SetPlaceSection(_placeSections.IndexOf(_curPlaceSection) - 1);
    }

    public void SetPlaceSection(int placeSectionIndex)
    {
        if (placeSectionIndex < 0 || placeSectionIndex >= _placeSections.Count)
        {
            Debug.LogWarning($"Index {placeSectionIndex}에 해당하는 섹션을 찾을 수 없습니다");
            return;
        }

        PlaceSection targetPlaceSection = _placeSections[placeSectionIndex];

        _curPlaceSection = targetPlaceSection;
        CameraController.MoveX(_curPlaceSection.SectionCenterX, 1f);
    }

    public void StartDetectingPlacePoints(bool isDetecting)
    {
        foreach (PlacePoint placePoint in _placePoints)
        {
            placePoint.StartDetectingMouseOver(isDetecting);
        }
    }

}
