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

    [SerializeField] private EventTime initialEventTimeForTest;
    [SerializeField] private EPlaceID initialPlaceID;

    
    private void Start()
    {
        Application.targetFrameRate = 300;
        Initialize();
    }

    private void Initialize()
    {
        AudioController.Load();
        CameraController.Load();
        UIManager.Load();
        WorldManager.Load();
        EventService.Load();
        ItemService.Load();
        PlaceService.Load();
        EventService.Load();
        EventTimeService.Load();
        CharacterService.Load();
        PictureService.Load();
        AudioService.Load();
        CameraService.Load();
        QuestManager.Load();

        EventTimeService.SetCurEventTime(initialEventTimeForTest);
        PlaceService.MoveToPlace(initialPlaceID, 0);

    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            DevelopmentTool.IsDebug = !DevelopmentTool.IsDebug;
        }
    }
}
