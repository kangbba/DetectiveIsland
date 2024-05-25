
using ArokaInspector.Attributes;
using UnityEngine;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime = new EventTime("2024-04-01", 9, 0);

    [SerializeField] private TextAsset _scenarioFile;

    public TextAsset ScenarioFile => _scenarioFile;
    public EventTime EventTime => _eventTime;

}