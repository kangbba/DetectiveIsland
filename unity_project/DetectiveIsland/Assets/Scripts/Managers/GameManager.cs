using System;
using System.Collections;
using System.Collections.Generic;
using Aroka.JsonUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Cysharp.Threading.Tasks;

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
        CameraController.Load();
        EventTimeService.Load();
        EventTimeUIService.Load();
        EventService.Load();
        
        ItemService.Load();
        ItemService.LoseAllItems();
        PlaceService.Load();
        CharacterService.Load();


        DialogueUI.Load();
        ChoiceSetUI.Load();
        ItemUI.Load();
        PlaceUIService.Load();
    }
    
    public void Move(string placeID){
        // 해당 placeID에 해당하는 PlaceData 가져오기
        PlaceData placeData = PlaceService.GetPlaceData(placeID);
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
        MoveToPlaceUniTask(placeID);
        // 이동하는 로직 작성
    }

    async UniTask QuestTask(){
        EventPlan eventPlan = EventService.GetEventPlan(EventTimeService.CurEventTime);
        await UniTask.WaitUntil(() => eventPlan.IsAllSolved());
    }
    private async UniTask MoveToPlaceUniTask(string placeID)
    {
        Debug.Log($"----------------------------------------LOOP START----------------------------------------");
        PlaceData placeData = PlaceService.GetPlaceData(placeID);
        Debug.Log($"현재 시간 : {EventTimeService.CurEventTime.ToString()}");
        Debug.Log($"장소 이동 : {placeData.PlaceNameForUser} ({placeData.PlaceID})");
        
        // AwaitChoices의 결과를 직접 받아 처리
        SetPhase(EGamePhase.Enter);

        //Place UI판넬 퇴장
        PlaceUIService.SetOnPanel(false, false, false, 500f);
        ItemUI.HideItemCheckPanelEnterButton();
        await UniTask.WaitForSeconds(.5f);


        SetPhase(EGamePhase.PlaceMoving); 

        //배경 세팅
        PlaceService.SetPlace(placeData);

        PlaceUIService.SetCurPlaceText(placeData.PlaceNameForUser);
        PlaceUIService.SetOnPanel(true, false, false, 500);
        await UniTask.WaitForSeconds(.5f);

        EventPlan eventPlan = EventService.GetEventPlan(EventTimeService.CurEventTime);
        ScenarioData scenarioData = eventPlan.GetScenarioData(placeID);
        if(eventPlan != null && scenarioData != null){
            Debug.Log("단순한 후 시나리오가 있음");
            ItemUI.HideItemCheckPanelEnterButton();
            if(!scenarioData.IsAllSolved()){
                Scenario scenario = ArokaJsonUtils.LoadScenario(scenarioData.ScenarioFile);
                if(scenarioData.IsViewed){
                    Debug.Log($"봤던 시나리오이고 해결이 {scenarioData.IsAllSolved()}, 말걸기 필요");
                    await EventProcessor.ProcessPositionInit(EventProcessor.GetLastPositionInit(scenario));
                    await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
                    SetPhase(EGamePhase.EventPlaying);
                    await EventProcessor.ScenarioTask(scenario);
                }
                else{
                    Debug.Log("처음 마주친 시나리오");
                    SetPhase(EGamePhase.EventPlaying);
                    await EventProcessor.ScenarioTask(scenario);
                }
                scenarioData.SetViewed(true);  
                CharacterService.AllCharacterFadeOutAndDestroy(1f);
                await UniTask.WaitForSeconds(1f);
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
                SetPhase(EGamePhase.Exit); 
                ItemUI.ShowItemCheckPanelEnterButton();
            }
        }
        else{
            Debug.Log("단순한 장소이동 호출");
        }
        isMoving = false;
        await PlaceUIService.CreateAndShowPlaceBtns(placeID, Move);
        Debug.Log($"----------------------------------------LOOP END {placeID}----------------------------------------");
    }

    // Implement the SetPhase method
    private void SetPhase(EGamePhase newPhase)
    {
        _curPhase = newPhase;
    }
}
