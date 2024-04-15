using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDemand : Element
{
    public string itemID;
    public List<Dialogue> dialogues;
    public List<Element> successElements;
    public List<Element> failElements;
}
