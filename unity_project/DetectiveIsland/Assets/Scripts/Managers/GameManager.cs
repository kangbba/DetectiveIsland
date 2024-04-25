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
        Application.targetFrameRate = 300;
        Initialize();
        EventTime startingEventTime = EventService.GetFirstEventPlan().EventTime;
        EventTimeService.SetCurEventTime(startingEventTime);
        EventTimeUIService.SetEventTime(startingEventTime);



        EventService.AllEventResetViewed();

        Move(EventService.GetFirstEventPlan().ScenarioDatas[0].PlaceID);
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
        ItemUIService.Load();
        PlaceService.Load();
        PlaceUIService.Load();
        CharacterService.Load();
        ChoiceSetService.Load();
        EventTimeService.Load();
        EventTimeUIService.Load();
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

    IEnumerator QuestRoutine(){
        EventPlan eventPlan = EventService.GetEventPlan(EventTimeService.CurEventTime);
        yield return new WaitUntil(() => eventPlan.IsAllSolved());
    }
    private IEnumerator MoveToPlaceCoroutine(string placeID)
    {
        Debug.Log($"----------------------------------------LOOP START----------------------------------------");
        PlaceData placeData = PlaceUIService.GetPlaceData(placeID);
        Debug.Log($"현재 시간 : {EventTimeService.CurEventTime.ToString()}");
        Debug.Log($"장소 이동 : {placeData.PlaceNameForUser} ({placeData.PlaceID})");
        
        // AwaitChoices의 결과를 직접 받아 처리
        SetPhase(EGamePhase.Enter);

        //Place UI판넬 퇴장
        PlaceUIService.SetOnPanel(false, false, false, .5f);
        ItemUIService.HideItemCheckPanelEnterButton();
        yield return new WaitForSeconds(.5f);


        SetPhase(EGamePhase.PlaceMoving); 

        //배경 세팅
        PlaceService.SetPlace(placeData);
        PlaceService.SetOnPanel(true, 1f);

        PlaceUIService.SetCurPlaceText(placeData.PlaceNameForUser);
        PlaceUIService.SetOnPanel(true, false, false, .5f);
        yield return new WaitForSeconds(.5f);

        EventPlan eventPlan = EventService.GetEventPlan(EventTimeService.CurEventTime);
        ScenarioData scenarioData = eventPlan.GetScenarioData(placeID);
        if(eventPlan != null && scenarioData != null){
            Debug.Log("단순한 후 시나리오가 있음");
            ItemUIService.HideItemCheckPanelEnterButton();
            if(!scenarioData.IsAllSolved()){
                Scenario scenario = ArokaJsonUtils.LoadScenario(scenarioData.ScenarioFile);
                if(scenarioData.IsViewed){
                    Debug.Log($"봤던 시나리오이고 해결이 {scenarioData.IsAllSolved()}, 말걸기 필요");
                    EventProcessor.PositionInits(CharacterService.GetLastPosition(scenario));
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                    SetPhase(EGamePhase.EventPlaying);
                    yield return StartCoroutine(EventProcessor.ScenarioRoutine(scenario));
                }
                else{
                    Debug.Log("처음 마주친 시나리오");
                    SetPhase(EGamePhase.EventPlaying);
                    yield return StartCoroutine(EventProcessor.ScenarioRoutine(scenario));
                }
                scenarioData.SetViewed(true);  
            }
            if(eventPlan.IsAllSolved()){
                EventPlan curEventPlan = EventService.GetNextEventPlan(EventTimeService.CurEventTime);
                if(curEventPlan != null){
                    EventTime nextEventTime = curEventPlan.EventTime;
                    EventTimeService.SetCurEventTime(nextEventTime);
                    EventTimeUIService.SetEventTime(nextEventTime);
                }
                else{
                    Debug.LogWarning("게임 엔딩 출력!");
                    EventTimeService.SetCurEventTime(new EventTime("2025-01-01", 09, 0));
                }
                CharacterService.DestoryAllCharacters();
                SetPhase(EGamePhase.Exit); 
                ItemUIService.ShowItemCheckPanelEnterButton();
            }
        }
        else{
            Debug.Log("단순한 장소이동 호출");
        }
        isMoving = false;
        yield return StartCoroutine(PlaceUIService.CreateAndShowPlaceBtns(placeID, Move));
        Debug.Log($"----------------------------------------LOOP END {placeID}----------------------------------------");
    }

    // Implement the SetPhase method
    private void SetPhase(EGamePhase newPhase)
    {
        _curPhase = newPhase;
    }
}
