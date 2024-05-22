using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GainFriendship : Element
{
    
    private bool _isGain;
    private ECharacterID _id;
    private int _amount;
    public GainFriendship(bool isGain, ECharacterID id, int amount)
    {
        _isGain = isGain;
        _id = id;
        _amount = amount;
    }

    public bool IsGain { get => _isGain; }
    public ECharacterID ID { get => _id; }
    public int Amount { get => _amount; }
}
