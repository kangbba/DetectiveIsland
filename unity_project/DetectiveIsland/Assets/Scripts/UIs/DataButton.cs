using UnityEngine;
using UnityEngine.EventSystems;
using System;
using Aroka.EaseUtils;

public abstract class DataButton : MonoBehaviour
{
    [SerializeField] protected EventTrigger _eventTrigger; // 이벤트 트리거 컴포넌트

    public string BtnKey { get; protected set; }

    public void Initialize(string btnKey)
    {
        BtnKey = btnKey;
        if (_eventTrigger == null)
        {
            _eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
    }

    public void ConnectOnClick(Action<string> action)
    {
        EventTrigger.Entry clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        clickEntry.callback.AddListener((eventData) => {
            action(BtnKey);
        });
        _eventTrigger.triggers.Add(clickEntry);
    }

    public void ConnectOnHover(Action<string> onEnterAction, Action<string> onExitAction)
    {
        // 마우스 오버 이벤트 추가 (PointerEnter)
        EventTrigger.Entry enterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        enterEntry.callback.AddListener((eventData) => {
            onEnterAction?.Invoke(BtnKey);
        });
        _eventTrigger.triggers.Add(enterEntry);

        // 마우스 오버 이벤트 제거 (PointerExit)
        EventTrigger.Entry exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        exitEntry.callback.AddListener((eventData) => {
            onExitAction?.Invoke(BtnKey);
        });
        _eventTrigger.triggers.Add(exitEntry);
    }

    protected void OnDestroy()
    {
        if (_eventTrigger != null)
        {
            _eventTrigger.triggers.Clear();
        }
    }

    public void SetInteractable(bool interactable)
    {
        // EventTrigger는 interactable에 따라 활성화 또는 비활성화됩니다
        _eventTrigger.enabled = interactable;
    }
}
