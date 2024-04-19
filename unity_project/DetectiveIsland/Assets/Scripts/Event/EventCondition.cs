using System;
using UnityEngine;


public enum EActionType
{
    AutoPlay,
    Talk,    // 캐릭터와 대화
    Collect  // 아이템 수집
}

[System.Serializable]
public class EventCondition
{
    [SerializeField] private EActionType _actionType; // 행동 유형
    
    [SerializeField] private string _targetID; // 대상 ID, 예: 캐릭터 이름, 아이템 ID

    public EventCondition(EActionType actionType, string targetID)
    {
        _actionType = actionType;
        _targetID = targetID;
    }

    public EActionType ActionType => _actionType;
    public string TargetID => _targetID;
}

