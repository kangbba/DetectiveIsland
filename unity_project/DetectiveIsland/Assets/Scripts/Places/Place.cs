using System.Collections;
using System.Collections.Generic;
using Aroka.EaseUtils;
using UnityEngine;

public class Place : SpriteEffector
{
    private PlaceData _placeData;

    public PlaceData PlaceData { get => _placeData; }

    public void Initialize(PlaceData placeData){
        _placeData = placeData;
        base.SetSprite(placeData.PlaceSprite, -1);
    }
    public void Enter(float totalTime){
        FadeInFromStart(totalTime);
    }
    public void Exit(float totalTime){
        FadeOut(totalTime);
    }
}
