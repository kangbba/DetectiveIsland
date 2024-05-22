using UnityEngine;

public enum SpriteMouseOverState
{
    None,
    Normal,
    MouseOver,
    Clicked
}

public class WorldBtn : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _noneSprite;
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _mouseOverSprite;
    [SerializeField] private Sprite _clickedSprite;

    protected bool _isMouseOver = false;
    protected bool _isRunning = false; // 중복 실행을 막기 위한 플래그
    protected bool _isDetecting = false; // 마우스 오버 감지 플래그
    protected float _hoverRadius = 3f; // 반경 크기 설정
    protected Camera _mainCamera;

    protected virtual void Start()
    {
        _mainCamera = Camera.main;
        SetSpriteState(SpriteMouseOverState.None);
    }

    public void StartDetecting(bool isDetecting)
    {
        _isDetecting = isDetecting;
    }

    private void Update()
    {
        if (_isDetecting)
        {
            CheckMouseOver();

            if (Input.GetMouseButtonDown(0) && _isMouseOver && !_isRunning)
            {
                OnClicked();
            }
        }
    }

    private void CheckMouseOver()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = _mainCamera.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        float distance = Vector3.Distance(mousePos, transform.position);
        _isMouseOver = distance <= _hoverRadius;

        if (_isMouseOver)
        {
            SetSpriteState(SpriteMouseOverState.MouseOver);
        }
        else
        {
            SetSpriteState(SpriteMouseOverState.Normal);
        }

        Debug.Log(_isMouseOver);
    }

    protected virtual void OnClicked()
    {
        SetSpriteState(SpriteMouseOverState.Clicked);
        Debug.Log("World button clicked");
    }

    private void SetSpriteState(SpriteMouseOverState state)
    {
        switch (state)
        {
            case SpriteMouseOverState.None:
                _spriteRenderer.sprite = _noneSprite;
                break;
            case SpriteMouseOverState.Normal:
                _spriteRenderer.sprite = _normalSprite;
                break;
            case SpriteMouseOverState.MouseOver:
                _spriteRenderer.sprite = _mouseOverSprite;
                break;
            case SpriteMouseOverState.Clicked:
                _spriteRenderer.sprite = _clickedSprite;
                break;
        }
    }
}
