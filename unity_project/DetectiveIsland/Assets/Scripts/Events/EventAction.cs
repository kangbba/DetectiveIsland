using UnityEngine;

public enum EActionType
{
    None = 0,
    Collect = 1,
    Move = 2,
}

[System.Serializable]
public class EventAction
{
    [SerializeField] private EActionType _actionType; // Action type
    [SerializeField] private string _targetID; // Target ID, e.g., character name, item ID

    public EventAction(EActionType actionType, string targetID)
    {
        _actionType = actionType;
        _targetID = targetID;
    }

    public EActionType ActionType => _actionType;
    public string TargetID => _targetID;

    // Executes the appropriate action based on the action type
    public void ExecuteAction()
    {
        switch (_actionType)
        {
            case EActionType.Collect:
                PerformCollectAction();
                break;
            case EActionType.Move:
                PerformMoveAction();
                break;
            default:
                Debug.LogError("Unsupported action type: " + _actionType);
                break;
        }
    }

    // Checks if the action has been successfully completed
    public bool IsActionCompleted()
    {
        switch (_actionType)
        {
            case EActionType.Collect:
                return CheckCollectCompleted();
            case EActionType.Move:
                return CheckMoveCompleted();
            default:
                Debug.LogError("Completion check not supported for action type:, always true " + _actionType);
                return true;
        }
    }

    private void PerformCollectAction()
    {
        ItemService.OwnItem(_targetID, true);  // Assuming Collect() tries to collect the item
        Debug.Log("Attempting to collect item: " + _targetID);
    }

    private bool CheckCollectCompleted()
    {
        return ItemService.IsOwnItem(_targetID);  // Check if the item is now owned
    }

    private void PerformMoveAction()
    {
        Debug.Log("Moving to location: " + _targetID);
        EventProcessor.Move(_targetID);
        // Implement movement logic
    }

    private bool CheckMoveCompleted()
    {
        return PlaceService.CurPlace.PlaceID == _targetID;
    }
}
