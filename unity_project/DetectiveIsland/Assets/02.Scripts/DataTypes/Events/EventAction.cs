using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAction
{
    void Execute();
}

public class EventAction
{
    private IAction _action;

    public EventAction(IAction action)
    {
        _action = action;
    }

    public void Execute()
    {
        _action.Execute();
    }
}

public class CollectItemAction : IAction
{
    private EItemID _itemID;

    public CollectItemAction(EItemID itemID)
    {
        _itemID = itemID;
    }

    public void Execute()
    {
        OwnershipService.SetItemOwnership(_itemID, true);
        Debug.Log("Collecting item: " + _itemID);
    }
}

public class GiveItemAction : IAction
{
    private EItemID _itemID;

    public GiveItemAction(EItemID itemID)
    {
        _itemID = itemID;
    }

    public void Execute()
    {
        OwnershipService.SetItemOwnership(_itemID, false);
        Debug.Log("Giving item: " + _itemID);
    }
}

public class MoveToPlaceAction : IAction
{
    private EPlaceID _placeID;
    private int _sectionIndex;

    public MoveToPlaceAction(EPlaceID placeID, int sectionIndex)
    {
        _placeID = placeID;
        _sectionIndex = sectionIndex;
    }

    public void Execute()
    {
        EventProcessor.MoveToPlace(_placeID, _sectionIndex).Forget();
        Debug.Log("Moving to place: " + _placeID);
    }
}
