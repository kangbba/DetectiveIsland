using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;

public static class PictureService 
{
    private static List<PictureData> _pictureDatas;
    private static Dictionary<string, PictureData> _instancedPictures;
    private static Transform _overlayedPicturePanel;

    public static void Load()
    {
        _pictureDatas = ArokaUtils.LoadResourcesFromFolder<PictureData>("PictureDatas");
        _instancedPictures = new Dictionary<string, PictureData>();
        _overlayedPicturePanel = new GameObject("Overlayed Picture Panel").transform;
    }

    public static PictureData GetPictureData(string id)
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
        string pictureID = overlayPicture.PictureID;
        string effectID = overlayPicture.EffectID;
        float effectTime = overlayPicture.EffectTime;
        PictureData foundPictureData = GetPictureData(pictureID);
        if(foundPictureData == null){
            Debug.LogWarning($"{foundPictureData.Id}에 해당하는 picture 없습니다");
            return;
        }
        switch (effectID)
        {
            case "FadeIn":
                PictureData instPictureData = GameObject.Instantiate(foundPictureData.gameObject, _overlayedPicturePanel).GetComponent<PictureData>();
                instPictureData.SpriteRenderer.FadeInFromStart(effectTime);
                _instancedPictures[pictureID] = instPictureData;
                break;
            case "FadeOut":
                if (_instancedPictures.TryGetValue(pictureID, out PictureData pictureData))
                {
                    pictureData.SpriteRenderer.FadeOut(effectTime);
                }
                break;
        }
    }
}
