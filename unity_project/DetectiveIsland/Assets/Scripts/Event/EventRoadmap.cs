using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "EventRoadMap", menuName = "EventRoadMap")]
public class EventRoadMap : ScriptableObject
{
    [SerializeField] private List<EventPlan> _eventPlans = new List<EventPlan>();

    public EventPlan FirstEventPlan { get => _eventPlans[0]; }
    public List<EventPlan> EventPlans => _eventPlans;
}




