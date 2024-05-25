using UnityEngine;

[System.Serializable]
public class QuestData : Element
{
    [SerializeField] private QuestID _questID;

    public QuestID QuestID => _questID;

    public QuestData(QuestID questID)
    {
        _questID = questID;
    }
}
