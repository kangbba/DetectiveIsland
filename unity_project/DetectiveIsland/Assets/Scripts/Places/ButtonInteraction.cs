using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    protected string _placeID;

    protected virtual void OnMouseDown()  // This method works with mouse input
    {
        Debug.Log("Button for " + _placeID + " was clicked.");
        // Base reaction to clicking, override in derived classes if needed
    }
}
