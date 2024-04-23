using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using UnityEngine;

public class PlacePanel : ArokaAnim
{
    [SerializeField] private Place _placePrefab;
    private Place _curPlace;
    public void SetPlace(PlaceData placeData)
    {
        if(_curPlace != null){
            _curPlace.Exit(1f);
        }
        // 새로운 GameObject 생성
        _curPlace = Instantiate(_placePrefab, transform);
        _curPlace.transform.localScale = Vector3.one * placeData.ScaleFactor;
        _curPlace.Initialize(placeData);
        _curPlace.Enter(1f);
    }
}
