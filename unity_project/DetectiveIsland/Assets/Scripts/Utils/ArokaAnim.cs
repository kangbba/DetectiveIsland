using Aroka.EaseUtils;
using TMPro;
using UnityEditor;
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

        public AnimState(ArokaAnim arokaAnim, Transform transform)
        {
            Debug.Log(arokaAnim.ObjectType);
            switch (arokaAnim.ObjectType)
            {
                case ObjectType.TextMeshProUGUI:
                    TextMeshProUGUI tmpro = transform.GetComponent<TextMeshProUGUI>();
                    _localPos = transform.GetComponent<TextMeshProUGUI>().rectTransform.anchoredPosition;
                    _localScale = transform.localScale;
                    _localRot = transform.localRotation;
                    _color = tmpro ? tmpro.color : Color.white; // Use image color if available, otherwise default to white
                    break;
                case ObjectType.Button:
                case ObjectType.Image:
                    Image image = transform.GetComponent<Image>();
                    _localPos = image.rectTransform.anchoredPosition;
                    _localScale = transform.localScale;
                    _localRot = transform.localRotation;
                    _color = image ? image.color : Color.white; // Use image color if available, otherwise default to white
                    break;
                case ObjectType.SpriteRenderer:
                    SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
                    _localPos = transform.position;
                    _localScale = transform.localScale;
                    _localRot = transform.localRotation;
                    _color = spriteRenderer ? spriteRenderer.color : Color.white; // Use sprite renderer color if available
                    break;
                case ObjectType.MeshRenderer:
                    MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
                    _localPos = transform.position;
                    _localScale = transform.localScale;
                    _localRot = transform.localRotation;
                    _color = meshRenderer && meshRenderer.sharedMaterial ? meshRenderer.sharedMaterial.color : Color.white; // Use material color if available
                    break;
                case ObjectType.Normal:
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
        private bool _isOn;
        private bool _isInitialized = false;
        [SerializeField] private AnimState _onState;
        [SerializeField] private AnimState _offState;

        public ObjectType ObjectType { get => DetermineObjectType(transform); }
        private ObjectType DetermineObjectType(Transform transform)
        {
            if (transform.GetComponent<SpriteRenderer>() != null)
                return ObjectType.SpriteRenderer;
            else if (transform.GetComponent<Button>() != null)
                return ObjectType.Button;
            else if (transform.GetComponent<TextMeshProUGUI>() != null)
                return ObjectType.TextMeshProUGUI;
            else if (transform.GetComponent<Image>() != null)
                return ObjectType.Image;
            else if (transform.GetComponent<MeshRenderer>() != null)
                return ObjectType.MeshRenderer;
            else
                return ObjectType.Normal;
        }
        private void Awake()
        {
            SetAnim(false, 0f);
            _isInitialized = true;
        }
        private void Reset()
        {
            Debug.Log("기본 세팅으로 자동 등록되었습니다.");
            AnimState animStateOn = new AnimState(this, transform);
            RegisterState(true, animStateOn);
            AnimState animStateOff = new AnimState(animStateOn.LocalPos, Vector3.one, animStateOn.LocalRot, animStateOn.Color);
            RegisterState(false, animStateOff);
        }

        private void RegisterState(bool isOn, AnimState state)
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

        [ArokaButton()]
        public void EditorRegister_On(){
            RegisterStateWithCurrent(true);
        }
        [ArokaButton()]
        public void EditorRegister_Off(){
            RegisterStateWithCurrent(false);
        }
        [ArokaButton()]
        public void EditorPreview_On(){
            PreviewState(true);
        }
        [ArokaButton()]
        public void EditorPreview_Off(){
            PreviewState(false);
        }

        private void PreviewState(bool isOn)
        {
            if (Application.isPlaying)
            {
                // 플레이 모드일 때는 애니메이션 시간을 1초로 설정
                SetAnim(isOn, 1f);
            }
            else
            {
                // 플레이 모드가 아닐 때는 즉시 상태 변경 (애니메이션 시간을 0초로 설정)
                SetAnim(isOn, 0f);
            }
        }

        public void RegisterOn(AnimState animState){
            _onState = animState;
        }
        public void RegisterOff(AnimState animState){
            _offState = animState;
        }
        private void RegisterStateWithCurrent(bool isOn)
        {
            AnimState currentState = new AnimState(this, transform);
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
            if(targetState == null){
                Debug.LogWarning("Target State is null");
                return;
            }
            if(_isInitialized && _isOn == isOn){
                return;
            }
            _isInitialized = true;
            _isOn = isOn;
            
            // 타입에 따라 분기하여 처리
            switch (ObjectType)
            {
                case ObjectType.Button:
                    Button btn = GetComponent<Button>();
                    btn.GetComponent<Image>().rectTransform.EaseAnchoredPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    transform.EaseColor(targetState.Color, totalTime);
                    btn.interactable = isOn;
                    break;
                case ObjectType.TextMeshProUGUI:
                    TextMeshProUGUI tmpro = GetComponent<TextMeshProUGUI>();
                    tmpro.rectTransform.EaseAnchoredPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    transform.EaseColor(targetState.Color, totalTime);
                    break;
                case ObjectType.Image:
                    Image img = GetComponent<Image>();
                    img.rectTransform.EaseAnchoredPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    transform.EaseColor(targetState.Color, totalTime);
                    break;
                case ObjectType.SpriteRenderer:
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    transform.EaseLocalPos(targetState.LocalPos, totalTime);
                    transform.EaseLocalScale(targetState.LocalScale, totalTime);
                    transform.EaseRot(targetState.LocalRot, totalTime);
                    transform.EaseColor(targetState.Color, totalTime);
                    break;
                case ObjectType.MeshRenderer:
                    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
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
    }
    
}

