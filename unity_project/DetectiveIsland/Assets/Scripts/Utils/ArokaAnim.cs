using Aroka.EaseUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aroka.Anim
{

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
                case ArokaAnim.ObjectType.TmPro:
                    RectTransform rectTransform = transform as RectTransform;
                    TextMeshProUGUI tmpro = transform.GetComponent<TextMeshProUGUI>();
                    _localPos = rectTransform.anchoredPosition3D;
                    _localScale = rectTransform.localScale;
                    _localRot = transform.localRotation;
                    _color = tmpro ? tmpro.color : Color.white; // Use image color if available, otherwise default to white
                    break;
                case ArokaAnim.ObjectType.Button:
                case ArokaAnim.ObjectType.Image:
                    Image image = transform.GetComponent<Image>();
                    _localPos = image.rectTransform.anchoredPosition;
                    _localScale = transform.localScale;
                    _localRot = transform.localRotation;
                    _color = image ? image.color : Color.white; // Use image color if available, otherwise default to white
                    break;
                case ArokaAnim.ObjectType.SpriteRenderer:
                    SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
                    _localPos = transform.position;
                    _localScale = transform.localScale;
                    _localRot = transform.localRotation;
                    _color = spriteRenderer ? spriteRenderer.color : Color.white; // Use sprite renderer color if available
                    break;
                case ArokaAnim.ObjectType.MeshRenderer:
                    MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
                    _localPos = transform.position;
                    _localScale = transform.localScale;
                    _localRot = transform.localRotation;
                    _color = meshRenderer && meshRenderer.sharedMaterial ? meshRenderer.sharedMaterial.color : Color.white; // Use material color if available
                    break;
                case ArokaAnim.ObjectType.Normal:
                    _localPos = transform.position;
                    _localScale = transform.localScale;
                    _localRot = transform.localRotation;
                    _color = Color.white; // Default color for normal objects without specific color property
                    break;
            }
        }
    }

    public class ArokaAnim : MonoBehaviour
    {
        [SerializeField] private AnimState _onState;
        [SerializeField] private AnimState _offState;
        [SerializeField] private bool _manualControl;
        ObjectType Type => DetermineObjectType(transform);

        public enum ObjectType
        {
            Button,
            TmPro,
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
        public void SetAnim(bool isOn, float totalTime, bool calledFromGroup = false)
        {
            if(calledFromGroup && _manualControl){
                
                return;
            }
            AnimState targetState = isOn ? _onState : _offState;

            // AnimState에서 오브젝트 타입 결정

            // 타입에 따라 분기하여 처리
            switch (Type)
            {
                case ObjectType.Button:
                    transform.GetComponent<Image>().EaseAnchoredPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    transform.GetComponent<Image>().EaseColor(targetState.Color, totalTime);
                    transform.GetComponent<Button>().interactable = isOn;
                    break;
                case ObjectType.TmPro:
                    transform.GetComponent<Image>().EaseAnchoredPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    transform.GetComponent<TextMeshProUGUI>().EaseColor(targetState.Color, totalTime);
                    break;
                case ObjectType.Image:
                    transform.GetComponent<Image>().EaseAnchoredPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    transform.GetComponent<Image>().EaseColor(targetState.Color, totalTime);
                    break;
                case ObjectType.SpriteRenderer:
                    transform.EaseLocalPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    transform.GetComponent<SpriteRenderer>().EaseColor(targetState.Color, totalTime);
                    break;
                case ObjectType.MeshRenderer:
                    transform.EaseLocalPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    break;
                case ObjectType.Normal:
                    transform.EaseLocalPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
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
            if (transform.GetComponent<Button>() != null)
                return ObjectType.Button;
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
}
