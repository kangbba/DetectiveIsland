using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using UnityEngine;
using UnityEngine.UI;

public class DialogueArrow : MonoBehaviour
{
    [SerializeField] private RectTransform _arrowRectTransform;
    private const float _pingPongSpeed = 12f; // 움직이는 속도 조절

    void Update()
    {
        // Calculate vertical movement using Mathf.PingPong function
        float offsetY = Mathf.PingPong(Time.time * _pingPongSpeed, 10f) - 5f; // Adjust range as needed
        _arrowRectTransform.anchoredPosition = new Vector2(_arrowRectTransform.anchoredPosition.x, offsetY);
    }

    public void SetAnchordPos(Vector2 preferredValue){
        GetComponent<Image>().rectTransform.anchoredPosition = preferredValue;
    }
}
