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

    private Coroutine _fadeRoutine;
    private Coroutine _blinkCoroutine;
    private Coroutine _talkingCoroutine;

    public string EmotionID => _emotionID;

    private void Start(){
        if(_backgroundRenderer != null)
            _backgroundRenderer.gameObject.SetActive(true);
        if(_faceRenderer != null)
            _faceRenderer.gameObject.SetActive(true);
        if(_eyesRenderer != null)
            _eyesRenderer.gameObject.SetActive(true);
        if(_mouthRenderer != null)
            _mouthRenderer.gameObject.SetActive(true);
    }

    public void SetOn(bool b, float totalTime){
        if(b){
            Fade(true, totalTime);
            StartBlinking();
        }
        else{
            Fade(false, totalTime);
            StopBlinking();
        }
    }
    private void Fade(bool b, float totalTime){
        if(_fadeRoutine != null){
            StopCoroutine(_fadeRoutine);
        }
        StartCoroutine(FadeRoutine(b, totalTime));
    }

    IEnumerator FadeRoutine(bool b, float totalTime){
        if(b){
            Debug.Log("Fadein");
            _backgroundRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), totalTime);
            yield return new WaitForSeconds(totalTime);
            _faceRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), 0);
            _eyesRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), 0);
            _mouthRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(1f), 0);
        }
        else{
            Debug.Log("FadeOut");
            _eyesRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), 0);
            _mouthRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), 0);
            _faceRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), 0);
            _backgroundRenderer.EaseSpriteColor(Color.white.ModifiedAlpha(0f), totalTime);
        }
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
        Debug.Log("로그5");
        
    }
    public void StopTalking()
    {
        if(_mouthSprites == null || _mouthSprites.Count == 0){
            return;
        }
        if (_talkingCoroutine != null)
            StopCoroutine(_talkingCoroutine);
        SetMouthSprite(0); 
        Debug.Log("로그6");
    }

    private IEnumerator BlinkingRoutine()
    {
        int index = 1;
        int spriteCount = _eyeSprites.Count;
        while (true)
        {
            SetEyesSprite(index % spriteCount);
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
        int index = 1;
        float accumTime = 0f;
        while (accumTime < totalTime)
        {
            accumTime += Time.deltaTime;
            if(accumTime > .1f){
                SetMouthSprite(index % _mouthSprites.Count);
                index++;
                accumTime = 0f;
            }
            yield return null;
        }
        _talkingCoroutine = null;
    }
    private void SetMouthSprite(int index){
        if(index >= _mouthSprites.Count){
            return;
        }
        Sprite sprite = _mouthSprites[index];
        _mouthRenderer.sprite = sprite;
    }
    private void SetEyesSprite(int index){
        if(index >= _eyeSprites.Count){
            return;
        }
        Sprite sprite = _eyeSprites[index];
        _eyesRenderer.sprite = sprite;
    }

}
