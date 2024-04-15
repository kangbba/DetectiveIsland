using System.Collections;
using System.Collections.Generic;
using ArokaUtil;
using UnityEngine;

public class PlacePanel : ArokaAnim
{
    SpriteRenderer _curSprite;
    public void SetPlace(PlaceData placeData)
    {
        if(_curSprite != null){
            _curSprite.transform.ArokaTr().SetSpriteRendererColor(Color.white.ModifiedAlpha(0f), 1f);
            Destroy(_curSprite.gameObject, 1f);
        }
        // 새로운 GameObject 생성
        GameObject placeObject = new GameObject($"{placeData.PlaceNameForUser} ({placeData.PlaceID})");
        placeObject.transform.SetParent(transform); // 부모 설정
        placeObject.transform.localScale = Vector3.one * placeData.ScaleFactor;
        _curSprite = placeObject.AddComponent<SpriteRenderer>();
        
        _curSprite.transform.ArokaTr().SetSpriteRendererColor(Color.white.ModifiedAlpha(0f), 0f);
        _curSprite.transform.ArokaTr().SetSpriteRendererColor(Color.white.ModifiedAlpha(1f), 1f);
        // 스프라이트 렌더러 추가

        // 스프라이트 설정
        _curSprite.sprite = placeData.PlaceSprite;
    }
}
