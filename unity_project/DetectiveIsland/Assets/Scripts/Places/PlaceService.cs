using System.Collections.Generic;
using UnityEngine;

public static class PlaceService
{
    private static GameObject _placeRoadmap;
    private static List<PlaceData> _placeDatas = new List<PlaceData>();
    public static List<PlaceData> PlaceDatas { get => _placeDatas; }
    private static PlacePanel _placePanel;
    private static PlaceData _curPlaceData;
    private const string PLACE_UNLOCKED_KEY = "place_unlocked_";  // 아이템 소유 정보의 키 접두어

    public static PlaceData CurPlaceData { get => _curPlaceData;  }

    public static void Load()
    {       
        _placePanel = UIManager.Instance.PlacePanel;
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
    public static void SetPlace(PlaceData placeData)
    {
        _curPlaceData = placeData;
        _placePanel.SetPlace(placeData);
    }

    public static void SetOnPanel(bool b, float totalTime){
        _placePanel.SetAnim(b, totalTime);
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

    public static List<PlaceData> GetUnlockedPlaceDatas()
    {
        List<PlaceData> placeDatasUnlocked = _placeDatas.FindAll(placeData => PlayerPrefs.GetInt(PLACE_UNLOCKED_KEY + placeData.PlaceID, 0) == 1);
        return placeDatasUnlocked ?? new List<PlaceData>();
    }
    
    public static bool IsUnlockedPlaceData(string placeID){
        
        PlaceData placeData = GetPlaceData(placeID);
        if (placeData != null){
            return PlayerPrefs.GetInt(PLACE_UNLOCKED_KEY + placeID) == 1;
        }
        else{
            Debug.LogError("해당 장소 아이디의 장소를 찾을수없음");
            return false;
        }
    }

    public static void UnlockPlace(string placeID, bool own)
    {
        // 특정 아이템의 소유 여부를 설정
        PlaceData placeData = GetPlaceData(placeID);
        if (placeData != null)
        {
            PlayerPrefs.SetInt(PLACE_UNLOCKED_KEY + placeID, own ? 1 : 0);
            PlayerPrefs.Save();  // 변경 사항을 즉시 저장
        }
    }
    
}
