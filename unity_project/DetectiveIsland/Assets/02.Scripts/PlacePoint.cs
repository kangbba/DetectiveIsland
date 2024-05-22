using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlacePoint : WorldBtn
{
    [SerializeField] private TextAsset _scenarioJsonFile;

    protected override void Start()
    {
        base.Start();
        if (_scenarioJsonFile == null)
        {
            Debug.LogWarning("대본 없는 PLACE POINT 존재");
        }
    }

    protected override void OnClicked()
    {
        base.OnClicked();
        if (!_isRunning)
        {
            Debug.Log("PlacePoint clicked");
            StartScenarioTask().Forget();
        }
    }

    private async UniTaskVoid StartScenarioTask()
    {
        _isRunning = true;
        try
        {
            Scenario scenario = EventService.LoadScenario(_scenarioJsonFile);
            await EventProcessor.PlaySectionScenario(scenario);
        }
        finally
        {
            _isRunning = false;
        }
    }
}
