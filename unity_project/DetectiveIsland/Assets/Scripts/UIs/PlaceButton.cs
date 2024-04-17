using UnityEngine;
using TMPro;
using System;

public class PlaceButton : DataButton
{
    [SerializeField] private TextMeshProUGUI _text;
    private PlaceData _placeData;

    public void Initialize(PlaceData placeData, Action<string> action)
    {
        _placeData = placeData;
        _text.text = placeData.PlaceNameForUser;
        SetupButton(placeData.PlaceID, action);
    }
}
