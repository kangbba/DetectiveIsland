using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaceButton : EventActionBtn
{
    [SerializeField] private TextMeshProUGUI placeText;
    public void Initialize(string placeID, string placeNameForUser){
        placeText.SetText(placeNameForUser);
        base.Initialize(new EventAction(EActionType.MoveToPlace, placeID));
    }
}
