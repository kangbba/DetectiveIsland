using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scenario
{
    public Scenario(List<PositionChange> _positionChanges, List<Element> elements )
    {
        this._elements = elements;
    }

    private List<Element> _elements = new List<Element>();

    public List<Element> Elements { get => _elements;  }
}
