using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue : Element
{
    private string _characterID;
    private List<Line> _lines;

    public Dialogue(string characterID, List<Line> lines)
    {
        this._characterID = characterID;
        this._lines = lines;
    }

    public string CharacterID { get => _characterID;}
    public List<Line> Lines { get => _lines; }
}