using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssetChange : Element
{
    private string _gainType;
    private string _itemID;
    private uint _itemAmount;

    public AssetChange(string gainType, string itemID, uint itemAmount)
    {
        this._gainType = gainType;
        this._itemID = itemID;
        this._itemAmount = itemAmount;
    }

    public string GainType { get => _gainType; set => _gainType = value; }
    public string ItemID { get => _itemID; set => _itemID = value; }
    public uint ItemAmount { get => _itemAmount; set => _itemAmount = value; }
}
