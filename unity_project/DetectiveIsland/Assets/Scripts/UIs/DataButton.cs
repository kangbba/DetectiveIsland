using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class DataButton : MonoBehaviour
{
    [SerializeField] protected Button _button; // 공통 버튼 컴포넌트
    private string _btnKey;

    public string BtnKey { get => _btnKey; }
    
    // 클릭 이벤트를 설정하는 메서드
    protected void SetupButton(string btnKey, Action<string> action)
    {
        _btnKey = btnKey;
        _button.onClick.AddListener(() => {
            Debug.Log($"Button for {_btnKey} clicked!");
            action(_btnKey);
        });
    }

    protected void OnDestroy()
    {
        _button.onClick.RemoveAllListeners(); // 리스너 제거
    }

    public void SetInteractable(bool interactable)
    {
        _button.interactable = interactable; // 버튼 상호작용 가능 상태 설정
    }
}
