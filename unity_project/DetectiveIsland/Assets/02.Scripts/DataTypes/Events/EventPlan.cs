
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime = new EventTime("2024-04-01", 9 , 0); 
    [SerializeField] private List<ScenarioData> _scenarioDatas = new List<ScenarioData>();


    public EventTime EventTime { get => _eventTime; }
    public List<ScenarioData> ScenarioDatas { get => _scenarioDatas; }

    public ScenarioData GetScenarioData(EPlaceID placeID)
    {
        return _scenarioDatas.FirstOrDefault(placeScenario => placeScenario.PlaceID == placeID);
    }
    public bool IsAllSolved()
    {
        foreach (ScenarioData scenario in _scenarioDatas)
        {
            if (!scenario.IsSolved)  // Assuming EventAction has a method to check its own condition
            {
                return false;  // If any condition is not met, return false
            }
        }
        return true;  // All conditions are met
    }

    public void Reset(){
        foreach(ScenarioData placeScenario in _scenarioDatas)
        {
            placeScenario.ResetScenarioData();
        }
    }
}