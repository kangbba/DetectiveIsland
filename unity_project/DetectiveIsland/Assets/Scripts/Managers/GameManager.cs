using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private bool isMoving = false;

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
        CoroutineUtils.SetCoroutineExecutor(this);
        Initialize();
        EventService.SetCurEventTime(new EventTime("2024-04-01", 9 , 0));
        StartCoroutine(MoveToPlaceCoroutine("cafe_seabreeze"));
    }

    private void Update(){
        CameraController.AdjustCamera();
    }

    private void Initialize()
    {
        EventService.Initialize();

        DialogueService.Initialize();

        ItemService.Initialize();

        PlaceService.Initialize();
        
        PlaceUIService.Initialize();

        CharacterService.Initialize();
 
    }
    
    public void Move(string placeID){
        // 해당 placeID에 해당하는 PlaceData 가져오기
        PlaceData placeData = PlaceUIService.GetPlaceData(placeID);
        if (placeData == null)
        {
            Debug.LogError($"Cannot find place with ID: {placeID}");
            return;
        }
        if(isMoving){
            Debug.LogError("이미 장소 이동중입니다");
            return;
        }
        isMoving = true;
        StartCoroutine(MoveToPlaceCoroutine(placeID));
        // 이동하는 로직 작성
    }
    private IEnumerator MoveToPlaceCoroutine(string placeID)
    {
        PlaceData placeData = PlaceUIService.GetPlaceData(placeID);
        Debug.Log($"Arrived at place: {placeData.PlaceNameForUser} ({placeData.PlaceID})");

        //배경 세팅
        PlaceService.SetPlace(placeData);
        PlaceService.SetOnPanel(true, 1f);
        yield return new WaitForSeconds(1f);

        //대화창 On
        DialogueService.SetOnPanel(true, 1f);
        yield return new WaitForSeconds(1f);

        //이벤트가 있다면 실행
        EventPlan eventPlan = EventService.GetEventPlan(EventService.CurEventTime, placeID);
        if(eventPlan != null){
            yield return StartCoroutine(ProcessEventRoutine(eventPlan));
        }

        //자유행동
        

        //대화창 Off
        DialogueService.SetOnPanel(false, 1f);
        yield return new WaitForSeconds(1f);


        isMoving = false;
    }

    public IEnumerator ProcessEventRoutine(EventPlan eventPlan){

        TextAsset textAsset = eventPlan.ScenarioFile;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }
}
