using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GainFriendship : Element
{

    public GainFriendship(bool isGain, string id, int amount)
    {
        IsGain = isGain;
        CharacterID = id;
        Amount = amount;
    }

    public bool IsGain;
    public string CharacterID;
    public int Amount;
}
