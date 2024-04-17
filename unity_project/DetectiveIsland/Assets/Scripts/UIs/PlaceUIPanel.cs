using System;
using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using Aroka.ArokaUtils;
using TMPro;
using Unity.Profiling;
using UnityEngine;

public class PlaceUIPanel : MonoBehaviour
{

    [SerializeField] private PlaceButton _placeBtnPrefab;
    [SerializeField] private ArokaAnim _placeUIPanelLeft;
    [SerializeField] private ArokaAnim _placeUIPanelRight;
    [SerializeField] private ArokaAnim _placeUIPanelUp;

    [SerializeField] private TextMeshProUGUI _curPlaceText;

    private static List<PlaceButton> _curPlaceBtns = new List<PlaceButton>(); // List to store button components

    public void SetOnPanel(bool up, bool left, bool right, float totalTime){
        _placeUIPanelUp.SetAnim(up, totalTime);
        _placeUIPanelLeft.SetAnim(left, totalTime);
        _placeUIPanelRight.SetAnim(right, totalTime);
    }
    public void SetCurPlaceText(string placeNameForUser){
        _curPlaceText.SetText(placeNameForUser);
    }

    public void CreatePlaceButtons(string selectedPlaceID, Action<string> moveAction)
    {
        DestroyPlaceButtons();

        
        GameObject selectedGameObject = PlaceUIService.GetPlaceDataGameObject(selectedPlaceID);
        if (selectedGameObject == null)
        {
            return; // 선택된 GameObject가 없으면 함수 종료
        }

        // 부모 PlaceData 처리
        Transform parentTransform = selectedGameObject.transform.parent;
        if (parentTransform != null)
        {
            PlaceData parentPlaceData = parentTransform.GetComponent<PlaceData>();
            if (parentPlaceData != null)
            {
                PlaceButton buttonLeft = GameObject.Instantiate(_placeBtnPrefab, _placeUIPanelLeft.transform);
                buttonLeft.Initialize(parentPlaceData, moveAction);
                _curPlaceBtns.Add(buttonLeft);
            }
        }

        // 선택된 장소의 자식 PlaceData 처리
        float spacingBetweenButtons = 150;
        int numChildren = selectedGameObject.transform.childCount;
        float totalHeight = numChildren * spacingBetweenButtons;
        float startingY = totalHeight / 2f; // 중앙으로부터의 시작 Y 위치

        for (int i = 0; i < numChildren; i++)
        {
            Transform child = selectedGameObject.transform.GetChild(i);
            PlaceData childPlaceData = child.GetComponent<PlaceData>();
            if (childPlaceData != null)
            {
                // 버튼의 Y 위치를 반대로 계산하여 위로 생성되도록 함
                float buttonPosY = startingY - i * spacingBetweenButtons;
                Vector3 anchordPosition = Vector3.up * buttonPosY;
                PlaceButton buttonRight = GameObject.Instantiate(_placeBtnPrefab, _placeUIPanelRight.transform);
                buttonRight.GetComponent<RectTransform>().anchoredPosition = anchordPosition;
                buttonRight.Initialize(childPlaceData, moveAction);
                _curPlaceBtns.Add(buttonRight);
            }
        }

    }
    public void SetInteractablePlaceButtons(bool b)
    {
        foreach(PlaceButton placeButton in _curPlaceBtns){
            placeButton.SetInteractable(b);
        }
    }
    public void DestroyPlaceButtons(){

        _placeUIPanelLeft.transform.DestroyAllChildren();
        _placeUIPanelRight.transform.DestroyAllChildren();
        _curPlaceBtns.Clear();
    }
}
