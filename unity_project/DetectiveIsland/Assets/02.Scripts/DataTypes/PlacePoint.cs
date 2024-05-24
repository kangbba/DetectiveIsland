using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlacePoint : WorldButton
{
    [SerializeField] private string[] _sentences;
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

    protected override async void OnButtonClicked()
    {
        if (_isEventPlaying)
        {
            return;
        }

        Debug.Log("PlacePoint clicked");
        _clickCount++;

        if (_clickCount >= 2 && _isContainGainPlace)
        {
            new EventAction(new MoveToPlaceAction(_placeIdToGo, 0)).Execute();
        }
        else
        {
            _isEventPlaying = true;
            SetButtonInteractable(false);  // 이벤트 실행 중 버튼 비활성화
            if (_sentences != null && _sentences.Length > 0)
            {
                Debug.Log("*문장들이 있으므로 다이얼로그 실행");
                await UIManager.ShowSimpleDialogue(_sentences);
            }
            if (_scenario != null)
            {
                Debug.Log("*시나리오 정보가 있으므로 시나리오 재생");
                await EventProcessor.PlayEvent(_scenario, false);
            }
            _isEventPlaying = false;
            Debug.Log("**ShowSimpleDialogue 또는 PlayEvent 가 모두 끝났음");
            SetButtonInteractable(true);  // 이벤트 종료 후 버튼 활성화
        }
    }
}
