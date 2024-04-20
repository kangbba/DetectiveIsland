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
        Move(EventService.GetFirstEventPlan().PlaceScenarios[0].PlaceID);
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
        Debug.Log($"----------------------------------------LOOP START----------------------------------------");
        PlaceData placeData = PlaceUIService.GetPlaceData(placeID);
        Debug.Log($"현재 시간 : {EventService.CurEventTime}");
        Debug.Log($"장소 이동 : {placeData.PlaceNameForUser} ({placeData.PlaceID})");
        
        // AwaitChoices의 결과를 직접 받아 처리
        SetPhase(EGamePhase.Enter);

        //Place UI판넬 퇴장
        CharacterService.DestroyAllCharacters(1f);
        PlaceUIService.SetOnPanel(false, false, false, .5f);
        ItemService.SetOnPanel(false, 0f);
        yield return new WaitForSeconds(.5f);


        SetPhase(EGamePhase.PlaceMoving); 

        //배경 세팅
        PlaceService.SetPlace(placeData);
        PlaceService.SetOnPanel(true, 1f);

        PlaceUIService.SetCurPlaceText(placeData.PlaceNameForUser);
        PlaceUIService.SetOnPanel(true, false, false, 1f);
        yield return new WaitForSeconds(1f);

        EventPlan eventPlan = EventService.GetEventPlan(EventService.CurEventTime);
        eventPlan.Initialize();
        EventService.LogEventPlan(eventPlan);

        PlaceScenario placeScenario = eventPlan.GetPlaceScenario(placeID);
        if(placeScenario != null){
            
            if (!placeScenario.IsViewed || !placeScenario.IsAllSolved()) {
                placeScenario.SetViewed(true);  
                SetPhase(EGamePhase.EventPlaying);
                Scenario scenario = ArokaJsonUtils.LoadScenario(placeScenario.ScenarioFile);
                yield return StartCoroutine(StoryProcessor.ScenarioRoutine(scenario));
            } 
            else {
                Debug.Log("한번 이상 열람된 이벤트");
            }
            // 이벤트 플랜의 나가는 조건이 모두 해결 되었는지 확인 후 시간 업데이트
            if (eventPlan.IsAllSolved()) {
                EventPlan curEventPlan = EventService.GetNextEventPlan(EventService.CurEventTime);
                if(curEventPlan != null){
                    EventTime nextEventTime = curEventPlan.EventTime;
                    EventService.SetCurEventTime(nextEventTime);
                }
                else{
                    Debug.LogWarning("게임 엔딩 출력!");
                    EventService.SetCurEventTime(new EventTime("2025-01-01", 09, 0));
                }
            }
        }

        CharacterService.DestroyAllCharacters(1f);
        //PlaceUI 판넬들 등장 및 이동가능버튼생성

        SetPhase(EGamePhase.Exit); 
        Debug.Log($"----------------------------------------LOOP END {placeID}----------------------------------------");
        yield return StartCoroutine(PlaceUIService.CreateAndShowPlaceBtns(placeID, Move));
        isMoving = false;
    }

    // Implement the SetPhase method
    private void SetPhase(EGamePhase newPhase)
    {
        _curPhase = newPhase;
    }
}
