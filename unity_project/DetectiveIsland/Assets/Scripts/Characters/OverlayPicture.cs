using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverlayPicture : Element
{

    private string _pictureID;
    private string _effectID;
    private float _effectTime;

    public OverlayPicture(string pictureID, string effectID, float effectTime)
    {
        _pictureID = pictureID;
        _effectID = effectID;
        _effectTime = effectTime;
    }

    public string PictureID { get => _pictureID; set => _pictureID = value; }
    public string EffectID { get => _effectID; set => _effectID = value; }
    public float EffectTime { get => _effectTime; set => _effectTime = value; }
}
