using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlacePoint : WorldBtn
{
    [SerializeField] private string[] _simpleLines;
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
            _scenario = null;
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

    protected override async void OnClicked()
    {
        base.OnClicked();
        Debug.Log("PlacePoint clicked");

        _clickCount++;

        // 조건에 따라 비동기 작업 수행
        if (_clickCount >= 2 && _isContainGainPlace)
        {
            new EventAction(new MoveToPlaceAction(_placeIdToGo, 0)).Execute();
        }
        else
        {
            await UIManager.ShowDialogue(_simpleLines);
            if(_scenario != null)
            await EventProcessor.PlaySmallEvent(_scenario);
        }
    }
}
