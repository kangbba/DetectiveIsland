using UnityEngine;

[CreateAssetMenu(menuName = "Create PlaceData", fileName = "PlaceData_")]
public class PlaceData : ScriptableObject
{
    [SerializeField] private string _placeID;
    [SerializeField] private string _placeNameForUser;
    [SerializeField] private Sprite _placeSprite;
    [Range(0,5)]
    [SerializeField] private float scaleFactor = 1f;

    public string PlaceID => _placeID;

    public string PlaceNameForUser => _placeNameForUser;

    public Sprite PlaceSprite { get => _placeSprite;  }
    public float ScaleFactor { get => scaleFactor; }
}
