using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public static class PlaceUIService
{
    private static PlaceUIPanel _placeUIPanel; 

    public static void Load()
    {
        _placeUIPanel = UIManager.Instance.PlaceUIPanel;
    }
    public static IEnumerator CreateAndShowPlaceBtns(string placeID, Action<string> moveAction){
        _placeUIPanel.CreatePlaceButtons(placeID, moveAction);
        _placeUIPanel.SetInteractablePlaceButtons(false);
        _placeUIPanel.SetOnPanel(true, true, true, .3f);
        yield return new WaitForSeconds(.3f);
        _placeUIPanel.SetInteractablePlaceButtons(true);
    }
    public static void SetOnPanel(bool up, bool right, bool left, float totalTime)
    {   
        _placeUIPanel.SetOnPanel(up, left, right, totalTime);
    }

    public static void SetCurPlaceText(string selectedPlaceID){
        _placeUIPanel.SetCurPlaceText(selectedPlaceID);
    }
}

