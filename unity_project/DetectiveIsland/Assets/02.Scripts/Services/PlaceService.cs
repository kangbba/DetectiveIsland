using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;

public enum EPlaceID
{
    None,
    HospitalBedroom,
    Test
    // 다른 장소들...
}

public static class PlaceService
{
    private static Transform _placePanel;
    private static List<Place> _placePrefabs = new List<Place>();

    private static Place _curPlace;
    public static Place CurPlace { get => _curPlace; }
    public static List<Place> PlacePrefabs { get => _placePrefabs; }

    public static void Load()
    {
        _placePanel = new GameObject("Place Panel").transform;
        _placePrefabs = ArokaUtils.LoadResourcesFromFolder<Place>("PlacePrefabs");
    }

    private static Place MakePlace(EPlaceID placeID, float totalTime)
    {
        Place placePrefab = GetPlacePrefab(placeID);
        if (placePrefab == null)
        {
            Debug.LogWarning($"{placeID}에 해당하는 Place Prefab 찾을 수 없음");
            return null;
        }
        Place instancedPlace = GameObject.Instantiate(placePrefab, _placePanel.transform);
        instancedPlace.transform.localPosition = Vector3.zero;
        instancedPlace.FadeInFromStart(totalTime);
        return instancedPlace;
    }

    private static Place GetPlacePrefab(EPlaceID placeID)
    {
        Place place = _placePrefabs.FirstOrDefault(placePrefab => placePrefab.PlaceID == placeID);
        if (place == null)
        {
            Debug.LogWarning($"{placeID} 이름의 Place 찾을수 없음");
        }
        return place;
    }

    public static void MoveToPlace(EPlaceID placeID, int sectionIndex){

        if(_curPlace != null){
            _curPlace.FadeOutAndDestroy(1f);
        }
        Place place = MakePlace(placeID, 1f);
        place.Initialize(sectionIndex);
        _curPlace = place;
    }
}
