using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraAction : Element
{
    private ECameraActionID _cameraActionID;
    private readonly float _cameraActionTime;
    private readonly bool _waitForFinish;

    public ECameraActionID CameraActionID => _cameraActionID;

    public float CameraActionTime => _cameraActionTime;

    public bool WaitForFinish => _waitForFinish;

    public CameraAction(ECameraActionID cameraActionID, float cameraActionTime, bool waitForFinish)
    {
        _cameraActionID = cameraActionID;
        _cameraActionTime = cameraActionTime;
        _waitForFinish = waitForFinish;
    }
}
