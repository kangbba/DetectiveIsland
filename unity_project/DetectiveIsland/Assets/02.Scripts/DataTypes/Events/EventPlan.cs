using UnityEngine;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime;
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private int _placeSectionIndex;
    [SerializeField] private TextAsset _scenarioFile;
    private bool _isAlreadyViewed;

    public EventTime EventTime => _eventTime;
    public EPlaceID PlaceID => _placeID;
    public int PlaceSectionIndex => _placeSectionIndex;
    public TextAsset ScenarioFile => _scenarioFile;
    public bool IsAlreadyViewed => _isAlreadyViewed;

    public PlaceSection PlaceSection => PlaceService.GetPlaceSection(_placeID, _placeSectionIndex);
}
