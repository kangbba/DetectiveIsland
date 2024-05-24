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

    private bool _isEventPlaying;

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
        if(_isEventPlaying){
            return;
        }
        Debug.Log("PlacePoint clicked");
        base.OnClicked();
        _clickCount++;

        if (_clickCount >= 2 && _isContainGainPlace)
        {
            new EventAction(new MoveToPlaceAction(_placeIdToGo, 0)).Execute();
        }
        else
        {
            _isEventPlaying = true;
            if(_simpleLines != null && _simpleLines.Length > 0){
                await UIManager.ShowSimpleDialogue(_simpleLines);
            }
            if(_scenario != null){
                await EventProcessor.PlayEvent(_scenario, false);
            }
            _isEventPlaying = false;
        }
    }
}
