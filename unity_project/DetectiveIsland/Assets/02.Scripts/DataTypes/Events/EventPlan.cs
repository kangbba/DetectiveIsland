using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime;
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private int _placeSectionIndex;
    [SerializeField] private TextAsset _scenarioFile;
    [SerializeField] private EPlaceID _placeIDToAutoMoveAfter = EPlaceID.None;

    [Header("RUNTIME")]
    [SerializeField] private bool _isCleared;

    public EventTime EventTime => _eventTime;
    public EPlaceID PlaceID => _placeID;
    public int PlaceSectionIndex => _placeSectionIndex;
    public TextAsset ScenarioFile => _scenarioFile;
    public bool IsCleared => _isCleared;


    public void SetCleared(bool b){
        _isCleared = b;
    }

    public PlaceSection PlaceSection => PlaceService.GetPlaceSection(_placeID, _placeSectionIndex);

    public EPlaceID PlaceIDToAutoMoveAfter { get => _placeIDToAutoMoveAfter;  }
}
