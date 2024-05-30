using UnityEngine;

[System.Serializable]
public class EventPlan
{
    [SerializeField] private EventTime _eventTime;
    [SerializeField] private EPlaceID _placeID;
    [SerializeField] private int _placeSectionIndex;
    [SerializeField] private TextAsset _scenarioFile;
    [SerializeField] private bool _isSolved;

    public EventTime EventTime => _eventTime;
    public EPlaceID PlaceID => _placeID;
    public int PlaceSectionIndex => _placeSectionIndex;
    public TextAsset ScenarioFile => _scenarioFile;
    public bool IsSolved => _isSolved;

    public void SetSolved(bool b){
        _isSolved = b;
    }

    public PlaceSection PlaceSection => PlaceService.GetPlaceSection(_placeID, _placeSectionIndex);
}
