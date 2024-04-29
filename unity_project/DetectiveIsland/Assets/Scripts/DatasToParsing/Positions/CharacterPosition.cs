using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterPosition
{
    private string _characterID;
    private string _positionID;

    public CharacterPosition(string characterID, string positionID)
    {
        this._characterID = characterID;
        this._positionID = positionID;
    }

    public string CharacterID { get => _characterID; set => _characterID = value; }
    public string PositionID { get => _positionID; set => _positionID = value; }
}