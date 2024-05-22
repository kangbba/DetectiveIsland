using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlacePoint : WorldBtn
{
    [SerializeField] private TextAsset _scenarioJsonFile;
    private Scenario _scenario;
    private bool _isContainGainPlace;
    private EPlaceID _placeIdToGo;
    private int _clickCount;

    // Delegate for click action
    private System.Action _onClickAction;

    protected override void Start()
    {
        base.Start();
        if (_scenarioJsonFile == null)
        {
            Debug.LogWarning("대본 없는 PLACE POINT 존재");
            return;
        }

        _scenario = EventService.LoadScenario(_scenarioJsonFile);
        
        foreach (var element in _scenario.Elements)
        {
            if (element is GainPlace gainPlace)
            {
                _isContainGainPlace = true;
                _placeIdToGo = gainPlace.ID;
                Debug.Log($"GainPlace ID: {gainPlace.ID}");
            }
        }
    }

    protected override void OnClicked()
    {
        base.OnClicked();
        Debug.Log("PlacePoint clicked");

        _clickCount++;

        if(_clickCount >=2 && _isContainGainPlace){
            _onClickAction = () => new EventAction(new MoveToPlaceAction(_placeIdToGo, 0)).Execute();
        }
        else{
            _onClickAction = () => StartScenarioTask().Forget();
        }
        
        // Invoke the assigned click action
        _onClickAction?.Invoke();
    }

    private async UniTaskVoid StartScenarioTask()
    {
        try
        {
            await EventProcessor.PlaySmallEvent(_scenario);
        }
        finally
        {
            // Perform any cleanup or state reset if necessary
        }
    }
}
