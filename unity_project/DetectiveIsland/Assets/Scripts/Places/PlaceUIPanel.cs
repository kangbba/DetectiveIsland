using System;
using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Profiling;
using UnityEngine;

public class PlaceUIPanel : MonoBehaviour
{

    [SerializeField] private ArokaAnim _placeUIPanelLeft;
    [SerializeField] private ArokaAnim _placeUIPanelRight;
    [SerializeField] private ArokaAnim _placeUIPanelUp;
    [SerializeField] private ArokaAnim _placeUIPanelCenter;

    [SerializeField] private TextMeshProUGUI _curPlaceText;


    public void MakeBtn(string placeID)
    {
    }
    public void SetCurPlaceText(string placeNameForUser){
        _curPlaceText.SetText(placeNameForUser);
    }
    public void OpenPanel(float totalTime){
        _placeUIPanelUp.SetAnim(true, totalTime);
        _placeUIPanelLeft.SetAnim(true, totalTime);
        _placeUIPanelRight.SetAnim(true, totalTime);
    }
    public void ClosePanel(float totalTime){
        _placeUIPanelUp.SetAnim(false, totalTime);
        _placeUIPanelLeft.SetAnim(false, totalTime);
        _placeUIPanelRight.SetAnim(false, totalTime);
    }
}
