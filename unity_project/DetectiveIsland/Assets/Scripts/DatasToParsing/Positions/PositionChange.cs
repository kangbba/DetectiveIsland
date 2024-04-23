using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionChange : Element
{
    private string _characterID;
    private string _positionID;

    public PositionChange(string characterID, string positionID)
    {
        this._characterID = characterID;
        this._positionID = positionID;
    }

    public string CharacterID { get => _characterID;  }
    public string PositionID { get => _positionID;  }
}