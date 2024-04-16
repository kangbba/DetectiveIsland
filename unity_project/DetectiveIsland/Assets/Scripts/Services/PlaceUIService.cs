using System;
using System.Collections;
using System.Collections.Generic;
using ArokaUtil;
using UnityEditor.Build;
using UnityEngine;

public static class PlaceUIService
{
    private static GameObject _placeRoadmap;
    private static List<PlaceData> _placeDatas = new List<PlaceData>();
    private static PlaceUIPanel _placeUIPanel; 

    public static List<PlaceData> PlaceDatas { get => _placeDatas; }

    public static void Initialize()
    {
        _placeUIPanel = UIManager.Instance.PlaceUIPanel;

        GameObject _placeRoadmapPrefab = Resources.Load<GameObject>("PlaceRoadmapPrefab");
        _placeRoadmap = GameObject.Instantiate(_placeRoadmapPrefab); // Instantiate the roadmap
        TraverseChildren(_placeRoadmap.transform); // Traverse to find and store PlaceData
    }

    private static void TraverseChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            PlaceData placeData = child.GetComponent<PlaceData>();
            if (placeData != null)
            {
                _placeDatas.Add(placeData);
            }
            TraverseChildren(child); // Recurse into each child
        }
    }
    public static GameObject GetPlaceDataGameObject(string placeID)
    {
        foreach (PlaceData placeData in _placeDatas)
        {
            if (placeData.PlaceID == placeID)
            {
                return placeData.gameObject;  // PlaceData 컴포넌트가 붙어 있는 GameObject 반환
            }
        }
        Debug.LogError($"No GameObject found with PlaceID: {placeID}");
        return null;
    }

    public static PlaceData GetPlaceData(string placeID)
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
    public static void SetOnPanel(bool up, bool right, bool left, float totalTime)
    {   
        _placeUIPanel.SetOnPanel(up, left, right, totalTime);
    }

    public static void SetInteractablePlaceButtons(bool b)
    {
        _placeUIPanel.SetInteractablePlaceButtons(b);
    }

    public static void SetCurPlaceText(string selectedPlaceID){
        _placeUIPanel.SetCurPlaceText(selectedPlaceID);
    }
    public static void CreatePlaceButtons(string selectedPlaceID, Action<string> moveAction){
        _placeUIPanel.CreatePlaceButtons(selectedPlaceID, moveAction);
    }

}

