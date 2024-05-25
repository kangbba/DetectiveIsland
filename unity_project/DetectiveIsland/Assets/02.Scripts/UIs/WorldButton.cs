using UnityEngine;
using UnityEngine.UI;

public abstract class WorldButton : MonoBehaviour
{
    private Button _button;

    protected virtual void Start()
    {
        LoadAndCreateButton();
    }

    private void LoadAndCreateButton()
    {
        GameObject buttonPrefab = Resources.Load<GameObject>("UIPrefabs/PlaceButtonPrefab");
        if (buttonPrefab == null)
        {
            Debug.LogError("Button Prefab could not be loaded from Resources.");
            return;
        }

        GameObject canvasObj = UIManager.MainCanvas.gameObject;

        GameObject buttonObj = Instantiate(buttonPrefab, canvasObj.transform);

        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("Button Prefab does not have RectTransform component.");
            return;
        }

        _button = buttonObj.GetComponent<Button>();
        if (_button == null)
        {
            Debug.LogError("Button Prefab does not have Button component.");
            return;
        }

        _button.onClick.AddListener(OnButtonClicked);
        _button.interactable = false;

        PositionButton(true);  // 초기 위치 설정
    }

    private void PositionButton(bool forceUpdate = false)
    {
        if (_button != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (forceUpdate || _button.transform.position != screenPos)
            {
                _button.transform.position = screenPos;
            }
        }
    }

    private void Update()
    {
        PositionButton();
    }

    public void SetButtonInteractable(bool b)
    {
        if(_button == null){
            return;
        }
        _button.interactable = b;
    }

    protected abstract void OnButtonClicked();

    private void OnDestroy()
    {
        if (_button != null)
        {
            Destroy(_button.gameObject);
        }
    }
}
