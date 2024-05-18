using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverlayPicture : Element
{

    private EPictureID _pictureID;
    private EPictureEffectID _effectID;
    private float _effectTime;

    public OverlayPicture(EPictureID pictureID, EPictureEffectID effectID, float effectTime)
    {
        _pictureID = pictureID;
        _effectID = effectID;
        _effectTime = effectTime;
    }

    public EPictureID PictureID { get => _pictureID; }
    public EPictureEffectID EffectID { get => _effectID; }
    public float EffectTime { get => _effectTime; }
}
