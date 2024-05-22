using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum EPlacePointState{
    NonDetected,
    Detected,
    PlaceButton,
}
public class PlacePoint : WorldBtn
{
    [SerializeField] private TextAsset _scenarioJsonFile;
    private Scenario _scenario;

    private GainPlace _gainPlace;

    private EPlacePointState _placePointState;

    protected override void Start()
    {
        base.Start();
        if (_scenarioJsonFile == null)
        {
            Debug.LogWarning("대본 없는 PLACE POINT 존재");
            return;
        }
         _scenario = EventService.LoadScenario(_scenarioJsonFile);
        
        _gainPlace = _scenario.Elements.FirstOrDefault(element => element is GainPlace) as GainPlace;
    }

    public void SetState(EPlacePointState placePointState){
        _placePointState = placePointState;

        switch(placePointState){
            case EPlacePointState.NonDetected:
            break;
            case EPlacePointState.Detected:
            StartScenarioTask().Forget();
            break;
            case EPlacePointState.PlaceButton:
            if(_gainPlace != null){
                new EventAction(new MoveToPlaceAction(_gainPlace.ID, 0)).Execute();
            }
            break;
        }
        
    }

    protected override void OnClicked()
    {
        base.OnClicked();
        if(_placePointState == EPlacePointState.NonDetected){
            SetState(EPlacePointState.Detected);
        }
        else if (_placePointState == EPlacePointState.Detected){
            SetState(EPlacePointState.PlaceButton);
        }
    }

    private async UniTaskVoid StartScenarioTask()
    {
        await EventProcessor.PlaySectionScenario(_scenario);
    }
}
