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

    
    private void Start()
    {
        Application.targetFrameRate = 300;
        Initialize();
        EventTime startingEventTime = EventService.GetFirstEventPlan().EventTime;
        EventTimeService.SetCurEventTime(startingEventTime);



        EventService.AllEventReset();

        EventProcessor.Move(EventService.GetFirstEventPlan().ScenarioDatas[0].PlaceID);
    }

    private void Initialize()
    {
        CameraController.Load();
        EventTimeService.Load();
        EventService.Load();
        ItemService.Load();
        ItemService.LoseAllItems();
        PlaceService.Load();
        CharacterService.Load();
        WorldManager.Load();
        PictureService.Load();
        UIManager.Load();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            DevelopmentTool.IsDebug = !DevelopmentTool.IsDebug;
        }
    }
}
