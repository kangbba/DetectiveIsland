using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverlayPicture : Element
{
    private string _effectID;

    private string _pictureID;

    public OverlayPicture(string effectID, string pictureID)
    {
        _effectID = effectID;
        _pictureID = pictureID;
    }

    public string EffectID { get => _effectID; set => _effectID = value; }
    public string PictureID { get => _pictureID; set => _pictureID = value; }
}
