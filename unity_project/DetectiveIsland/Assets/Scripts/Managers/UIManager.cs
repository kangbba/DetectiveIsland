using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _placePanel;
    [SerializeField] private GameObject _itemPanel;
    [SerializeField] private GameObject _characterPanel;
    [SerializeField] private GameObject _linePnael;

    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate UIManager instance detected. Destroying the new instance.");
            Destroy(gameObject);
        }
    }

    public GameObject PlacePanel => _placePanel;
    public GameObject ItemPanel => _itemPanel;
    public GameObject CharacterPanel => _characterPanel;
    public GameObject LinePanel => _linePnael;
}
