using System;
using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using Aroka.JsonUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private bool isMoving = false;

    public enum EGamePhase
    {
        Enter,
        ConditionWaiting,  
        PlaceMoving,        
        EventPlaying,
        FreeActing,
        Exit,
    }

    // Current phase state
    private EGamePhase _curPhase;

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
        EventService.Load();
        DialogueService.Load();
        ItemService.Load();
        ItemService.LoseAllItems();
        PlaceService.Load();
        PlaceUIService.Load();
        CharacterService.Load();
        ChoiceSetService.Load();
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
            Debug.LogWarning("이미 장소 이동중입니다");
            return;
        }
        isMoving = true;
        StartCoroutine(MoveToPlaceCoroutine(placeID));
        // 이동하는 로직 작성
    }
    private IEnumerator MoveToPlaceCoroutine(string placeID)
    {
        // AwaitChoices의 결과를 직접 받아 처리
        SetPhase(EGamePhase.Enter);

        //Place UI판넬 퇴장
        CharacterService.DestroyAllCharacters(1f);
        PlaceUIService.SetOnPanel(false, false, false, .5f);
        ItemService.SetOnPanel(false, 0f);
        yield return new WaitForSeconds(.5f);


        SetPhase(EGamePhase.PlaceMoving); 
        PlaceData placeData = PlaceUIService.GetPlaceData(placeID);
        Debug.Log($"Arrived at place: {placeData.PlaceNameForUser} ({placeData.PlaceID})");

        //배경 세팅
        PlaceService.SetPlace(placeData);
        PlaceService.SetOnPanel(true, 1f);

        PlaceUIService.SetCurPlaceText(placeData.PlaceNameForUser);
        PlaceUIService.SetOnPanel(true, false, false, 1f);
        yield return new WaitForSeconds(1f);

        EventPlan eventPlan = EventService.GetEventPlan(EventService.CurEventTime, placeID);
        
        //이벤트가 있다면 
        if(eventPlan != null){
            SetPhase(EGamePhase.EventPlaying); 
            TextAsset scenarioFile = eventPlan.ScenarioFile;
            Scenario scenario = ArokaJsonUtils.LoadScenario(scenarioFile);

            yield return StartCoroutine(EventProcessor.InitializeScenarioRoutine(scenario));
            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(EventProcessor.ConditionCheckRoutine(eventPlan.EventEnterCondition));
            yield return StartCoroutine(EventProcessor.ScenarioRoutine(scenario));

            //이벤트 완료!
            var remainedEvents = EventService.GetEventPlansByDate(EventService.CurEventTime.Date).EventTimeFilter(EventService.CurEventTime, TimeRelation.Future);
            foreach (var plan in remainedEvents)
            {
                Debug.Log($"오늘 남은 이벤트 ID: {plan.PlaceID}, 시간: {plan.EventTime}");
            }
            if(EventProcessor.TimeProcessConditionCheck(eventPlan.TimeProcessCondition)){
                EventService.SetCurEventTime(remainedEvents[0].EventTime);
            }
            else{
                
            }
        }
        CharacterService.DestroyAllCharacters(1f);
        SetPhase(EGamePhase.FreeActing); 
        //PlaceUI 판넬들 등장 및 이동가능버튼생성
        yield return StartCoroutine(PlaceUIService.CreateAndShowPlaceBtns(placeID, Move));
        isMoving = false;

        SetPhase(EGamePhase.Exit); 
    }

    // Implement the SetPhase method
    private void SetPhase(EGamePhase newPhase)
    {
        _curPhase = newPhase;
        Debug.Log($"Game phase set to: {_curPhase}");
    }
}
