using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : Manager<PlaceData>
{
    public override void Initialize(string folderName, GameObject mainPanel)
    {
        base.Initialize(folderName, mainPanel);
        Debug.Log("PlaceManager initialized with place panel: " + MainPanel.name);
    }
    public PlaceData GetPlaceData(string placeID)
    {
        foreach (PlaceData place in _dataList)
        {
            if (place.PlaceID == placeID)
            {
                return place;
            }
        }
        return null;
    }
}
