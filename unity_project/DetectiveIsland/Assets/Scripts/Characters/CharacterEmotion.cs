using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;

public class CharacterEmotion : MonoBehaviour // MonoBehaviour를 상속 받아야 함
{
    [SerializeField] private string _emotionID;
    [SerializeField] private SpriteRenderer _backgroundRenderer;
    [SerializeField] private SpriteRenderer _faceRenderer;
    [SerializeField] private SpriteRenderer _eyesRenderer;  // Sprite 대신 SpriteRenderer로 변경
    [SerializeField] private SpriteRenderer _mouthRenderer;  // Sprite 대신 SpriteRenderer로 변경

    [SerializeField] private List<Sprite> _eyeSprites = new List<Sprite>();  // 눈 깜빡임 스프라이트 배열
    [SerializeField] private List<Sprite> _mouthSprites = new List<Sprite>();// 입 움직임 스프라이트 배열

    private Coroutine _blinkCoroutine;
    private Coroutine _talkingCoroutine;

    public string EmotionID => _emotionID;

    public void Initialize(){
        _backgroundRenderer.gameObject.SetActive(true);
        _faceRenderer.gameObject.SetActive(true);
        _eyesRenderer.gameObject.SetActive(true);
        _mouthRenderer.gameObject.SetActive(true);
    }

    public void SetOn(bool b, float totalTime){
        if(b){
            FadeIn(totalTime);
            StartBlinking();
            StartTalking(10f);
        }
        else{
            FadeOut(totalTime);
            StopBlinking();
            StopTalking();
        }
    }
    private void FadeIn(float duration)
    {
        Debug.Log("Fadein");
        _backgroundRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), duration);
        _faceRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), duration);
        _eyesRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), duration);
        _mouthRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), duration);
    }

    // Fade out the emotion visual elements
    private void FadeOut(float duration)
    {
        Debug.Log("FadeOut");
        _backgroundRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), duration);
        _faceRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), duration);
        _eyesRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), duration);
        _mouthRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), duration);
    }

    private void StartBlinking()
    {
        if(_eyeSprites == null || _eyeSprites.Count == 0){
            return;
        }
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = StartCoroutine(BlinkingRoutine());
    }
    private void StopBlinking()
    {
        if(_eyeSprites == null || _eyeSprites.Count == 0){
            return;
        }
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
        SetEyesSprite(0);
    }

    public void StartTalking(float totalTime)
    {
        if(_mouthSprites == null || _mouthSprites.Count == 0){
            return;
        }
        if (_talkingCoroutine != null)
            StopCoroutine(_talkingCoroutine);
        _talkingCoroutine = StartCoroutine(TalkingRoutine(totalTime));
        
    }
    public void StopTalking()
    {
        if(_mouthSprites == null || _mouthSprites.Count == 0){
            return;
        }
        if (_talkingCoroutine != null)
            StopCoroutine(_talkingCoroutine);
        SetMouthSprite(0); 
    }

    private IEnumerator BlinkingRoutine()
    {
        int index = 0;
        int spriteCount = _eyeSprites.Count;
        while (true)
        {
            SetEyesSprite(index);
           if(index % spriteCount == 0){
              yield return new WaitForSeconds(3f);
            }
            else{
              yield return new WaitForSeconds(.2f);
            }
            index++;
        }
    }


    private IEnumerator TalkingRoutine(float totalTime)
    {
        int index = 0;
        float accumTime = 0f;
        while (accumTime < totalTime)
        {
            accumTime += Time.deltaTime;
            if(accumTime > .2f){
                SetMouthSprite(index);
                index++;
                accumTime = 0f;
            }
            yield return null;
        }
        _talkingCoroutine = null;
    }
    private void SetMouthSprite(int index){
        Sprite sprite = _mouthSprites[index % _mouthSprites.Count];
        _mouthRenderer.sprite = sprite;
    }
    private void SetEyesSprite(int index){
        Sprite sprite = _eyeSprites[index % _eyeSprites.Count];
        _eyesRenderer.sprite = sprite;
    }

}
