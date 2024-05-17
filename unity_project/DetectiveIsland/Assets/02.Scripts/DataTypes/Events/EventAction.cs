using System;
using UnityEngine;

public enum EActionType
{
    None = 0,
    CollectItem = 1,
    GiveItem = 2,
    MoveToPlace = 3,
}

[System.Serializable]
public class EventAction
{    
    
    [SerializeField] private EActionType _actionType;
    [SerializeField] private string _targetID;
    
    public event Action<EActionType, bool> OnActionCompleted;  // 이벤트 선언

    public EventAction(EActionType actionType, string targetID)
    {
        _actionType = actionType;
        _targetID = targetID;
    }

    public EActionType ActionType => _actionType;
    public string TargetID => _targetID;

    public void ExecuteAction()
    {
        switch (_actionType)
        {
            case EActionType.CollectItem:
                ItemService.OwnItem(_targetID, true);  // Assuming Collect() tries to collect the item
                Debug.Log("Attempting to collect item: " + _targetID);
                break;
            case EActionType.GiveItem:
                ItemService.OwnItem(_targetID, false);  // Assuming Collect() tries to collect the item
                Debug.Log("Attempting to collect item: " + _targetID);
                break;
            case EActionType.MoveToPlace:
                EventProcessor.Move(_targetID);
                break;
            default:
                Debug.LogError("Unsupported action type: " + _actionType);
                break;
        }
    }
}
