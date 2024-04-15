using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private EventService _eventService;
    private DialogueService _dialogueService;
    private ItemService _itemService;
    private PlaceService _placeService;
    private CharacterService _characterService;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        Initialize();
        _eventService.SetCurEventTime(new EventTime("2024-04-01", 9 , 0));
        StartCoroutine(MoveToPlaceCoroutine("cafe_seabreeze"));
    }

    private void Initialize()
    {

        _eventService = new EventService();
        _eventService.Initialize();

        _dialogueService = new DialogueService();
        _dialogueService.Initialize();

        // ItemService 초기화
        _itemService = new ItemService();
        _itemService.Initialize();

        // PlaceService 초기화
        _placeService = new PlaceService();
        _placeService.Initialize();

        // CharacterService 초기화
        _characterService = new CharacterService();
        _characterService.Initialize();

    }
    
    private IEnumerator MoveToPlaceCoroutine(string placeID)
    {
        // 해당 placeID에 해당하는 PlaceData 가져오기
        PlaceData placeData = _placeService.GetPlaceData(placeID);
        if (placeData != null)
        {
            // 이동하는 로직 작성
            Debug.Log("Moving to place: " + placeData.PlaceID);
            _placeService.SetPlace(placeData);
            _placeService.SetOnPanel(true, 1f);
            yield return new WaitForSeconds(1f);

            Debug.Log("Arrived at place: " + placeData.PlaceID);
            _dialogueService.SetOnPanel(true, 1f);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            Debug.LogError("Cannot find place with ID: " + placeID);
        }
    }
}
