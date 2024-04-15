using UnityEngine;

[CreateAssetMenu(menuName = "Create PlaceData", fileName = "PlaceData_")]
public class PlaceData : ScriptableObject
{
    [SerializeField] private string _placeID;
    [SerializeField] private string _placeNameForUser;

    public string PlaceID => _placeID;

    public string PlaceNameForUser => _placeNameForUser;
}
