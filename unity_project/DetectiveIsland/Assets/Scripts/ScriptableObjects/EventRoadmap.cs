using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "EventRoadMap", menuName = "EventRoadMap")]
public class EventRoadMap : ScriptableObject
{
    [SerializeField] private List<EventPlan> _eventPlans = new List<EventPlan>();

    public EventPlan FirstEventPlan { get => _eventPlans[0]; }
    public List<EventPlan> EventPlans => _eventPlans;

    public List<EventPlan> GetEventPlansByDate(string date)
    {
        List<EventPlan> eventPlans = new List<EventPlan>();
        foreach (EventPlan plan in _eventPlans)
        {
            if (plan.EventTime.IsSameDate(date))
            {
                eventPlans.Add(plan);
            }
        }
        return eventPlans;
    }
    public List<EventPlan> GetEventPlansByEventTime(EventTime eventTime)
    {
        List<EventPlan> eventPlans = new List<EventPlan>();
        foreach (EventPlan plan in _eventPlans)
        {
            if (plan.EventTime.Equals(eventTime))
            {
                eventPlans.Add(plan);
            }
        }
        return eventPlans;
    }

    public EventPlan GetEventPlan(EventTime eventTime, string placeID)
    {
        foreach (EventPlan plan in _eventPlans)
        {
            if (plan.EventTime.Equals(eventTime) && plan.PlaceID == placeID)
            {
                return plan;
            }
        }
        return null;
    }

}




