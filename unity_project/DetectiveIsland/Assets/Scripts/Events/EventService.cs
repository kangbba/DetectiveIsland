using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static void AllEventReset(){
        foreach(EventPlan eventPlan in EventPlans){
            eventPlan.Reset();
        }
    }

    
}