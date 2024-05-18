using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;

[System.Serializable]
public class PagePlan
{
    [SerializeField] private float _xPoint;
    [SerializeField] private List<PlaceButton> _placeBtns;
    public float XPoint { get => _xPoint; }
}
public class Place : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRend;
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private int _initialPageIndex;
    [SerializeField] private List<PagePlan> _pagePlans = new List<PagePlan>();
    private int _curPageIndex;

    public bool IsPreviousPageExist => _pagePlans.Count > 1 && (_curPageIndex > 0);
    public bool IsNextPageExist => _pagePlans.Count > 1 && (_curPageIndex  < _pagePlans.Count - 1);

    public EPlaceID PlaceID => _placeID;
    public string PlaceNameForUser => _placeNameForUser;

    public List<PagePlan> PagePlans => _pagePlans;
    public PagePlan CurPagePlan => _pagePlans[_curPageIndex];

    public int CurPageIndex { get => _curPageIndex; }

    public void Initialize()
    {
        _spriteRend.sortingOrder = -10;
        _spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(0f), 0f);
        SetPage(_initialPageIndex);
    }

    public void SetNextPage(){
        if(!IsNextPageExist){
            Debug.LogWarning("다음 페이지가 없습니다");
            return;
        }
        SetPage(_curPageIndex + 1);
    }
    public void SetPreviousPage(){
        if(!IsPreviousPageExist){
            Debug.LogWarning("이전 페이지가 없습니다");
            return;
        }
        SetPage(_curPageIndex - 1);
    }
    public void SetPage(int targetPageIndex){
        _curPageIndex = targetPageIndex;
        CameraController.MoveX(CurPagePlan.XPoint, 1f);
    }
    public void FadeIn(float totalTime){
        _spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(1f), totalTime);
    }
    public void FadeOut(float totalTime){
        _spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(0f), totalTime);
    }
    public void FadeInFromStart(float totalTime){
        FadeOut(0f);
        FadeIn(totalTime);
    }
    public void FadeOutAndDestroy(float totalTime){
        _spriteRend.EaseSpriteColor(_spriteRend.color.ModifiedAlpha(0f), totalTime);
    }
}
