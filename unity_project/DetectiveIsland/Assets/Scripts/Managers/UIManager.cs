using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private ArokaAnim _placePanel;
    [SerializeField] private ArokaAnim _itemPanel;
    [SerializeField] private ArokaAnim _characterPanel;
    [SerializeField] private ArokaAnim _linePnael;

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

    public ArokaAnim PlacePanel => _placePanel;
    public ArokaAnim ItemPanel => _itemPanel;
    public ArokaAnim CharacterPanel => _characterPanel;
    public ArokaAnim LinePanel => _linePnael;
}
