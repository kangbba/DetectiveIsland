using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPosition : Element
{
    private List<CharacterPosition> _characterPositions;

    public ModifyPosition(List<CharacterPosition> characterPositions)
    {
        _characterPositions = characterPositions;
    }

    public List<CharacterPosition> CharacterPositions { get => _characterPositions; set => _characterPositions = value; }
}
