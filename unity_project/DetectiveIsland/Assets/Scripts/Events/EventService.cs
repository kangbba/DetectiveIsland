using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.CoroutineUtils;
using UnityEngine;


public static class EventService
{
    private static  EventRoadMap         _eventRoadmap;
    public static   List<EventPlan>       EventPlans            { get =>  _eventRoadmap.EventPlans; }
  
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
            EventPlacePlan placeScenario = eventPlan.PlaceScenarios[i];
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
    
}