

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenarioData{
    [SerializeField] private    EPlaceID         _placeID;
    [SerializeField] private    TextAsset        _scenarioFile;

    [SerializeField]  private bool _isEntered = false;
    [SerializeField]  private bool _isExited = false;

    [SerializeField] private List<EventAction> _solveConditions = new List<EventAction>();
    [SerializeField] private List<EventAction> _doneEventActions = new List<EventAction>(); // 완료된 이벤트 액션을 추적합니다.

    public   EPlaceID           PlaceID             {     get => _placeID;         }
    public   TextAsset        ScenarioFile        {     get => _scenarioFile;    }
    
    public ModifyPosition RecentModifyPosition { get; set; }

    public bool IsEntered { get => _isEntered; set => _isEntered = value; }
    public bool IsExited { get => _isExited; set => _isExited = value; }

    // 외부에서 이 메소드를 호출하여 완료된 액션을 등록
    public void ExecuteActionThenAdd(EventAction actionToExecute)
    {
        actionToExecute.Execute();
        _doneEventActions.Add(actionToExecute);
    }

    public void ResetScenarioData(){
        IsEntered = false;
        IsExited = false;
        _doneEventActions.Clear();
    }


    public bool IsSolved
    {
        get
        {
            foreach (EventAction condition in _solveConditions)
            {
                bool isConditionMet = false;
                foreach (EventAction doneAction in _doneEventActions)
                {
                    if (doneAction == condition)
                    {
                        isConditionMet = true;
                        break;
                    }
                }
                if (!isConditionMet)
                    return false;
            }
            return true;
        }
    }
    public bool IsSolvedAndExited
    {
        get
        {
            return IsExited && IsSolved;
        }
    }

}