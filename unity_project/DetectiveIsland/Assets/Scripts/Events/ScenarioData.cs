

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenarioData{
    [SerializeField] private    string           _placeID = "";
    [SerializeField] private    TextAsset        _scenarioFile;

    [SerializeField]  private bool _isEntered = false;
    [SerializeField]  private bool _isExited = false;

    [SerializeField] private List<EventAction> _solveConditions = new List<EventAction>();
    [SerializeField] private List<EventAction> _doneEventActions = new List<EventAction>(); // 완료된 이벤트 액션을 추적합니다.

    public   string           PlaceID             {     get => _placeID;         }
    public   TextAsset        ScenarioFile        {     get => _scenarioFile;    }
    
    public ModifyPosition RecentModifyPosition { get; set; }

    public bool IsEntered { get => _isEntered; set => _isEntered = value; }
    public bool IsExited { get => _isExited; set => _isExited = value; }

    // 외부에서 이 메소드를 호출하여 완료된 액션을 등록
    public void ExecuteActionThenAdd(EventAction actionToExecute)
    {
        Debug.Log($"{actionToExecute.ActionType} {actionToExecute.TargetID} 의 행동을 추가했다.");
        actionToExecute.ExecuteAction();
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
                    if (doneAction.ActionType == condition.ActionType && doneAction.TargetID == condition.TargetID)
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