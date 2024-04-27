using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.Build;
using UnityEngine;

public static class PlaceUIService
{
    private static PlaceUIPanel _placeUIPanel; 

    public static void Load()
    {
        _placeUIPanel = UIManager.Instance.PlaceUIPanel;
    }
    public static async UniTask CreateAndShowPlaceBtns(string placeID, Action<string> moveAction){
        _placeUIPanel.CreatePlaceButtons(placeID, moveAction);
        _placeUIPanel.SetInteractablePlaceButtons(false);
        _placeUIPanel.SetOnPanel(true, true, true, .3f);
        await UniTask.WaitForSeconds(.3f);
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

