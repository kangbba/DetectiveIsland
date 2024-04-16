using UnityEngine;
using System.Collections;

public static class CameraController
{
    private static Camera _mainCamera;
    private static Coroutine _shakeCoroutine;
    private static float _targetAspectRatio = 16f / 9f;  // Set this to your game's designed aspect ratio

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
            ArokaCoroutineUtils.StopCoroutine(_shakeCoroutine);
        }

        _shakeCoroutine = ArokaCoroutineUtils.StartCoroutine(ShakeCoroutine(magnitude, totalTime));
    }
    // main 카메라를 할당하는 함수
    private static void RegisterMainCamera()
    {
        if(_mainCamera == null){
           _mainCamera = Camera.main;
        }
    } 
    public static void AdjustCamera()
    {
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / _targetAspectRatio;
        Camera camera = Camera.main;

        if (scaleHeight < 1.0f)  // When the screen is wider than your target aspect
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else  // When the screen is narrower than your target aspect
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

}
