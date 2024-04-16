using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlaceUIService
{
    private static GameObject _placeRoadmap;
    private static List<PlaceData> _placeDatas = new List<PlaceData>();
    private static PlaceUIPanel _placeUIPanelLeft; 
    private static PlaceUIPanel _placeUIPanelRight;
    private static List<PlaceButton> _curPlaceBtns = new List<PlaceButton>(); // List to store button components

    public static List<PlaceData> PlaceDatas { get => _placeDatas; }

    public static void Initialize()
    {
        _placeUIPanelLeft = UIManager.Instance.PlaceUIPanelLeft;
        _placeUIPanelRight = UIManager.Instance.PlaceUIPanelRight;

        GameObject _placeRoadmapPrefab = Resources.Load<GameObject>("PlaceRoadmapPrefab");
        _placeRoadmap = GameObject.Instantiate(_placeRoadmapPrefab); // Instantiate the roadmap
        if (_placeRoadmapPrefab != null)
        {
            TraverseChildren(_placeRoadmap.transform); // Traverse to find and store PlaceData
        }
        Debug.Log($"Loaded {_placeDatas.Count} place data from roadmap.");
    }

    public static void CreatePlaceButtons(){
        _curPlaceBtns.Clear(); // Clear existing buttons if reinitializing
        foreach (PlaceData placeData in _placeDatas)
        {
            PlaceButton buttonLeft = GameObject.Instantiate(Resources.Load<PlaceButton>("PlaceButtonPrefab"), _placeUIPanelLeft.transform);
            buttonLeft.Initialize(placeData);
            _curPlaceBtns.Add(buttonLeft);

            PlaceButton buttonRight = GameObject.Instantiate(Resources.Load<PlaceButton>("PlaceButtonPrefab"), _placeUIPanelRight.transform);
            buttonRight.Initialize(placeData);
            _curPlaceBtns.Add(buttonRight);
        }
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
    public static void SetOnPanel(bool b, float totalTime)
    {
        _placeUIPanelLeft.SetAnim(b, totalTime);
        _placeUIPanelRight.SetAnim(b, totalTime);
    }
}

