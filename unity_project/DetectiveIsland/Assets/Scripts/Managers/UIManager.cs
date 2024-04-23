using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;
    
    [SerializeField] private ItemCheckPanel _itemCheckPanel;
    [SerializeField] private ItemDemandPanel _itemDemandPanel;
    [SerializeField] private ItemOwnPanel _itemOwnPanel;
    [SerializeField] private PlacePanel _placePanel;
    [SerializeField] private PlaceUIPanel _placeUIPanel;
    [SerializeField] private CharacterPanel _characterPanel;
    [SerializeField] private DialoguePanel _dialoguePanel;
    [SerializeField] private ChoiceSetPanel _choiceSetPanel;

    public ItemCheckPanel ItemCheckPanel => _itemCheckPanel;
    public ItemDemandPanel ItemDemandPanel { get => _itemDemandPanel; }
    
    public CharacterPanel CharacterPanel { get => _characterPanel; }

    public PlaceUIPanel PlaceUIPanel { get => _placeUIPanel; }
    public ChoiceSetPanel ChoiceSetPanel { get => _choiceSetPanel; }
    public ItemOwnPanel ItemOwnPanel { get => _itemOwnPanel; }
    public DialoguePanel DialoguePanel { get => _dialoguePanel; }
    public PlacePanel PlacePanel { get => _placePanel; }

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
