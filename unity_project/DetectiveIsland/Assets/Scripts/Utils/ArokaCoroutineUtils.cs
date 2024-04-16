using UnityEngine;
using System.Collections;

public static class ArokaCoroutineUtils
{
    private static MonoBehaviour coroutineExecutor;


    // CoroutineExecutor 설정
    public static void SetCoroutineExecutor(MonoBehaviour executor)
    {
        coroutineExecutor = executor;
    }

    // 코루틴 시작
    public static Coroutine StartCoroutine(IEnumerator routine)
    {
        if (coroutineExecutor == null)
        {
            Debug.LogError("CoroutineExecutor is not set. Please call SetCoroutineExecutor method first.");
            return null;
        }

        return coroutineExecutor.StartCoroutine(routine);
    }

    // 코루틴 정지
    public static void StopCoroutine(Coroutine routine)
    {
        if (coroutineExecutor == null)
        {
            Debug.LogError("CoroutineExecutor is not set. Please call SetCoroutineExecutor method first.");
            return;
        }

        coroutineExecutor.StopCoroutine(routine);
    }

    // 지정된 시간 후에 코루틴 시작
    public static Coroutine StartCoroutineAfterDelay(float delay, IEnumerator routine)
    {
        return coroutineExecutor.StartCoroutine(DelayedCoroutine(delay, routine));
    }

    // 지연된 코루틴
    private static IEnumerator DelayedCoroutine(float delay, IEnumerator routine)
    {
        yield return new WaitForSeconds(delay);
        yield return coroutineExecutor.StartCoroutine(routine);
    }

    // 조건을 만족할 때까지 대기하는 코루틴
    public static IEnumerator WaitUntil(System.Func<bool> condition)
    {
        yield return new WaitUntil(condition);
    }

    // 일정 시간 동안 대기하는 코루틴
    public static IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
