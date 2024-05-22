using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GainPlace : Element
{
    private bool _isGain;
    private EPlaceID _id;

    public GainPlace(bool isGain, EPlaceID id)
    {
        _isGain = isGain;
        _id = id;
    }

    public EPlaceID ID { get => _id; }
    public bool IsGain { get => _isGain; }
}
