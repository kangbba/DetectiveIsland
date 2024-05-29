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
    Town1 = 3,
    Town2 = 4,
    Town3 = 5,
    Town4 = 6,
    Town5 = 7,
    Town6 = 8,
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
    public static void MoveToPlace(EPlaceID placeID, int sectionIndex)
    {
        if (CurPlace != null)
        {
            Place placeToDestroy = CurPlace;
            _curPlace = null;
            placeToDestroy.OnExit();
            placeToDestroy.FadeOutAndDestroy(1f);
        }
        Place place = MakePlaceAndRegister(placeID, 1f);
        _curPlace = place;
        place.OnEnter(sectionIndex);
        UIManager.SetMouseCursorMode(EMouseCursorMode.Normal);
    }
    private static Place MakePlaceAndRegister(EPlaceID placeID, float totalTime)
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

    public static PlaceSection GetPlaceSection(EPlaceID placeID, int placeSectionIndex)
    {
        Place place = _placePrefabs.FirstOrDefault(p => p.PlaceID == placeID);
        if (place != null)
        {
            if (placeSectionIndex >= 0 && placeSectionIndex < place.PlaceSections.Count)
            {
                return place.PlaceSections[placeSectionIndex];
            }
            else
            {
                Debug.LogWarning($"Invalid place section index: {placeSectionIndex} for placeID: {placeID}");
            }
        }
        else
        {
            Debug.LogWarning($"Place with ID {placeID} not found");
        }
        return null;
    }
}
