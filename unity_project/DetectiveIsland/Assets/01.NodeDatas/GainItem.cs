using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GainItem : Element
{
    public GainItem(bool isGain, EItemID id, int amount)
    {
        IsGain = isGain;
        ID = id;
        Amount = amount;
    }
    
    public bool IsGain;
    public EItemID ID;
    public int Amount;
}
