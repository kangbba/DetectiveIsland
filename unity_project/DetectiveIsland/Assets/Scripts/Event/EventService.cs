using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.CoroutineUtils;
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
    [SerializeField] private    EventCondition   _eventCondition = null;
    [SerializeField] private    TextAsset        _scenarioFile;

    public string           PlaceID              { get => _placeID; }
    public TextAsset        ScenarioFile         { get => _scenarioFile; }
    public EventTime        EventTime            { get => _eventTime;  }
    public EventCondition   EventCondition       { get => _eventCondition; }
}

public static class EventService
{
    private static EventRoadMap         _eventRoadmap;
    private static EventTime            _curEventTime = null;
    public static List<EventPlan>       EventPlans   => _eventRoadmap.EventPlans;
    public static EventTime             CurEventTime { get => _curEventTime; }

    public static void Load()
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
        _curEventTime = new EventTime(eventTime.Date, eventTime.Hour, eventTime.Minute);
    }

    // 입력된 EventTime 기준으로 이미 완료된 데일리 이벤트 플랜을 필터링하는 확장 메서드
    public static List<EventPlan> EventTimeFilter(this List<EventPlan> eventPlans, EventTime inputTime, TimeRelation timeRelation)
    {
        return eventPlans.Where(plan => plan.EventTime.CompareTime(inputTime) == timeRelation && plan.EventTime.Date == inputTime.Date).ToList();
    }

    public static EventPlan GetFirstEventPlan(){
        return _eventRoadmap.EventPlans[0];
    }

    public static List<EventPlan> GetEventPlansByDate(string date)
    {
        int year = int.Parse(date.Split('-')[0]);
        int month = int.Parse(date.Split('-')[1]);
        int day = int.Parse(date.Split('-')[2]);
        
        List<EventPlan> eventPlans = new List<EventPlan>();
        foreach (EventPlan plan in _eventRoadmap.EventPlans)
        {
            if (plan.EventTime.CompareDate(year, month, day) == TimeRelation.Same)
            {
                eventPlans.Add(plan);
            }
        }
        return eventPlans;
    }
    public static List<EventPlan> GetEventPlans(EventTime eventTime)
    {
        List<EventPlan> eventPlans = new List<EventPlan>();
        foreach (EventPlan plan in _eventRoadmap.EventPlans)
        {
            if (plan.EventTime.Equals(eventTime))
            {
                eventPlans.Add(plan);
            }
        }
        return eventPlans;
    }

    public static EventPlan GetEventPlan(EventTime eventTime, string placeID)
    {
        foreach (EventPlan plan in _eventRoadmap.EventPlans)
        {
            if (plan.EventTime.Equals(eventTime) && plan.PlaceID == placeID)
            {
                return plan;
            }
        }
        return null;
    }

    
}