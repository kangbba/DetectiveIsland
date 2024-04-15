using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ArokaUtil;

[System.Serializable]
public class UIState
{
    [SerializeField] private Vector2 _position;
    [SerializeField] private Vector2 _scale;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private Color _color;

    public Vector2 Position => _position;
    public Vector2 Scale => _scale;
    public Vector3 Rotation => _rotation;
    public Color Color => _color;

    public UIState(Vector2 position, Vector2 scale, Vector3 rotation, Color color)
    {
        _position = position;
        _scale = scale;
        _rotation = rotation;
        _color = color;
    }

    public UIState(RectTransform rectTransform, Image image)
    {
        _position = rectTransform.anchoredPosition;
        _scale = rectTransform.localScale;
        _rotation = rectTransform.localEulerAngles;
        _color = image.color;
    }
}

public class UIAnim : MonoBehaviour
{
    [SerializeField] private UIState _onState;
    [SerializeField] private UIState _offState;


    private void Awake()
    {
        SetAnim(false, 0f);
    }

    private void Reset()
    {
        Debug.Log("기본 세팅으로 자동 등록되었습니다 ");
        UIState uiStateOn = new UIState(GetComponent<RectTransform>(), GetComponent<Image>());
        RegisterState(true, uiStateOn);
        RegisterStateWithCurrent(true);
        UIState uiStateOff = new UIState(uiStateOn.Position, uiStateOn.Scale, uiStateOn.Rotation, uiStateOn.Color.ModifiedAlpha(0));
        RegisterState(false, uiStateOff);
    }

    public void RegisterState(bool isOn, UIState uiState)
    {
        if (isOn)
        {
            _onState = uiState;
        }
        else
        {
            _offState = uiState;
        }
    }

    public void RegisterStateWithCurrent(bool isOn)
    {
        if (isOn)
        {
            _onState = new UIState(transform.GetComponent<RectTransform>(), GetComponent<Image>());
        }
        else
        {
            _offState = new UIState(transform.GetComponent<RectTransform>(), GetComponent<Image>());
        }
    }

    public void SetAnim(bool isOn, float totalTime)
    {
        if (isOn)
        {
            transform.ArokaTr().SetAnchoredPos(_onState.Position, totalTime);
            transform.ArokaTr().SetLocalScale(_onState.Scale, totalTime);
            transform.ArokaTr().SetRot(Quaternion.Euler(_onState.Rotation), totalTime);
            transform.ArokaTr().SetColor(_onState.Color, totalTime);
        }
        else
        {
            transform.ArokaTr().SetAnchoredPos(_offState.Position, totalTime);
            transform.ArokaTr().SetLocalScale(_offState.Scale, totalTime);
            transform.ArokaTr().SetRot(Quaternion.Euler(_offState.Rotation), totalTime);
            transform.ArokaTr().SetColor(_offState.Color, totalTime);
        }
    }
    public void SetOnAnimFromOff(float totalTime)
    {
        SetAnim(false, 0f);
        SetAnim(true, totalTime);
    }
}
