using UnityEngine;
using System.Collections;


namespace Aroka.CoroutineUtils{

    public static class ArokaCoroutineUtils
    {
        // 더미 MonoBehaviour 클래스 생성
        public class DummyMonoBehaviour : MonoBehaviour
        {
            // 이 클래스는 특별한 로직을 가지지 않으며, 코루틴 실행을 위한 MonoBehaviour 기능만을 제공합니다.
        }
        private static MonoBehaviour coroutineExecutor;
        private static void CheckMonoBehaviourAndAssign()
        {
            if (coroutineExecutor == null)
            {
                if (Application.isPlaying) // 플레이 모드에서만 실행
                {
                    GameObject obj = new GameObject("CoroutineExecutor");
                    coroutineExecutor = obj.AddComponent<DummyMonoBehaviour>();
                    UnityEngine.Object.DontDestroyOnLoad(obj); // 게임이 종료될 때까지 존재하도록 설정
                }
                else
                {
                    Debug.LogWarning("Attempted to initialize CoroutineExecutor while not in play mode. This operation is only valid while playing.");
                }
            }
        }


        // 코루틴 시작
        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            CheckMonoBehaviourAndAssign();
            return coroutineExecutor.StartCoroutine(routine);
        }
        // 코루틴 정지
        public static void StopCoroutine(Coroutine routine)
        {
            CheckMonoBehaviourAndAssign();
            coroutineExecutor.StopCoroutine(routine);
        }

        // 지정된 시간 후에 코루틴 시작
        public static Coroutine StartCoroutineAfterDelay(float delay, IEnumerator routine)
        {
            CheckMonoBehaviourAndAssign();
            return coroutineExecutor.StartCoroutine(DelayedCoroutine(delay, routine));
        }

        // 지연된 코루틴
        private static IEnumerator DelayedCoroutine(float delay, IEnumerator routine)
        {
            CheckMonoBehaviourAndAssign();
            yield return new WaitForSeconds(delay);
            yield return coroutineExecutor.StartCoroutine(routine);
        }

        // 조건을 만족할 때까지 대기하는 코루틴
        public static IEnumerator WaitUntil(System.Func<bool> condition)
        {
            CheckMonoBehaviourAndAssign();
            yield return new WaitUntil(condition);
        }

        // 일정 시간 동안 대기하는 코루틴
        public static IEnumerator WaitForSecondsCoroutine(float seconds)
        {
            CheckMonoBehaviourAndAssign();
            yield return new WaitForSeconds(seconds);
        }
    }

    
}
