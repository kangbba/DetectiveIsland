using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FriendshipModify : Element
{

    public FriendshipModify(bool isGain, string id, int amount)
    {
        IsGain = isGain;
        CharacterID = id;
        Amount = amount;
    }

    public bool IsGain;
    public string CharacterID;
    public int Amount;
}
