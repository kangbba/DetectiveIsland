using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraAction : Element
{
    private ECameraActionID _cameraActionID;
    private readonly float _cameraActionTime;

    public ECameraActionID CameraActionID => _cameraActionID;

    public float CameraActionTime => _cameraActionTime;

    public CameraAction(ECameraActionID cameraActionID, float cameraActionTime)
    {
        _cameraActionID = cameraActionID;
        _cameraActionTime = cameraActionTime;
    }
}
