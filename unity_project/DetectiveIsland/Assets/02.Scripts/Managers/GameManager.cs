using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Cysharp.Threading.Tasks;
using Aroka.Anim;
using UnityEngine.TextCore.Text;

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
        
        EventProcessor.MoveToPlace(EventService.GetFirstEventPlan().ScenarioDatas[0].PlaceID);
    }

    private void Initialize()
    {
        AudioController.Load();
        CameraController.Load();
        UIManager.Load();
        WorldManager.Load();
        //
        EventTimeService.Load();
        EventService.Load();
        ItemService.Load();
        PlaceService.Load();
        CharacterService.Load();
        PictureService.Load();
        AudioService.Load();
        CameraService.Load();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            DevelopmentTool.IsDebug = !DevelopmentTool.IsDebug;
        }
    }
}
