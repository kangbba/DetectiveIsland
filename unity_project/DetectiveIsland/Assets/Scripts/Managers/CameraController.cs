using UnityEngine;
using System.Collections;
using Aroka.EaseUtils;
using Aroka.ArokaUtils;
using Aroka.Curves;
using Cysharp.Threading.Tasks;
using System.Data.Common;
using Aroka.CoroutineUtils;

public static class CameraController
{
    private static Camera _mainCamera;
    private static Coroutine _shakeRoutine;
    private static float _targetAspectRatio = 16f / 9f;  // Set this to your game's designed aspect ratio


    public static void Load(){
        _mainCamera = Camera.main;
    }
    // 쉐이크 효과를 적용하는 코루틴
    public static IEnumerator ShakeRoutine(float magnitude, float totalTime)
    {
        Vector3 originalPos = _mainCamera.transform.position;

        float elapsed = 0f;

        while (elapsed < totalTime)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Vector3 targetPos = originalPos + new Vector3(x, y, originalPos.z);
            _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, targetPos, Time.deltaTime * 10f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _mainCamera.transform.position = originalPos;
    }

    public static void MoveX(float x, float totalTime){
        _mainCamera.transform.EasePos(_mainCamera.transform.position.ModifiedX(x), totalTime, ArokaCurves.CurvName.EASE_OUT);
    }

    // 쉐이크 효과를 적용하는 함수
    public static void ShakeCamera(float magnitude, float totalTime)
    {
        if(_shakeRoutine != null){
            CoroutineUtils.StopCoroutine(_shakeRoutine);
        }
        _shakeRoutine = CoroutineUtils.StartCoroutine(ShakeRoutine(magnitude, totalTime));
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
