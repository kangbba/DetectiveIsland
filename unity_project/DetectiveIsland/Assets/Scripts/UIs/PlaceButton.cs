using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure to include this if you're using TextMeshPro for the button text

public class PlaceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text; // Reference to the Text component on the button
    [SerializeField] private Button _button; // Reference to the Button component

    public void Initialize(PlaceData placeData)
    {
        _text.text = placeData.PlaceNameForUser; // Set the button text
        _button.onClick.AddListener(() => HandleButtonClick(placeData)); // Add listener for click
    }

    private void HandleButtonClick(PlaceData placeData)
    {
        Debug.Log($"Button for {placeData.PlaceNameForUser} clicked!"); // Log when button is clicked
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners(); // Remove listeners when the button is destroyed
    }
}
