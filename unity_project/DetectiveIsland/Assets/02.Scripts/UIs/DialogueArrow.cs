using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using Aroka.ArokaUtils;
using UnityEngine;
using UnityEngine.UI;

public class DialogueArrow : MonoBehaviour
{
    [SerializeField] private RectTransform _arrowRectTransform;
    private const float _pingPongSpeed = 12f; // 움직이는 속도 조절

    private bool _isActive;

    void Update()
    {
        // Calculate vertical movement using Mathf.PingPong function
        float offsetY = Mathf.PingPong(Time.time * _pingPongSpeed, 10f) - 5f; // Adjust range as needed
        _arrowRectTransform.anchoredPosition = new Vector2(_arrowRectTransform.anchoredPosition.x, offsetY);
    }

    public void SetAnchordPos(Vector2 preferredValue){
        GetComponent<Image>().rectTransform.anchoredPosition = preferredValue;
    }

    public void ShowDialogueArrow(Vector2 initialAnchoredPos){
        _isActive = true;
        _arrowRectTransform.GetComponent<Image>().color = Color.white.ModifiedAlpha(1f);
        SetAnchordPos(initialAnchoredPos);
    }

    public void HideDialogueArrow(){
        _isActive = false;
        _arrowRectTransform.GetComponent<Image>().color = Color.white.ModifiedAlpha(0f);
    }
}
