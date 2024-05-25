using UnityEngine;

public enum EMouseCursorMode
{
    Normal,
    Detect
}

public class UIMouseCursor : MonoBehaviour
{
    [SerializeField] private Canvas _canvas; // UI 요소를 렌더링하는 Canvas
    [SerializeField] private RectTransform _cursorImage; // 마우스 커서 위치를 위한 빈 RectTransform
    [SerializeField] private GameObject _normalImg; // Normal 모드에서 사용할 이미지
    [SerializeField] private GameObject _detectingImg; // Detect 모드에서 사용할 이미지

    private EMouseCursorMode _currentMode;

    private void Start()
    {
        Cursor.visible = false; // 기본 마우스 커서 숨기기
        SetState(EMouseCursorMode.Normal);
    }

    void Update()
    {
        // 마우스 위치를 가져와서 캔버스 상의 위치로 변환
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out mousePos);

        // 마우스 커서 이미지를 마우스 위치로 이동
        _cursorImage.localPosition = mousePos;
    }

    public void SetState(EMouseCursorMode mode)
    {
        _currentMode = mode;
        switch (mode)
        {
            case EMouseCursorMode.Normal:
                _cursorImage.gameObject.SetActive(true);
                _normalImg.SetActive(true);
                _detectingImg.SetActive(false);
                break;
            case EMouseCursorMode.Detect:
                _cursorImage.gameObject.SetActive(true);
                _normalImg.SetActive(true);
                _detectingImg.SetActive(true);
                break;
        }
    }
}
