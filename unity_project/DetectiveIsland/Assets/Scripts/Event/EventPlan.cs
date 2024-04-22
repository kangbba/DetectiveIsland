
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime = new EventTime("2024-04-01", 9 , 0); 
    [SerializeField] private List<PlaceScenario> _placeScenarios = new List<PlaceScenario>();


    public EventTime EventTime { get => _eventTime; }
    public List<PlaceScenario> PlaceScenarios { get => _placeScenarios; }

    public PlaceScenario GetPlaceScenario(string placeID)
    {
        return _placeScenarios.FirstOrDefault(placeScenario => placeScenario.PlaceID == placeID);
    }
    public bool IsAllSolved()
    {
        foreach (PlaceScenario scenario in _placeScenarios)
        {
            if (!scenario.IsAllSolved())  // Assuming EventAction has a method to check its own condition
            {
                return false;  // If any condition is not met, return false
            }
        }
        return true;  // All conditions are met
    }

    public void Initialize(){
        foreach(PlaceScenario placeScenario in _placeScenarios){
            placeScenario.SetViewed(false);
        }
    }
}
