using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.CoroutineUtils;
using UnityEngine;


public static class EventService
{
    private static  EventRoadMap         _eventRoadmap;
    private static  EventTime            _curEventTime     =   null;
    public static   List<EventPlan>       EventPlans            { get =>  _eventRoadmap.EventPlans; }
    public static   EventTime             CurEventTime          { get => _curEventTime;             }

    public static void Load()
    {       
         _eventRoadmap = Resources.Load<EventRoadMap>("EventRoadMap/MainEventRoadMap");
        if (_eventRoadmap == null)
        {
            Debug.LogError($"Failed to load EventRoadMap from Resources folder with filename: {"EventRoadMap"}");
        }
    }
    public static void LogEventPlan(EventPlan eventPlan){
        for(int i = 0 ; i < eventPlan.PlaceScenarios.Count ; i++){
            if(i == 0 ){
                 Debug.Log($"***************************");
            }
            PlaceScenario placeScenario = eventPlan.PlaceScenarios[i];
            Debug.Log($"-----{i+1}번째 장소 시나리오 ----- ");
            Debug.Log($"[장소 : {placeScenario.PlaceID}]");
            Debug.Log($"[해결 여부 : {(placeScenario.IsAllSolved() ? "해결됨" : " 해결안됨")}]");
            if(i == (eventPlan.PlaceScenarios.Count - 1) ){
                 Debug.Log($"***************************");
            }
        }
    }
    public static EventPlan GetFirstEventPlan(){
        return _eventRoadmap.EventPlans[0];
    }

    public static EventPlan GetEventPlan(EventTime eventTime)
    {
        foreach (EventPlan plan in _eventRoadmap.EventPlans)
        {
            if (plan.EventTime.Equals(eventTime))
            {
                return plan;
            }
        }
        return null;
    }
    public static EventPlan GetNextEventPlan(EventTime eventTime)
    {
        EventPlan eventPlan = GetEventPlan(eventTime);
        int currentIndex = EventPlans.IndexOf(eventPlan);
        if (currentIndex == -1 || currentIndex + 1 >= EventPlans.Count)
        {
            Debug.LogWarning("Current EventPlan is the last one or not found.");
            return null; 
        }
        return EventPlans[currentIndex + 1];
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


    // public static List<EventPlan> GetEventPlansByDate(string date)
    // {
    //     int year = int.Parse(date.Split('-')[0]);
    //     int month = int.Parse(date.Split('-')[1]);
    //     int day = int.Parse(date.Split('-')[2]);
        
    //     List<EventPlan> eventPlans = new List<EventPlan>();
    //     foreach (EventPlan plan in _eventRoadmap.EventPlans)
    //     {
    //         if (plan.EventTime.CompareDate(year, month, day) == TimeRelation.Same)
    //         {
    //             eventPlans.Add(plan);
    //         }
    //     }
    //     return eventPlans;
    // }

    
}