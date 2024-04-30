using System;
using System.Collections;
using System.Collections.Generic;
using Aroka.JsonUtils;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Cysharp.Threading.Tasks;
using Aroka.Anim;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;


    private UIManager _uiManager;

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
        _uiManager = UIManager.Instance;
    }

    private void Start()
    {
        Application.targetFrameRate = 300;
        Initialize();
        EventTime startingEventTime = EventService.GetFirstEventPlan().EventTime;
        EventTimeService.SetCurEventTime(startingEventTime);
        EventTimeUIService.SetEventTime(startingEventTime);



        EventService.AllEventReset();

        EventProcessor.Move(EventService.GetFirstEventPlan().ScenarioDatas[0].PlaceID);
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
    }
    
    // Implement the SetPhase method
    private void SetPhase(EGamePhase newPhase)
    {
        _curPhase = newPhase;
    }
}
