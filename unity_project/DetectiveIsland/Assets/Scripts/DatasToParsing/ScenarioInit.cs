using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioInit
{
    private List<PositionInit> _positionInits;

    public ScenarioInit(List<PositionInit> positionInit)
    {
        _positionInits = positionInit;
    }

    public List<PositionInit> PositionInits { get => _positionInits; }
}
