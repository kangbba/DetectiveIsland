using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Place : ArokaEffector
{
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private string _placeNameForUser;
    private List<PlaceSection> _placeSections = new List<PlaceSection>();
    private List<PlacePoint> _placePoints = new List<PlacePoint>();
    private int _curSectionIndex = -1;

    public EPlaceID PlaceID => _placeID;
    public List<PlaceSection> PlaceSections => _placeSections;

    private PlaceUIPanel _placeUIPanel;

    public PlaceSection CurPlaceSection 
    {
        get => (_curSectionIndex >= 0 && _curSectionIndex < _placeSections.Count) ? _placeSections[_curSectionIndex] : null;
        set
        {
            if (_curSectionIndex != value.SectionIndex)
            {
                _curSectionIndex = value.SectionIndex;
            }
        }
    }


    public void Initialize(int initialSectionIndex, PlaceUIPanel placeUIPanel)
    {
        _placeUIPanel = placeUIPanel;
        SpriteRenderer.sortingOrder = -1;

        _placePoints = GetComponentsInChildren<PlacePoint>().ToList();
        _placeSections = GetComponentsInChildren<PlaceSection>()
                            .OrderBy(section => section.SectionCenterX)
                            .ToList();

        SetPlaceSection(initialSectionIndex);

        _placeUIPanel.SetCurPlaceNameForUser(_placeNameForUser);
    }

    //place points로 UI에게 만들어달라고 요청
    public void MakeBtnsWithPlacePoints(){
        _placeUIPanel.MakePlacePointBtns(_placePoints);
    }
    //UI에게 place points로 만들어진 버튼들을 파괴해달라고 요청
    public void DestroyPlacePointBtns(){
        _placeUIPanel.DestroyPlacePointBtns();
    }
    
    //UI에게 Place 를 상호작용할수있는 UI모드를 조정해달라고 요청
    public void SetPlaceMode(EPlaceUIPanelState mode, float totalTime){
        _placeUIPanel.SetUIState(mode, totalTime);
    }

    public void SetPlaceSection(int placeSectionIndex)
    {
        Debug.Log($"{placeSectionIndex}로 설정 시도");
        if (placeSectionIndex < 0 || placeSectionIndex >= _placeSections.Count)
        {
            Debug.LogWarning($"Index {placeSectionIndex}에 해당하는 섹션을 찾을 수 없습니다");
            return;
        }
        _curSectionIndex = placeSectionIndex;
    }

    //UI가 이 함수를 통해 쉽게 페이지를 넘길수있게 함.
    public void SetNextPlaceSectionForUI()
    {
        if (_curSectionIndex < _placeSections.Count - 1)
        {
            SetPlaceSection(_curSectionIndex + 1);
            CameraController.MoveX(CurPlaceSection.SectionCenterX, 1f);
        }
        else
        {
            Debug.LogWarning("다음 페이지가 없습니다");
        }
    }

    public void SetPreviousPlaceSectionForUI()
    {
        if (_curSectionIndex > 0)
        {
            SetPlaceSection(_curSectionIndex - 1);
            CameraController.MoveX(CurPlaceSection.SectionCenterX, 1f);
        }
        else
        {
            Debug.LogWarning("이전 페이지가 없습니다");
        }
    }
}
