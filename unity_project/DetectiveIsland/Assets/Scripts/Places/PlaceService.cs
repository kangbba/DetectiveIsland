using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;

public static class PlaceService 
{

    private static PlaceUIPanel _placeUIPanel;
    private static PlacePanel _placePanel;
    private static List<Place> _placePrefabs = new List<Place>();
    private static Place _curPlace;

    public static Place CurPlace { get => _curPlace; }

    public static void Load(){
        _placePanel = UIManager.Instance.PlacePanel;
        _placeUIPanel = UIManager.Instance.PlaceUIPanel;
        _placePrefabs = ArokaUtils.LoadResourcesFromFolder<Place>("PlacePrefabs");
    }

    public static Place GetPlacePrefab(string placeID){
        Place place = _placePrefabs.FirstOrDefault(placePrefab => placePrefab.PlaceID == placeID);
        if(place == null){
            Debug.LogWarning($"{placeID} 이름의 Place 찾을수 없음");
        }
        return place;
    }
    public static void SetPlace(string placeID, float totalTime){

        if(_curPlace != null){
            FadeOutCurPlaceThenDestroy(totalTime);
        }
        _curPlace = InstantiatePlaceThenFadeIn(placeID, totalTime);
    }
    public static Place InstantiatePlaceThenFadeIn(string placeID, float totalTime){

        Place placePrefab = GetPlacePrefab(placeID);
        if(placePrefab  == null){
            Debug.LogWarning($"{placeID}에 해당하는 Place Prefab 찾을 수 없음");
        }
        Place instancedPlace = GameObject.Instantiate(placePrefab, _placePanel.transform);
        instancedPlace.transform.localPosition = Vector3.zero;
        instancedPlace.Initialize();
        instancedPlace.FadeIn(totalTime);
        return instancedPlace;
    }

    public static void FadeOutCurPlaceThenDestroy(float totalTime){
        if(_curPlace == null){
            Debug.LogWarning($"_curPlace 이 없음");
            return;
        }
        _curPlace.FadeOutAndDestroy(totalTime);
    }

    public static void InstantiateMovingBtnsUI(List<PlacePoint> placePoints){

        _placeUIPanel.InstantiateMovingBtns(placePoints);
    }
    public static void DestroyMovingBtnsUI(){

        _placeUIPanel.DestroyMovingBtns();
    }

}
