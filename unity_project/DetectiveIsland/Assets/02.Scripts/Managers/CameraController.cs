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
    public static Camera MainCamera { get => _mainCamera; }
    private static Coroutine _shakeRoutine;

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
        _mainCamera.transform.EasePos(_mainCamera.transform.position.ModifiedX(x), totalTime, ArokaCurves.CurvName.EASE_IN_AND_OUT);
    }

    // 쉐이크 효과를 적용하는 함수
    public static void ShakeCamera(float magnitude, float totalTime)
    {
        if(_shakeRoutine != null){
            CoroutineUtils.StopCoroutine(_shakeRoutine);
        }
        _shakeRoutine = CoroutineUtils.StartCoroutine(ShakeRoutine(magnitude, totalTime));
    }

}
