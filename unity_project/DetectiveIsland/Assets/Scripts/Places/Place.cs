using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using UnityEngine;

public class Place : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRend;
    [SerializeField] private string _placeID;
    [SerializeField] private string _placeNameForUser;
    private List<PlacePoint> _placePoints;

    public string PlaceID => _placeID.Trim();

    public string PlaceNameForUser => _placeNameForUser;

    public List<PlacePoint> PlacePoints { get => _placePoints; }
    public List<PlacePoint> MovingPlacePoints
    {
        get
        {
            return _placePoints.Where(placePoint => placePoint.EventAction.ActionType == EActionType.MoveToPlace).ToList();
        }
    }
    public void Initialize(){
        _spriteRend.sortingOrder = - 10;
        _spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(0f), 0f);
        _placePoints = transform.GetComponentsInChildren<PlacePoint>().ToList();
       
       PlaceService.DestroyMovingBtnsUI();
       PlaceService.InstantiateMovingBtnsUI(MovingPlacePoints);
    }
    public void FadeIn(float totalTime){
        _spriteRend.EaseSpriteColor(Color.white.ModifiedAlpha(1f), totalTime);
    }
    public void FadeOutAndDestroy(float totalTime){
        _spriteRend.EaseSpriteColor(_spriteRend.color.ModifiedAlpha(0f), totalTime);
    }

}
