using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePanel : ArokaAnim
{
    public void SetPlace(PlaceData placeData)
    {
        // 새로운 GameObject 생성
        GameObject placeObject = new GameObject($"{placeData.PlaceNameForUser} ({placeData.PlaceID})");
        placeObject.transform.SetParent(transform); // 부모 설정

        // 스프라이트 렌더러 추가
        SpriteRenderer spriteRenderer = placeObject.AddComponent<SpriteRenderer>();

        // 스프라이트 설정
        spriteRenderer.sprite = placeData.PlaceSprite;
    }
}
