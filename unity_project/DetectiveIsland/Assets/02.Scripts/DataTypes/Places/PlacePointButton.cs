using UnityEngine;

public class PlacePointButton : WorldButton
{
    private Place _parentPlace;

    protected override void Start()
    {
        base.Start();
    }

    public void Initialize(Place parentPlace)
    {
        _parentPlace = parentPlace;
    }

    protected override void OnButtonClicked()
    {
        _parentPlace.OnPlacePointClicked(this);
    }
}
