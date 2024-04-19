using UnityEngine;

public class UIMouseCursor : MonoBehaviour
{
    public RectTransform cursorImage; // 마우스 커서 이미지의 RectTransform
    public Canvas canvas; // UI 요소를 렌더링하는 Canvas

    void Start()
    {
        Cursor.visible = false; // 기본 마우스 커서 숨기기
    }

    void Update()
    {
        // 마우스 위치를 가져와서 캔버스 상의 위치로 변환
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out mousePos);

        // 마우스 커서 이미지를 마우스 위치로 이동
        cursorImage.localPosition = mousePos;
    }
}
