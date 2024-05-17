using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

public enum EPlaceUIPanelState
{
    None,
    NormalMode,
    MovingMode,
    SearchingMode
}
public class PlaceUIPanel : UIStateManager<EPlaceUIPanelState>
{
    [SerializeField] private TextMeshProUGUI _curPlaceText;
    [SerializeField] private Transform _btnParent;
    [SerializeField] private PlaceButton _placeBtnPrefab;
    [SerializeField] private Button _movingBtn;
    [SerializeField] private Button _searchingBtn;
    [SerializeField] private Button _backBtn;
    [SerializeField] private Button _nextPageBtn;
    [SerializeField] private Button _previousPageBtn;

    private Place CurPlace => PlaceService.CurPlace;
    private List<PlaceButton> _instPlaceBtns = new List<PlaceButton>();
    private EPlaceUIPanelState _curPlaceUIPanelState;
    private EPlaceUIPanelState _previousUIPanelState;


    private void Start(){
        SetUIState(EPlaceUIPanelState.None, 0f);
        _movingBtn.onClick.AddListener(() => SetUIState(EPlaceUIPanelState.MovingMode, .5f));
        _searchingBtn.onClick.AddListener(() => SetUIState(EPlaceUIPanelState.SearchingMode, .5f));
        _backBtn.onClick.AddListener(() => SetUIState(_previousUIPanelState, .5f));

        _previousPageBtn.onClick.AddListener(() => OnClickedPreviousPageBtn());
        _nextPageBtn.onClick.AddListener(() => OnClickedNextPageBtn());
    }

    private void InstantiateMovingBtns(List<PlacePoint> movingPlacePoints){
        foreach(PlacePoint movingPlacePoint in movingPlacePoints){
            MakeMovingBtn(movingPlacePoint);
        }
    }
    private void DestroyMovingBtns(){
        for(int i = _instPlaceBtns.Count - 1 ; i >= 0 ; i--){
            Destroy(_instPlaceBtns[i].gameObject);
        }
        _instPlaceBtns.Clear();
    }
    private void MakeMovingBtn(PlacePoint placePoint){
        EventAction movingEventAction = placePoint.EventAction;
        if(movingEventAction.ActionType != EActionType.MoveToPlace){
            Debug.LogError($"잘못된 인풋 {movingEventAction}");
            return;
        }
        PlaceButton instPlaceBtn = Instantiate(_placeBtnPrefab, _btnParent);
        Place place = PlaceService.GetPlacePrefab(movingEventAction.TargetID);
        if(place == null){
            Debug.LogError($"잘못된 {movingEventAction.TargetID}");
            return;
        }
        instPlaceBtn.Initialize(place, placePoint, CameraController.MainCamera);
        _instPlaceBtns.Add(instPlaceBtn);
    }

    private void OnClickedPreviousPageBtn(){
        CurPlace.SetPreviousPage();
        RefreshPageBtns();
    }
    private void OnClickedNextPageBtn(){
        CurPlace.SetNextPage();
        RefreshPageBtns();
    }
    private void RefreshPageBtns(){
        Debug.Log($"현재페이지 {CurPlace.CurPageIndex}");
        _previousPageBtn.interactable = CurPlace.IsPreviousPageExist;
        _nextPageBtn.interactable = CurPlace.IsNextPageExist;
        DestroyMovingBtns();
        InstantiateMovingBtns(CurPlace.CurPagePlan.PlacePoints);
    }

    public override void SetUIState(EPlaceUIPanelState placeUIPanelState, float totalTime){
        base.SetUIState(placeUIPanelState, totalTime);
        if(_curPlaceUIPanelState == placeUIPanelState){
            return;
        }
        _previousUIPanelState = _curPlaceUIPanelState;
        _curPlaceUIPanelState = placeUIPanelState;
        RefreshPageBtns();

        switch(placeUIPanelState){
            case EPlaceUIPanelState.None:
            break;
            case EPlaceUIPanelState.NormalMode:
            break;
            case EPlaceUIPanelState.MovingMode:
            break;
            case EPlaceUIPanelState.SearchingMode:
            break;
            default:
            break;

        }
    }
}
