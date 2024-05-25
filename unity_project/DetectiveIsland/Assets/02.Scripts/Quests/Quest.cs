using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public enum QuestID
{
    HospitalDoorPassword
}

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed
}

public abstract class Quest : MonoBehaviour
{
    [SerializeField] private QuestID _questID;
    public QuestID QuestID => _questID;
    public QuestState State { get; private set; } = QuestState.NotStarted;
    public bool IsCompleted => State == QuestState.Completed;

    private Action _onComplete;

    public Quest(QuestID questID)
    {
        _questID = questID;
    }

    public virtual void StartQuest(Action onCompleteCallback)
    {
        _onComplete = onCompleteCallback;
        State = QuestState.InProgress;
    }

    protected void CompleteQuest()
    {
        State = QuestState.Completed;
        _onComplete?.Invoke();
    }

    public abstract UniTask WaitUntilComplete();
}
