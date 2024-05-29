using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventRoadmap", menuName = "ScriptableObjects/EventRoadmap")]
public class EventRoadmap : ScriptableObject
{
    [SerializeField] private List<EventPlan> allEventPlans = new List<EventPlan>();

    public List<EventPlan> AllEventPlans => allEventPlans;
}
