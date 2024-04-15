using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private PlaceManager _placeManager;
    private ItemManager _itemManager;
    private CharacterManager _characterManager;

     private void Start()
    {
        Initialize();
        StartCoroutine(MoveToPlaceCoroutine("initialPlaceID"));
    }
    private void Initialize()
    {
        // UIManager로부터 UI 패널 GameObject 가져오기
        ArokaAnim placePanel = UIManager.Instance.PlacePanel;
        ArokaAnim itemPanel = UIManager.Instance.ItemPanel;
        ArokaAnim characterPanel = UIManager.Instance.CharacterPanel;

        // PlaceManager 초기화
        _placeManager = new PlaceManager();
        _placeManager.Initialize(folderName: "PlaceDatas", mainPanel: placePanel);

        // ItemManager 초기화
        _itemManager = new ItemManager();
        _itemManager.Initialize(folderName: "ItemDatas", mainPanel: itemPanel);
        
        // CharacterManager 초기화
        _characterManager = new CharacterManager();
        _characterManager.Initialize(folderName: "CharacterDatas", mainPanel: characterPanel);

        // 기타 Manager들의 초기화...
    }

    
     private IEnumerator MoveToPlaceCoroutine(string placeID)
    {
        // 해당 placeID에 해당하는 PlaceData 가져오기
        PlaceData placeData = _placeManager.GetPlaceData(placeID);
        if (placeData != null)
        {
            // 이동하는 로직 작성
            Debug.Log("Moving to place: " + placeData.PlaceID);
            yield return null; // 예시로 하나의 프레임을 대기
            Debug.Log("Arrived at place: " + placeData.PlaceID);
        }
        else
        {
            Debug.LogError("Cannot find place with ID: " + placeID);
        }
    }
    
}
