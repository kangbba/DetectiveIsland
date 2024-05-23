using UnityEngine;

public class UIMouseCursor : MonoBehaviour
{
    [SerializeField] private Canvas _canvas; // UI 요소를 렌더링하는 Canvas
    public RectTransform cursorImage; // 마우스 커서 이미지의 RectTransform

    private void Start()
    {
        Cursor.visible = false; // 기본 마우스 커서 숨기기
    }
    void Update()
    {
        // 마우스 위치를 가져와서 캔버스 상의 위치로 변환
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, Input.mousePosition, _canvas.worldCamera, out mousePos);

        // 마우스 커서 이미지를 마우스 위치로 이동
        cursorImage.localPosition = mousePos;
    }
}
