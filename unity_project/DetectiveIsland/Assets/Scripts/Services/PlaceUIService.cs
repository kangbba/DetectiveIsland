using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlaceUIService
{
    private static GameObject _placeRoadmap;
    private static List<PlaceData> _placeDatas;
    private static PlaceUIPanel _placeUIPanelLeft; 
    private static PlaceUIPanel _placeUIPanelRight;
    private static List<PlaceButton> _curPlaceBtns = new List<PlaceButton>(); // List to store button components

    public static void Initialize()
    {
        _placeUIPanelLeft = UIManager.Instance.PlaceUIPanelLeft;
        _placeUIPanelRight = UIManager.Instance.PlaceUIPanelRight;
        GameObject _placeRoadmapPrefab = Resources.Load<GameObject>("PlaceRoadmapPrefab");

        if (_placeRoadmapPrefab != null)
        {
            _placeRoadmap = GameObject.Instantiate(_placeRoadmapPrefab); // Instantiate the roadmap
            TraverseChildren(_placeRoadmap.transform); // Traverse to find and store PlaceData
            GameObject.Destroy(_placeRoadmap); // Optionally destroy the instance after extracting data
        }
        else
        {
            Debug.LogError("Failed to load PlaceRoadmapPrefab. Please check the path and prefab existence.");
        }

        CreatePlaceButtons(); // Create buttons based on extracted PlaceData
        Debug.Log($"Loaded {_placeDatas.Count} place data from roadmap.");
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
    private static void CreatePlaceButtons()
    {
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

