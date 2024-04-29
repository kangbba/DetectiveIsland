using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInit : Element
{
    private List<CharacterPosition> _characterPositions;

    public PositionInit(List<CharacterPosition> characterPositions)
    {
        _characterPositions = characterPositions;
    }

    public List<CharacterPosition> CharacterPositions { get => _characterPositions; set => _characterPositions = value; }
}
