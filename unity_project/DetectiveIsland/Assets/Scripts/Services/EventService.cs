using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime;
    [SerializeField] private PlaceData _placeData;
    [SerializeField] private TextAsset _scenarioFile; // JSON 파일 경로 또는 이름

    public EventPlan(EventTime eventTime, PlaceData placeData, TextAsset scenarioFile)
    {
        _eventTime = eventTime;
        _placeData = placeData;
        _scenarioFile = scenarioFile;
    }

    public PlaceData PlaceData { get => _placeData; }
    public TextAsset ScenarioFile { get => _scenarioFile; }
    public EventTime EventTime { get => _eventTime;  }
}

public static class EventService
{
    private static EventRoadMap _eventRoadmap;
    private static EventTime _curEventTime = null;
    public static EventTime CurEventTime { get => _curEventTime; }

    public static void Initialize()
    {       
         _eventRoadmap = Resources.Load<EventRoadMap>("EventRoadMap/MainEventRoadMap");
        if (_eventRoadmap == null)
        {
            Debug.LogError($"Failed to load EventRoadMap from Resources folder with filename: {"EventRoadMap"}");
        }
    }
    public static void SetCurEventTime(string date, int hour, int minute)
    {
        _curEventTime = new EventTime(date, hour, minute);
    }

    public static void SetCurEventTime(EventTime eventTime)
    {
        Debug.Log($"새로 설정된 EventTime : {eventTime.Date} - {eventTime.Hour}시 {eventTime.Minute}분");
        _curEventTime = eventTime;
    }
    public static EventPlan GetEventPlan(EventTime eventTime, string placeID){
        return _eventRoadmap.GetEventPlan(eventTime, placeID);
    }
}