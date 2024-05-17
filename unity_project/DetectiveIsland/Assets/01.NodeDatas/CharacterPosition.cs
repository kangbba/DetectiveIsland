using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ECharacterPositionID{
    Middle = 0,
    Left = 1,
    Right = 2
}
[System.Serializable]
public class CharacterPosition
{
    private ECharacterID _characterID;
    private ECharacterPositionID _positionID;

    public CharacterPosition(ECharacterID characterID, ECharacterPositionID positionID)
    {
        this._characterID = characterID;
        this._positionID = positionID;
    }

    public ECharacterID CharacterID { get => _characterID; set => _characterID = value; }
    public ECharacterPositionID PositionID { get => _positionID; set => _positionID = value; }
}