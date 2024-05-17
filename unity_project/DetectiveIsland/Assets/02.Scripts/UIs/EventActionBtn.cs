using System.Collections;
using System.Collections.Generic;
using Aroka.EaseUtils;
using UnityEngine;
using UnityEngine.UI;

public class EventActionBtn : MonoBehaviour
{
    [SerializeField] private Button _button; // Reference to the Button component
    private EventAction _eventActionOnPressed;

    public void Initialize(EventAction eventAction){
        _eventActionOnPressed = eventAction;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ExecuteEventAction);
    }

    private void ExecuteEventAction()
    {
        if (_eventActionOnPressed == null)
        {
            Debug.LogError("No EventAction configured for this button.");
            return;
        }
        _eventActionOnPressed.ExecuteAction();
    }

    private void SetInteractable(bool isInteractable)
    {
        if (_button == null)
        {
            return;
        }
        _button.interactable = isInteractable;
    }
    public void SetOn(bool b, float totalTime){
        transform.EaseLocalScale((b ? 1 : 0) * Vector3.one , totalTime);
        SetInteractable(!b);
    }

    void OnDestroy()
    {
        // It's good practice to remove listeners when the GameObject is destroyed
        if (_button != null)
        {
            _button.onClick.RemoveListener(ExecuteEventAction);
        }
    }
}
