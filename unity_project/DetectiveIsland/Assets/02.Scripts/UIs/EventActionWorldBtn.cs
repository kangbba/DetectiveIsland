using Aroka.EaseUtils;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]  // Ensure a Collider component is always attached.
public class EventActionWorldBtn : MonoBehaviour
{
    [SerializeField] private EventAction _eventActionOnPressed;
    [SerializeField] private Collider2D _col; 

    private void Start(){
        _col.isTrigger = true;
        SetInteractable(true);
    }
    private void OnMouseDown()
    {
        if (_eventActionOnPressed == null)
        {
            Debug.LogError("No EventAction configured for this world button.");
            return;
        }
        _eventActionOnPressed.ExecuteAction();
    }

    public void SetOn(bool b, float totalTime){
        Debug.Log($"SetOn {b} 호출");
        transform.EaseLocalScale((b ? 1 : 0) * Vector3.one , totalTime);
        SetInteractable(b);
    }

    private void SetInteractable(bool b){
        _col.enabled = b;
    }
}
