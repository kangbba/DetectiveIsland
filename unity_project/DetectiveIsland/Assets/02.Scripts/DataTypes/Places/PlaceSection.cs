using UnityEngine;

public class PlaceSection : MonoBehaviour
{
    [SerializeField] private int _sectionIndex;
    public int SectionIndex  => _sectionIndex;
    public float SectionCenterX => transform.position.x;
}
