using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;

public class Place : MonoBehaviour
{
    [SerializeField] private string _placeID;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private SpriteRenderer _spriteRend;

    private List<EventActionWorldBtn> _eventActionBtns = new List<EventActionWorldBtn>();

    private void Start(){
        _eventActionBtns = GetComponentsInChildren<EventActionWorldBtn>().ToList();
        _spriteRend.sortingOrder = - 10;
    }   

    public string PlaceID => _placeID.Trim();

    public string PlaceNameForUser => _placeNameForUser;

    public void Initialize(){
        _spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(0f), 0f);
    }
    public void FadeIn(float totalTime){
        _spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(1f), totalTime);
    }
    public void FadeOutAndDestroy(float totalTime){
        _spriteRend.EaseSpriteColor(_spriteRend.color.ModifiedAlpha(0f), totalTime);
    }

    public void ShowPlaceMoveBtns(float totalTime){
        foreach(EventActionWorldBtn btn in _eventActionBtns){
            btn.SetOn(true, totalTime);
        }
    }
    public void HidePlaceMoveBtns(float totalTime){
        foreach(EventActionWorldBtn btn in _eventActionBtns){
            btn.SetOn(false, totalTime);
        }
    }
}
