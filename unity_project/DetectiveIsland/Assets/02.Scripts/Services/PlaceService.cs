using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;

public static class PlaceService 
{
    private static Transform _placePanel;
    private static List<Place> _placePrefabs = new List<Place>();

    private static Place _curPlace;
    public static Place CurPlace { get => _curPlace; }

    public static void Load(){
        _placePanel = new GameObject("Place Panel").transform;
        _placePrefabs = ArokaUtils.LoadResourcesFromFolder<Place>("PlacePrefabs");
    }

    public static Place MakePlace(string placeID, float totalTime){

        Place placePrefab = PlaceService.GetPlacePrefab(placeID);
        if(placePrefab  == null){
            Debug.LogWarning($"{placeID}에 해당하는 Place Prefab 찾을 수 없음");
        }
        Place instancedPlace = GameObject.Instantiate(placePrefab, _placePanel.transform);
        _curPlace = instancedPlace;
        instancedPlace.transform.localPosition = Vector3.zero;
        instancedPlace.Initialize();
        instancedPlace.FadeInFromStart(totalTime);
        return instancedPlace;
    }
    
    public static Place GetPlacePrefab(string placeID){
        Place place = _placePrefabs.FirstOrDefault(placePrefab => placePrefab.PlaceID == placeID);
        if(place == null){
            Debug.LogWarning($"{placeID} 이름의 Place 찾을수 없음");
        }
        return place;
    }

    public static void CurPlaceFadeIn(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning("_curPlace is null");
            return;
        }
        _curPlace.FadeIn(totalTime);
    } 
    
    private static void CurPlaceFadeInFromStart(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning("_curPlace is null");
            return;
        }
        _curPlace.FadeInFromStart(totalTime);
    }

    private static void CurPlaceFadeOut(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning("_curPlace is null");
            return;
        }
        _curPlace.FadeOut(totalTime);
    }
    private static void CurPlaceFadeOutThenDestroy(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning($"_curPlace 이 없음");
            return;
        }
        _curPlace.FadeOutAndDestroy(totalTime);
    }


}
