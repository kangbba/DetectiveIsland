using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scenario
{
    private List<Element> _elements = new List<Element>();

    public Scenario(List<Element> elements)
    {
        this._elements = elements;
    }

    public List<Element> Elements { get => _elements;  }
}
