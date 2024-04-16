using UnityEngine;
using System.Collections;

public static class CameraController
{
    private static Camera _mainCamera;
    private static Coroutine _shakeCoroutine;

    // 쉐이크 효과를 적용하는 코루틴
    public static IEnumerator ShakeCoroutine(float magnitude, float totalTime)
    {
        RegisterMainCamera();
        Vector3 originalPos = _mainCamera.transform.position;

        float elapsed = 0f;

        while (elapsed < totalTime)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            _mainCamera.transform.position = originalPos + new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _mainCamera.transform.position = originalPos;
    }

    // 쉐이크 효과를 적용하는 함수
    public static void ShakeCamera(float magnitude, float totalTime)
    {
        if (_shakeCoroutine != null)
        {
            // 이미 실행 중인 쉐이크 코루틴이 있다면 중지
            CoroutineUtils.StopCoroutine(_shakeCoroutine);
        }

        _shakeCoroutine = CoroutineUtils.StartCoroutine(ShakeCoroutine(magnitude, totalTime));
    }
    // main 카메라를 할당하는 함수
    private static void RegisterMainCamera()
    {
        if(_mainCamera == null){
           _mainCamera = Camera.main;
        }
    }
}
