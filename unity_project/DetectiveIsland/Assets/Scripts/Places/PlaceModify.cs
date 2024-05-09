using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaceModify : Element
{
    private bool _isGain;
    private string _id;

    public PlaceModify(bool isGain, string id)
    {
        _isGain = isGain;
        _id = id;
    }


    public bool IsGain { get => _isGain; set => _isGain = value; }
    public string Id { get => _id; set => _id = value; }
}
