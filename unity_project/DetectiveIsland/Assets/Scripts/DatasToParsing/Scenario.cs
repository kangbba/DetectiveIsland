using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scenario
{
    public Scenario(List<Element> elements)
    {
        this._elements = elements;
    }

    private ScenarioInit _scenarioInit;
    private List<Element> _elements = new List<Element>();

    public List<Element> Elements { get => _elements;  }
    public ScenarioInit ScenarioInit { get => _scenarioInit; }
}
