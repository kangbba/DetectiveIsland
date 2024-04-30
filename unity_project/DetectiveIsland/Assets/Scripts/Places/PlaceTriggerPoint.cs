using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlacePoint : MonoBehaviour
{
    [SerializeField] private EventAction _eventAction;

    public EventAction EventAction { get => _eventAction;  }
}
