using System.Collections.Generic;
using ArokaUtil;
using UnityEngine;

public class PlaceService
{
    private PlacePanel _placePanel;
    private List<PlaceData> _placeDatas;
    private PlaceData _curPlaceData;

    public PlaceData CurPlaceData { get => _curPlaceData;  }

    public void Initialize()
    {       
        _placePanel = UIManager.Instance.PlacePanel;
        _placeDatas = Utils.LoadDatasFromFolder<PlaceData>("PlaceDatas");
        Debug.Log(_placeDatas.Count);
    }
    public PlaceData GetPlaceData(string placeID)
    {
        foreach (PlaceData place in _placeDatas)
        {
            if (place.PlaceID == placeID)
            {
                return place;
            }
        }
        return null;
    }
    public void SetPlace(PlaceData placeData)
    {
        _curPlaceData = placeData;
        _placePanel.SetPlace(placeData);
    }

    public void SetOnPanel(bool b, float totalTime){
        _placePanel.SetAnim(b, totalTime);
    }
}
