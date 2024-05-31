using System;
using UnityEngine;
using UnityEngine.UI;

public class UIParent : MonoBehaviour
{
    private Canvas _mainCanvas;
    private CanvasRenderer _mainCanvasRenderer;
    [SerializeField] private OverlaySentenceDisplayer _overlaySentenceDisplayer;
    [SerializeField] private EventTimeDisplayer _eventTimeDisplayer;
    [SerializeField] private OverlayPicturePanel _overlayPicturePanel;
    [SerializeField] private PlaceUIPanel _placeUIPanel;
    [SerializeField] private DialoguePanel _dialoguePanel;
    [SerializeField] private SimpleDialoguePanel _simpleDialoguePanel;
    [SerializeField] private ItemDemandPanel _itemDemandPanel;
    [SerializeField] private ChoiceSetPanel _choiceSetPanel;
    [SerializeField] private ItemCheckPanel _itemCheckPanel;
    [SerializeField] private Button _itemCheckPanelEnterBtn;
    [SerializeField] private ItemOwnPanel _itemOwnPanel;
    [SerializeField] private UIMouseCursor _uiMouseCursor;

    public OverlaySentenceDisplayer OverlaySentenceDisplayer => _overlaySentenceDisplayer;
    public ItemDemandPanel ItemDemandPanel => _itemDemandPanel;
    public ItemOwnPanel ItemOwnPanel => _itemOwnPanel;
    public DialoguePanel DialoguePanel => _dialoguePanel;
    public ChoiceSetPanel ChoiceSetPanel => _choiceSetPanel;
    public EventTimeDisplayer EventTimeDisplayer => _eventTimeDisplayer;
    public PlaceUIPanel PlaceUIPanel => _placeUIPanel;
    public UIMouseCursor UIMouseCursor => _uiMouseCursor;
    public ItemCheckPanel ItemCheckPanel => _itemCheckPanel;
    public Button ItemCheckPanelEnterBtn => _itemCheckPanelEnterBtn;

    public SimpleDialoguePanel SimpleDialoguePanel { get => _simpleDialoguePanel; }
    public Canvas MainCanvas { get => _mainCanvas;  }
    public CanvasRenderer MainCanvasRenderer { get => _mainCanvasRenderer; }
    public OverlayPicturePanel OverlayPicturePanel { get => _overlayPicturePanel; }

    private void Start(){
        _mainCanvas = GetComponent<Canvas>();
        _mainCanvasRenderer = GetComponent<CanvasRenderer>();
    }
    public void Init(){

    }
}
