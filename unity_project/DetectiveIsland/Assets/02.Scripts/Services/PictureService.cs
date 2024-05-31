using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public enum EPictureID
{
    None = 0,
    Black = 1,
    White = 2,
    FrontOfCafeSeabreeze = 3,
    CafeDoorBell = 4,
}
public enum EPictureEffectID
{
    None = 0,
    FadeIn = 1,
    FadeOut = 2,
}
public static class PictureService
{
    private static List<Picture> _picturePrefabs;
    private static Dictionary<EPictureID, Picture> _instancedPictures;
    private static OverlayPicturePanel _overlayedPicturePanel;

    public static void Load(OverlayPicturePanel overlayPicturePanel)
    {
        _picturePrefabs = ArokaUtils.LoadResourcesFromFolder<Picture>("PicturePrefabs");
        _instancedPictures = new Dictionary<EPictureID, Picture>();
        _overlayedPicturePanel = overlayPicturePanel;
    }

    public static Picture GetPicture(EPictureID id)
    {
        Picture picture = _picturePrefabs.FirstOrDefault(data => data.Id == id);
        if (picture == null)
        {
            Debug.LogWarning($"{id} 이름의 Picture 찾을수 없음");
        }
        return picture;
    }
    public static async void SetOverlayPictureEffect(OverlayPicture overlayPicture)
    {
        EPictureID pictureID = overlayPicture.PictureID;
        EPictureEffectID effectID = overlayPicture.EffectID;
        float effectTime = overlayPicture.EffectTime;
        Picture foundPicture = GetPicture(pictureID);
        if(foundPicture == null){
            Debug.LogWarning($"{foundPicture.Id}에 해당하는 picture 없습니다");
            return;
        }
        switch (effectID)
        {
            case EPictureEffectID.FadeIn:
                Picture instPicture = GameObject.Instantiate(foundPicture.gameObject, _overlayedPicturePanel.transform).GetComponent<Picture>();
                instPicture.transform.localPosition = Vector3.zero;
                instPicture.FadeInFromBlack(effectTime);
                _instancedPictures[pictureID] = instPicture;
                break;
            case EPictureEffectID.FadeOut:
                if (_instancedPictures.TryGetValue(pictureID, out Picture picture))
                {
                    picture.FadeOutAndDestroy(effectTime);
                }
                break;
            default:
                break;
        }
        await UniTask.WaitForSeconds(effectTime);
    }
}
