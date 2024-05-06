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
}
