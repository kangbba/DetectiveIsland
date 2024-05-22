using System;
using System.Collections.Generic;
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
        ItemService.OwnItem(_itemID, true);
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
        ItemService.OwnItem(_itemID, false);
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
        PlaceService.MoveToPlace(_placeID, _sectionIndex);
        Debug.Log("Moving to place: " + _placeID);
    }
}
