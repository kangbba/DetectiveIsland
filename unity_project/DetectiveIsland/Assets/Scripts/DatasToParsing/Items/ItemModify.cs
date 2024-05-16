using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemModify : Element
{

    public ItemModify(bool isGain, string id, int amount)
    {
        IsGain = isGain;
        ID = id;
        Amount = amount;
    }
    
    public bool IsGain;
    public string ID;
    public int Amount;
}
