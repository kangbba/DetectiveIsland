using System.Collections.Generic;
using ArokaUtil;
using UnityEngine;

public static class PlaceService
{
    private static PlacePanel _placePanel;
    private static PlaceData _curPlaceData;

    public static PlaceData CurPlaceData { get => _curPlaceData;  }

    public static void Initialize()
    {       
        _placePanel = UIManager.Instance.PlacePanel;
    }
    public static void SetPlace(PlaceData placeData)
    {
        _curPlaceData = placeData;
        _placePanel.SetPlace(placeData);
    }

    public static void SetOnPanel(bool b, float totalTime){
        _placePanel.SetAnim(b, totalTime);
    }

    
}
