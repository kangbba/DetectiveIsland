using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ArokaUtil;

public class ArokaAnim : MonoBehaviour
{
    [SerializeField] private AnimState _onState;
    [SerializeField] private AnimState _offState;
    ObjectType Type => DetermineObjectType(transform);


    public enum ObjectType
    {
        Image,          // GameObject with an Image component
        SpriteRenderer, // GameObject with a SpriteRenderer component
        MeshRenderer,   // GameObject with a MeshRenderer component
        Normal          // GameObject with no specific renderer
    }
    private void Awake()
    {
        SetAnim(false, 0f);
    }

    private void Reset()
    {
        Debug.Log("기본 세팅으로 자동 등록되었습니다.");
        AnimState animStateOn = new AnimState(transform, Type);
        RegisterState(true, animStateOn);
        AnimState animStateOff = new AnimState(animStateOn.LocalPos, Vector3.zero, animStateOn.LocalRot, animStateOn.Color);
        RegisterState(false, animStateOff);
    }

    public void RegisterState(bool isOn, AnimState state)
    {
        if (isOn)
        {
            _onState = state;
        }
        else
        {
            _offState = state;
        }
    }

    public void RegisterStateWithCurrent(bool isOn)
    {
        AnimState currentState = new AnimState(transform, Type);
        if (isOn)
        {
            _onState = currentState;
        }
        else
        {
            _offState = currentState;
        }
    }
    public void SetAnim(bool isOn, float totalTime)
    {
        AnimState targetState = isOn ? _onState : _offState;

        // AnimState에서 오브젝트 타입 결정

        // 타입에 따라 분기하여 처리
        switch (Type)
        {
            case ObjectType.Image:
                // RectTransform과 함께 있는 Image 컴포넌트의 경우
                transform.ArokaTr().SetAnchoredPos(targetState.LocalPos, totalTime);
                // 공통 속성 적용
                transform.ArokaTr().SetLocalScale(targetState.LocalScale, totalTime);
                transform.ArokaTr().SetRot(targetState.LocalRot, totalTime);
                transform.ArokaTr().SetImageColor(targetState.Color, totalTime);
                break;
            case ObjectType.SpriteRenderer:
            case ObjectType.MeshRenderer:
            case ObjectType.Normal:
                // SpriteRenderer, MeshRenderer, 또는 일반 오브젝트인 경우
                transform.ArokaTr().SetLocalPos(targetState.LocalPos, totalTime);
                transform.ArokaTr().SetLocalScale(targetState.LocalScale, totalTime);
                transform.ArokaTr().SetRot(targetState.LocalRot, totalTime);
                break;
        }

    }

    public void SetOnAnimFromOff(float totalTime)
    {
        SetAnim(false, 0f);
        SetAnim(true, totalTime);
    }
    private ObjectType DetermineObjectType(Transform transform)
    {
        if (transform.GetComponent<Image>() != null)
            return ObjectType.Image;
        else if (transform.GetComponent<SpriteRenderer>() != null)
            return ObjectType.SpriteRenderer;
        else if (transform.GetComponent<MeshRenderer>() != null)
            return ObjectType.MeshRenderer;
        else
            return ObjectType.Normal;
    }
}

[System.Serializable]
public class AnimState
{
    [SerializeField] private Vector3 _localPos;
    [SerializeField] private Vector3 _localScale;
    [SerializeField] private Quaternion _localRot;
    [SerializeField] private Color _color;

    public Vector3 LocalPos => _localPos;
    public Vector3 LocalScale => _localScale;
    public Quaternion LocalRot => _localRot;
    public Color Color { get => _color; set => _color = value; }

    public AnimState(Vector3 pos, Vector3 scale, Quaternion rotation, Color color)
    {
        _localPos = pos;
        _localScale = scale;
        _localRot = rotation;
        _color = color;
    }

    public AnimState(Transform transform, ArokaAnim.ObjectType type)
    {
        switch (type)
        {
            case ArokaAnim.ObjectType.Image:
                RectTransform rectTransform = transform as RectTransform;
                Image image = transform.GetComponent<Image>();
                _localPos = rectTransform.anchoredPosition3D;
                _localScale = rectTransform.localScale;
                _localRot = transform.localRotation;
                _color = image.color;
                break;
            case ArokaAnim.ObjectType.SpriteRenderer:
                SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
                _localPos = transform.position;
                _localScale = transform.localScale;
                _localRot = transform.localRotation;
                _color = spriteRenderer.color;
                break;
            case ArokaAnim.ObjectType.MeshRenderer:
                MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
                _localPos = transform.position;
                _localScale = transform.localScale;
                _localRot = transform.localRotation;
                _color = meshRenderer.sharedMaterial.color;
                break;
            case ArokaAnim.ObjectType.Normal:
                _localPos = transform.position;
                _localScale = transform.localScale;
                _localRot = transform.localRotation;
                _color = Color.white; // Default color
                break;
        }
    }

}
