using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModify : Element
{
    private bool _isGain;
    private string _id;
    private int _amount;

    public ItemModify(bool isGain, string id, int amount)
    {
        _isGain = isGain;
        _id = id;
        _amount = amount;
    }
    
    public bool IsGain { get => _isGain; set => _isGain = value; }
    public string Id { get => _id; set => _id = value; }
    public int Amount { get => _amount; set => _amount = value; }
}
