using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECameraActionID
{
    None = 0,
    ShakeNormal = 1,
    ShakeStrong = 2,
    GoLeftRight = 3,
}
public static class CameraService 
{
    public static void PlayCameraAction(CameraAction cameraAction)
    {
        switch (cameraAction.CameraActionID)
        {
            case ECameraActionID.ShakeNormal:
                // ShakeNormal 액션 실행
                CameraController.ShakeCamera(5f, cameraAction.CameraActionTime);
                break;
            case ECameraActionID.ShakeStrong:
                // ShakeStrong 액션 실행
                CameraController.ShakeCamera(10f, cameraAction.CameraActionTime);
                break;
            case ECameraActionID.GoLeftRight:
                // GoLeftRight 액션 실행
                break;
            default:
                // None 또는 정의되지 않은 액션 처리
                Debug.LogWarning("Undefined camera action.");
                break;
        }
    }
}
