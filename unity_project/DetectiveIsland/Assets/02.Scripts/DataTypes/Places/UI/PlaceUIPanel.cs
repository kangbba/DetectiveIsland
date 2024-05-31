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
public class PlaceUIPanel : ArokaUIState<EPlaceUIPanelState>
{
    [SerializeField] private TextMeshProUGUI _curPlaceText;
    [SerializeField] private Transform _btnParent;
    [SerializeField] private Button _nextPageBtn;
    [SerializeField] private Button _previousPageBtn;
    [SerializeField] private Button _placeBtnPrefab;

    private List<WorldTracker> _worldTrackers = new List<WorldTracker>();

    private Place CurPlace => PlaceService.CurPlace;
    private EPlaceUIPanelState _curPlaceUIPanelState;


    private void Start(){
        SetUIState(EPlaceUIPanelState.None, 0f);
        _previousPageBtn.onClick.AddListener(() => CurPlace.SetPreviousPlaceSectionForUI());
        _nextPageBtn.onClick.AddListener(() => CurPlace.SetNextPlaceSectionForUI());
        
        DestroyPlacePointBtns();
    }

    
    //place 가 이 함수를 통해 버튼을 생성할수있도록 함
    public void MakePlacePointBtns(List<PlacePoint> placePoints)
    {
        Debug.Log("OnPlaceEnterOnPlaceEnter");
        foreach (PlacePoint placePoint in placePoints)
        {
            if (_placeBtnPrefab == null)
            {
                Debug.LogError("Button Prefab could not be loaded from Resources.");
                return;
            }
            Button inst_placeBtn = Instantiate(_placeBtnPrefab.gameObject).GetComponent<Button>();
            inst_placeBtn.transform.SetParent(UIManager.MainCanvas.transform);
            inst_placeBtn.gameObject.AddComponent<WorldTracker>();
            WorldTracker worldTracker = inst_placeBtn.gameObject.GetComponent<WorldTracker>();
            worldTracker.Initialize(placePoint.transform);
            inst_placeBtn.onClick.AddListener(() => placePoint.ProperAction());

            _worldTrackers.Add(worldTracker);
        }
    }

    //place 가 이 함수를 통해 버튼을 파괴할수있도록 함
    public void DestroyPlacePointBtns()
    {
        Debug.Log("OnPlaceExitOnPlaceExit");
        for(int i = _worldTrackers.Count - 1 ; i >= 0 ; i--){
            Destroy(_worldTrackers[i].gameObject);
        }
        _worldTrackers.Clear();
    }

    public void SetCurPlaceNameForUser(string s){
        _curPlaceText.SetText(s);
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
