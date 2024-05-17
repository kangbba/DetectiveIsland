using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue : Element
{
    private ECharacterID _characterID;
    private List<Line> _lines;

    public Dialogue(ECharacterID characterID, List<Line> lines)
    {
        this._characterID = characterID;
        this._lines = lines;
    }

    public ECharacterID CharacterID { get => _characterID; set => _characterID = value; }
    public List<Line> Lines { get => _lines; set => _lines = value; }
}