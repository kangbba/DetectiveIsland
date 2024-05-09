using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FriendshipModify : Element
{
    private bool _isGain;
    private string _characterID;
    private int _amount;

    public FriendshipModify(bool isGain, string id, int amount)
    {
        _isGain = isGain;
        _characterID = id;
        _amount = amount;
    }

    public bool IsGain { get => _isGain; set => _isGain = value; }
    public string Id { get => _characterID; set => _characterID = value; }
    public int Amount { get => _amount; set => _amount = value; }
}
