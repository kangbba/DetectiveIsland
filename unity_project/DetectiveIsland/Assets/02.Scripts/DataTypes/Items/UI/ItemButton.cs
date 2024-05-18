using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private Image _previewImg;
    [SerializeField] private Image _cursoredImg;
    private ItemData _itemData;
    private Action<EItemID> _action;

    public ItemData ItemData { get => _itemData; }

    public void Initialize(ItemData itemData, Action<EItemID> onClick)
    {
        _itemData = itemData;
        _action = onClick;

        if (_itemData.ItemSprite != null)
        {
            _previewImg.sprite = _itemData.ItemSprite;
        }

        SetSelectedImg(false);

        // EventTrigger를 통해 클릭 이벤트를 설정합니다.
        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        AddEventTrigger(eventTrigger, EventTriggerType.PointerClick, OnClicked);
    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, Action callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => { callback(); });
        trigger.triggers.Add(entry);
    }

    public void SetSelectedImg(bool b)
    {
        _cursoredImg.gameObject.SetActive(b);
    }

    public void OnClicked()
    {
        _action?.Invoke(_itemData.ItemID);
    }
}
