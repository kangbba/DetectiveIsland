

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaceScenario{
     
    [SerializeField] private    string           _placeID = "";
    [SerializeField] private    TextAsset        _scenarioFile;
    [SerializeField] private    List<EventAction> _solveConditions = new List<EventAction>();
    [SerializeField] private    bool         _isViewed;
    public   List<EventAction> ExitConditions { get => _solveConditions; } 
    public   string           PlaceID             { get => _placeID;              }
    public   TextAsset        ScenarioFile        { get => _scenarioFile;         }
    public bool IsViewed { get => _isViewed; }

    public bool IsAllSolved()
    {
        foreach (EventAction action in _solveConditions)
        {
            if (!action.CheckActionFulfilled())  // Assuming EventAction has a method to check its own condition
            {
                return false;  // If any condition is not met, return false
            }
        }
        return true;  // All conditions are met
    }
    public void ShowCharacters(){
    }
    public void SetViewed(bool b){
        _isViewed = b;
    }
}