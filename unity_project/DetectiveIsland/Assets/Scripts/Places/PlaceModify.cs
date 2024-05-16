using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaceModify : Element
{

    public PlaceModify(bool isGain, string id)
    {
        IsGain = isGain;
        ID = id;
    }


    public bool IsGain;
    public string ID;
}
