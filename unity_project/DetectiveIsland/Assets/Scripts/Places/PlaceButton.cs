using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceButton : EventActionBtn
{
    [SerializeField] private TextMeshProUGUI placeText;
    private Camera _mainCamera;
    private PlacePoint _placePoint;
    public void Initialize(Place place, PlacePoint placePoint, Camera mainCamera){
        placeText.SetText(place.PlaceNameForUser);
        _placePoint = placePoint;
        _mainCamera = mainCamera;
        base.Initialize(new EventAction(EActionType.MoveToPlace, place.PlaceID));
    }
    private void Update(){
        transform.position = _mainCamera.WorldToScreenPoint(_placePoint.transform.position) + Vector3.left * _mainCamera.transform.position.x;
    }
}
