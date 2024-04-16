using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : SpriteEffector
{
    private PlaceData _placeData;

    public PlaceData PlaceData { get => _placeData; }

    public void Initialize(PlaceData placeData){
        _placeData = placeData;
        base.Initialize();
        base.SetSprite(placeData.PlaceSprite);
    }
    public void Enter(float totalTime){
        FadeInFromStart(totalTime);
    }
    public void Exit(float totalTime){
        FadeOut(totalTime);
        Destroy(gameObject, totalTime);
    }
}
