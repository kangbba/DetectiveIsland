using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CameraActionID
{
    None,
    ShakeNormal,
    ShakeStrong,
    GoLeftRight,
}
public class CameraAction : Element
{
    private readonly string _cameraActionID;
    private readonly float _cameraActionTime;

    public string CameraActionID => _cameraActionID;

    public float CameraActionTime => _cameraActionTime;

    public CameraAction(string cameraActionID, float cameraActionTime)
    {
        _cameraActionID = cameraActionID;
        _cameraActionTime = cameraActionTime;
    }
}
