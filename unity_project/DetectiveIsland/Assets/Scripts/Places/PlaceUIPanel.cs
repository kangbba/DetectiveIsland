using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum EPlaceUIPanelState
{
    None,
    ShowBtnsMode,
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

    private List<PlaceButton> _instPlaceBtns = new List<PlaceButton>();
    private EPlaceUIPanelState _curPlaceUIPanelState;
    private EPlaceUIPanelState _previousUIPanelState;


    private void Start(){
        SetUIState(EPlaceUIPanelState.None, 0f);
        _movingBtn.onClick.AddListener(() => SetUIState(EPlaceUIPanelState.MovingMode, .5f));
        _searchingBtn.onClick.AddListener(() => SetUIState(EPlaceUIPanelState.SearchingMode, .5f));
        _backBtn.onClick.AddListener(() => SetUIState(_previousUIPanelState, .5f));
    }

    public void InstantiateMovingBtns(List<PlacePoint> movingPlacePoints){
        foreach(PlacePoint movingPlacePoint in movingPlacePoints){
            MakeMovingBtn(movingPlacePoint.EventAction, movingPlacePoint.transform.position);
        }
    }
    public void DestroyMovingBtns(){
        for(int i = _instPlaceBtns.Count - 1 ; i >= 0 ; i--){
            Destroy(_instPlaceBtns[i].gameObject);
        }
        _instPlaceBtns.Clear();
    }
    private void MakeMovingBtn(EventAction movingEventAction, Vector3 worldPos){
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
        instPlaceBtn.Initialize(place.PlaceID, place.PlaceNameForUser);
        instPlaceBtn.transform.position = CameraController.MainCamera.WorldToScreenPoint(worldPos);
        _instPlaceBtns.Add(instPlaceBtn);
    }

    public override void SetUIState(EPlaceUIPanelState placeUIPanelState, float totalTime){
        base.SetUIState(placeUIPanelState, totalTime);
        if(_curPlaceUIPanelState == placeUIPanelState){
            return;
        }
        _previousUIPanelState = _curPlaceUIPanelState;
        _curPlaceUIPanelState = placeUIPanelState;

        switch(placeUIPanelState){
            case EPlaceUIPanelState.MovingMode:
            CharacterService.AllCharacterFadeOut(.2f);
            break;
            case EPlaceUIPanelState.SearchingMode:
            CharacterService.AllCharacterFadeOut(.2f);
            break;
            case EPlaceUIPanelState.ShowBtnsMode:
            CharacterService.AllCharacterFadeIn(.2f);
            break;
            default:
            break;

        }
    }
    public void SetCurPlaceText(string placeNameForUser){
        _curPlaceText.SetText(placeNameForUser);
    }
}
