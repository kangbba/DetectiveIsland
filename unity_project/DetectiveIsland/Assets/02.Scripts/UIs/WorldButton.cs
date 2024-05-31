using UnityEngine;

public class WorldTracker : MonoBehaviour
{
    private Transform _targetTransform;

    public void Initialize(Transform targetTransform)
    {
        if (targetTransform == null)
        {
            Debug.LogError("Target Transform is null.");
            return;
        }

        _targetTransform = targetTransform;

        PositionTracker(true);  // 초기 위치 설정
    }

    private void PositionTracker(bool forceUpdate = false)
    {
        if (_targetTransform != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(_targetTransform.position);
            if (forceUpdate || transform.position != screenPos)
            {
                transform.position = screenPos;
            }
        }
    }

    private void Update()
    {
        PositionTracker();
    }
}
