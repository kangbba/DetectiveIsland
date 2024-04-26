using System;
using UnityEngine;


public enum EActionType
{
    None = 0,
    Talk = 1, 
    Collect = 2,
    Sleep = 3,
}

[System.Serializable]
public class EventAction
{
    public EventAction(EActionType actionType, string targetID)
    {
        _actionType = actionType;
        _targetID = targetID;
    }

    [SerializeField] private EActionType     _actionType; // 유형
    
    [SerializeField] private string             _targetID; // 대상 ID, 예: 캐릭터 이름, 아이템 ID


    public  EActionType  ActionType       => _actionType;
    public  string  TargetID         => _targetID;

    public bool CheckActionFulfilled()
    {
        switch (_actionType)
        {
            case EActionType.Collect:
                return CheckItemOwnership(_targetID);
            default:
                Debug.LogError("Unsupported action type for checking conditions.");
                return false;
        }
    }
    private bool CheckItemOwnership(string itemID)
    {
        return ItemService.IsOwnItem(itemID);
    }
}

