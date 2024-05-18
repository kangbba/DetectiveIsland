using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDemand : Element
{
    private EItemID _itemID;
    private List<Dialogue> _dialogues;
    private List<Element> _successElements;
    private List<Element> _failElements;
    private List<Element> _cancelElements;

    public ItemDemand(EItemID itemID, List<Dialogue> dialogues, List<Element> successElements, List<Element> failElements, List<Element> cancelElements)
    {
        this._itemID = itemID;
        this._dialogues = dialogues;
        this._successElements = successElements;
        this._failElements = failElements;
        this._cancelElements = cancelElements;
    }

    public EItemID ItemID { get => _itemID; set => _itemID = value; }
    public List<Dialogue> Dialogues { get => _dialogues; set => _dialogues = value; }
    public List<Element> FailElements { get => _failElements; set => _failElements = value; }
    public List<Element> SuccessElements { get => _successElements; set => _successElements = value; }
    public List<Element> CancelElements { get => _cancelElements; set => _cancelElements = value; }
}