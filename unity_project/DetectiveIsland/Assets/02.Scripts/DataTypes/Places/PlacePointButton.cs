using ArokaInspector.Attributes;
using Cysharp.Threading.Tasks;
using UnityEngine;
public enum ButtonActionType
{
    None,
    MovePlace,
    GetItem,
    PlayEvent
}
public class PlacePointButton : WorldButton
{
    private Place _parentPlace;
    [SerializeField] private ButtonActionType _actionType;

    [ShowIf(nameof(_actionType), ButtonActionType.MovePlace)]
    [SerializeField] private EPlaceID _placeID;

    [ShowIf(nameof(_actionType), ButtonActionType.GetItem)]
    [SerializeField] private EItemID _itemID;

    [ShowIf(nameof(_actionType), ButtonActionType.PlayEvent)]
    [SerializeField] private TextAsset _scenarioFile;

    public void Initialize(Place parentPlace)
    {
        _parentPlace = parentPlace;
    }

    protected override void OnButtonClicked()
    {
        _parentPlace.OnPlacePointClicked(this);
    }

    public async void Execute()
    {
        switch (_actionType)
        {
            case ButtonActionType.MovePlace:
                Debug.Log($"Moving to place: {_placeID}");
                PlaceService.MoveToPlace(_placeID, 0);
                break;
            case ButtonActionType.GetItem:
                Debug.Log($"Getting item: {_itemID}");
                await EventProcessor.ProcessGainItem(new GainItem(true, _itemID, 1));
                break;
            case ButtonActionType.PlayEvent:
                Debug.Log("Playing event...");
                await EventProcessor.PlayScenarioFileOnly(_scenarioFile);
                break;
            default:
                Debug.LogWarning("Unsupported action type.");
                break;
        }
    }

}
