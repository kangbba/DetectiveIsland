using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

public enum EPlaceUIPanelState
{
    None,
    NavigateMode,
}
public class PlaceUIPanel : UIStateManager<EPlaceUIPanelState>
{
    [SerializeField] private TextMeshProUGUI _curPlaceText;
    [SerializeField] private Transform _btnParent;
    [SerializeField] private Button _nextPageBtn;
    [SerializeField] private Button _previousPageBtn;

    private Place CurPlace => PlaceService.CurPlace;
    private EPlaceUIPanelState _curPlaceUIPanelState;


    private void Start(){
        SetUIState(EPlaceUIPanelState.None, 0f);

        _previousPageBtn.onClick.AddListener(() => CurPlace.SetPreviousPlaceSection());
        _nextPageBtn.onClick.AddListener(() => CurPlace.SetNextPlaceSection());
    }

    public override void SetUIState(EPlaceUIPanelState placeUIPanelState, float totalTime){
        base.SetUIState(placeUIPanelState, totalTime);
        if(placeUIPanelState != EPlaceUIPanelState.None && _curPlaceUIPanelState == placeUIPanelState){
            return;
        }
        _curPlaceUIPanelState = placeUIPanelState;

        switch(placeUIPanelState){
            case EPlaceUIPanelState.None:
            break;
            case EPlaceUIPanelState.NavigateMode:
            break;
            default:
            break;

        }
    }
}
