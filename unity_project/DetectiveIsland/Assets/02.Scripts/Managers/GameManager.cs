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
        InitializeAsync().Forget();
    }

    private async UniTaskVoid InitializeAsync()
    {
        Initialize();
        await UniTask.Yield(); // 모든 초기화 작업이 완료될 때까지 프레임 끝까지 기다림

        EventPlanManager.SetCurEventTime(initialEventTimeForTest);
        PlaceService.MoveToPlace(initialPlaceID, 0);
    }

    private void Initialize()
    {
        CameraController.Load();
        UIManager.Load();

        AudioController.Load();
        WorldManager.Load();
        EventService.Load();
        ItemService.Load();
        PlaceService.Load();
        EventService.Load();
        EventPlanManager.Load();
        CharacterService.Load();
        PictureService.Load(UIManager.UIParent.OverlayPicturePanel);
        AudioService.Load();
        CameraActionService.Load();
        QuestManager.Load();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            DevelopmentTool.IsDebug = !DevelopmentTool.IsDebug;
        }
    }
}
