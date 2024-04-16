using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;
    
    [SerializeField] private ItemPanel _itemPanel;
    [SerializeField] private PlacePanel _placePanel;
    [SerializeField] private PlaceUIPanel _placeUIPanelLeft;
    [SerializeField] private PlaceUIPanel _placeUIPanelRight;
    [SerializeField] private CharacterPanel _characterPanel;
    [SerializeField] private DialoguePanel _dialoguePanel;

    public ItemPanel ItemPanel => _itemPanel;
    
    public PlacePanel PlacePanel => _placePanel;    
    
    public CharacterPanel CharacterPanel => _characterPanel;
    public DialoguePanel DialoguePanel => _dialoguePanel;

    public PlaceUIPanel PlaceUIPanelLeft { get => _placeUIPanelLeft; }
    public PlaceUIPanel PlaceUIPanelRight { get => _placeUIPanelRight; }

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

}
