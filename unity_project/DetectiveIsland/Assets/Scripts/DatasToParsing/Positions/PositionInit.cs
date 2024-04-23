using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInit
{
    private string _characterID;
    private string _positionID;

    public PositionInit(string characterID, string positionID)
    {
        this._characterID = characterID;
        this._positionID = positionID;
    }

    public string CharacterID { get => _characterID;  }
    public string PositionID { get => _positionID;  }
}
