using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum EPlaceID
{
    None = 0,
    HospitalBedroom = 1,
    HospitalDoor = 2,
    TownDeep = 3,
    TownMiddle = 4,
    TownEntrance = 5,
    // 다른 장소들...
}

public static class PlaceService
{
    private static Transform _placePanel;
    private static List<Place> _placePrefabs = new List<Place>();

    private static Place _curPlace;
    public static Place CurPlace { get => _curPlace; }
    public static List<Place> PlacePrefabs { get => _placePrefabs; }

    private static bool _isMoving;

    public static void Load()
    {
        _placePanel = new GameObject("Place Panel").transform;
        _placePrefabs = ArokaUtils.LoadResourcesFromFolder<Place>("PlacePrefabs");
    }

    public static Place MakePlaceAndRegister(EPlaceID placeID, float totalTime)
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
        _curPlace = instancedPlace;
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
}
