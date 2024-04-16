using UnityEngine;

public class PlaceData : MonoBehaviour
{
    
    [SerializeField] private string _placeID;
    [SerializeField] private Sprite _placeSprite;
    [Range(0,5)]
    [SerializeField] private float scaleFactor = 1f;

    public string PlaceID => _placeID.ToString();

    public string PlaceNameForUser => transform.name;

    public Sprite PlaceSprite { get => _placeSprite;  }
    public float ScaleFactor { get => scaleFactor; }
}
