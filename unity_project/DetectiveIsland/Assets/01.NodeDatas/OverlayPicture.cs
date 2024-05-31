using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverlayPicture : Element
{
    private EPictureID _pictureID;
    private EPictureEffectID _effectID;
    private bool _isPreset;
    private float _pictureTime;


    public OverlayPicture(EPictureID pictureID, EPictureEffectID effectID, bool isPreset, float effectTime){
        _pictureID = pictureID;
        _effectID = effectID;
        _isPreset = isPreset;
        _pictureTime = effectTime;
    }

    public EPictureID PictureID { get => _pictureID; }
    public EPictureEffectID EffectID { get => _effectID; }
    public bool IsPreset { get => _isPreset; }
    public float EffectTime { get => _pictureTime; }
}
