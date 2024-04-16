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
        Initialize();
        EventService.SetCurEventTime(EventService.GetFirstEventPlan().EventTime);
        Move(EventService.GetFirstEventPlan().PlaceID);
    }

    private void Update(){
        CameraController.AdjustCamera();
    }

    private void Initialize()
    {
        CoroutineUtils.SetCoroutineExecutor(this);
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

       

        //이벤트가 있다면 실행
        EventPlan eventPlan = EventService.GetEventPlan(EventService.CurEventTime, placeID);
        if(eventPlan != null){
            //대화창 On
            DialogueService.SetOnPanel(true, 1f);
            yield return new WaitForSeconds(1f);
            
            yield return StartCoroutine(ProcessEventRoutine(eventPlan));
            //대화로인해 30초정도 흘렀다.
            EventService.AFewSecondsLater();
            Debug.Log(EventService.CurEventTime.ToString());
            //이제 남은 데일리이벤트는 몇개일까?
            Debug.Log($"총 {EventService.GetDailyEventPlans(EventService.CurEventTime.Date).Count} 개의 데일리이벤트가 있었습니다.)");
            Debug.Log($"방금 한개 해서 {EventService.GetPassedDailyEventPlans(EventService.CurEventTime).Count} 개의 데일리이벤트를 처리했습니다.)");
            Debug.Log($"");
            //대화창 Off
            DialogueService.SetOnPanel(false, 1f);
            yield return new WaitForSeconds(1f);
        }


        //자유행동

        
        //이동가능버튼생성
        PlaceUIService.CreatePlaceButtons();
        PlaceUIService.SetOnPanel(true, 1f);
        yield return new WaitForSeconds(1f);



        isMoving = false;
    }

    public IEnumerator ProcessEventRoutine(EventPlan eventPlan){
        Debug.Log("이벤트 실행이 돌입");
        TextAsset textAsset = eventPlan.ScenarioFile;
        Scenario scenario = ArokaJsonUtil.LoadScenario(eventPlan.ScenarioFile);
       
        List<Element> elements = scenario.Elements;
        foreach(Element element in elements){
            Debug.Log($"{elements.Count}중 {element.GetType()} 실행중");
            yield return ProcessElementRoutine(element);
        }
    }

    public IEnumerator ProcessElementRoutine(Element element){
        if(element is Dialogue){
            Dialogue dialogue = element as Dialogue;
            Debug.Log($"{dialogue.CharacterID}가 말한다");
            yield return StartCoroutine(DialogueService.DisplayTextRoutine(dialogue));
        }
        yield return null;
    }
}
