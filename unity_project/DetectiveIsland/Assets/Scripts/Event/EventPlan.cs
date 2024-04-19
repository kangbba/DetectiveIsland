
using UnityEngine;

[System.Serializable]
public class EventPlan
{
    public EventPlan(EventTime eventTime, string placeID, TextAsset scenarioFile)
    {
        _eventTime = eventTime;
        _placeID = placeID;
        _scenarioFile = scenarioFile;
    }

    [SerializeField] private    EventTime        _eventTime = null;
    [SerializeField] private    string           _placeID = "";
    [SerializeField] private    EventAction   _eventEnterCondition = null;
    [SerializeField] private    EventAction   _timeProcessCondition = null;
    [SerializeField] private    TextAsset        _scenarioFile;

    public   string           PlaceID                { get => _placeID;              }
    public   TextAsset        ScenarioFile           { get => _scenarioFile;         }
    public   EventTime        EventTime              { get => _eventTime;            }
    public   EventAction   EventEnterCondition       { get => _eventEnterCondition;  }
    public   EventAction   TimeProcessCondition      { get => _timeProcessCondition; }
}