using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDemand : Element
{
    private string _itemID;
    private List<Dialogue> _dialogues;
    private List<Element> _successElements;
    private List<Element> _failElements;

    public ItemDemand(string itemID, List<Dialogue> dialogues, List<Element> successElements, List<Element> failElements)
    {
        this._itemID = itemID;
        this._dialogues = dialogues;
        this._successElements = successElements;
        this._failElements = failElements;
    }

    public List<Element> FailElements { get => _failElements; set => _failElements = value; }
    public List<Element> SuccessElements { get => _successElements; set => _successElements = value; }
    public string ItemID { get => _itemID; set => _itemID = value; }
    public List<Dialogue> Dialogues { get => _dialogues; set => _dialogues = value; }
}