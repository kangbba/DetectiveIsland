using System.Collections;
using System.Collections.Generic;
using Aroka.JsonUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlacePoint : MonoBehaviour
{
    [SerializeField] private TextAsset _jNodeFile;
    private bool _isMouseOver = false;
    private bool _isRunning = false; // 중복 실행을 막기 위한 플래그
    private float _hoverRadius = 5f; // 반경 크기 설정
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = CameraController.MainCamera;
        if (_jNodeFile == null)
        {
            Debug.LogWarning("대본 없는 PLACE POINT 존재");
        }
    }
    public void StartDetectingMouseOver(bool b)
    {
        if(b){
            StartCoroutine(DetectMouseOverRoutine());
        }
        else{
            StopCoroutine(DetectMouseOverRoutine());
        }
    }
    private IEnumerator DetectMouseOverRoutine()
    {
        while (true)
        {
            CheckMouseOver();
            yield return null; // 매 프레임마다 감지
        }
    }

    private void CheckMouseOver()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = _mainCamera.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        float distance = Vector3.Distance(mousePos, transform.position);
        _isMouseOver = distance <= _hoverRadius;

        Debug.Log(_isMouseOver);
    }

    private void OnMouseDown()
    {
        if (_isMouseOver && !_isRunning)
        {
            Debug.Log("마우스 클릭");
            StartScenarioTask().Forget();
        }
    }

    private async UniTaskVoid StartScenarioTask()
    {
        _isRunning = true;
        try
        {
            Scenario scenario = ArokaJsonUtils.LoadScenario(_jNodeFile);
            await EventProcessor.ScenarioTask(scenario);
        }
        finally
        {
            _isRunning = false;
        }
    }
}
