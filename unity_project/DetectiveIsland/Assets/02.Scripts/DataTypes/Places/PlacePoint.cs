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
public class PlacePoint : MonoBehaviour
{
    [SerializeField] private ButtonActionType _actionType;

    [ShowIf(nameof(_actionType), ButtonActionType.MovePlace)]
    [SerializeField] private EPlaceID _placeID;

    [ShowIf(nameof(_actionType), ButtonActionType.GetItem)]
    [SerializeField] private EItemID _itemID;

    [ShowIf(nameof(_actionType), ButtonActionType.PlayEvent)]
    [SerializeField] private TextAsset _scenarioFile;

    public async void ProperAction()
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
                Scenario scenario = _scenarioFile.LoadScenario();
                await EventProcessor.PlayScenarioWithDialougePanel(scenario);
                break;
            default:
                Debug.LogWarning("Unsupported action type.");
                break;
        }
    }

}
