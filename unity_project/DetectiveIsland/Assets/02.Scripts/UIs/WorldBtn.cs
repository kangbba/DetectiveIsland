using Aroka.ArokaUtils;
using UnityEngine;

public class WorldBtn : MonoBehaviour
{
    protected bool _isMouseOver = false;
    protected bool _isDetecting = false; // 마우스 오버 감지 플래그
    [SerializeField] float _detectingRadius = 3f; // 반경 크기 설정
    protected Camera _mainCamera;
    private bool _isPressed = false;

    protected virtual void Start()
    {
        _mainCamera = Camera.main;
    }

    public void StartDetecting(bool isDetecting)
    {
        _isDetecting = isDetecting;
    }

    private void Update()
    {
        if (_isDetecting && !ArokaUtils.IsMouseOverUI())
        {
            CheckMouseOver();
            
            if (_isMouseOver && Input.GetMouseButtonDown(0))
            {
                _isPressed = true;
            }

            if (_isPressed && Input.GetMouseButtonUp(0))
            {
                if (_isMouseOver)
                {
                    OnClicked();
                }
                _isPressed = false;
            }
        }
        else{
            Debug.Log("체크하지않음");
        }
    }

    private void CheckMouseOver()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = _mainCamera.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        float distance = Vector3.Distance(mousePos, transform.position);
        _isMouseOver = distance <= _detectingRadius;

        if (_isMouseOver)
        {
            OnMouseOver();
        }
        else
        {
            OnMouseExit();
        }
    }

    protected virtual void OnClicked()
    {
        Debug.Log("World button clicked");
    }

    protected virtual void OnMouseOver()
    {
    }

    protected virtual void OnMouseExit()
    {
    }
}
