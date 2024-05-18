using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placeText;
    [SerializeField] private EPlaceID _placeID;
    private EventAction _moveToPlaceAction;

    private void Start(){
        // 장소로 이동하는 액션
        _moveToPlaceAction = new EventAction(new MoveToPlaceAction(_placeID));

        // 클릭 이벤트 추가
        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
        AddEventTrigger(eventTrigger, EventTriggerType.PointerClick, OnClicked);
    }
    public void Initialize(Place place)
    {
        placeText.SetText(place.PlaceNameForUser);

    }

    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((eventData) => { callback(); });
        trigger.triggers.Add(entry);
    }

    private void OnClicked()
    {
        _moveToPlaceAction.Execute();
    }
}
