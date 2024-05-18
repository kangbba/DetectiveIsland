using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;

public enum EPictureID
{
    None = 0,
    Black = 1,
    White = 2,
}
public enum EPictureEffectID
{
    None = 0,
    FadeIn = 1,
    FadeOut = 2,
}
public static class PictureService
{
    private static List<PictureData> _pictureDatas;
    private static Dictionary<EPictureID, PictureData> _instancedPictures;
    private static Transform _overlayedPicturePanel;

    public static void Load()
    {
        _pictureDatas = ArokaUtils.LoadResourcesFromFolder<PictureData>("PictureDatas");
        _instancedPictures = new Dictionary<EPictureID, PictureData>();
        _overlayedPicturePanel = new GameObject("Overlayed Picture Panel").transform;
    }

    public static PictureData GetPictureData(EPictureID id)
    {
        PictureData pictureData = _pictureDatas.FirstOrDefault(data => data.Id == id);
        if (pictureData == null)
        {
            Debug.LogWarning($"{id} 이름의 Picture 찾을수 없음");
        }
        return pictureData;
    }

    public static void SetPictureEffect(OverlayPicture overlayPicture)
    {
        EPictureID pictureID = overlayPicture.PictureID;
        EPictureEffectID effectID = overlayPicture.EffectID;
        float effectTime = overlayPicture.EffectTime;
        PictureData foundPictureData = GetPictureData(pictureID);
        if(foundPictureData == null){
            Debug.LogWarning($"{foundPictureData.Id}에 해당하는 picture 없습니다");
            return;
        }
        switch (effectID)
        {
            case EPictureEffectID.FadeIn:
                PictureData instPictureData = GameObject.Instantiate(foundPictureData.gameObject, _overlayedPicturePanel).GetComponent<PictureData>();
                instPictureData.SpriteRenderer.FadeInFromStart(effectTime);
                _instancedPictures[pictureID] = instPictureData;
                break;
            case EPictureEffectID.FadeOut:
                if (_instancedPictures.TryGetValue(pictureID, out PictureData pictureData))
                {
                    pictureData.SpriteRenderer.FadeOut(effectTime);
                }
                break;
            default:
                break;
        }
    }
}
