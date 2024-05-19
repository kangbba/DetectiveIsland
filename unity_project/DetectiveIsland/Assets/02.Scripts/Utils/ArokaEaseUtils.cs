using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using Aroka.CoroutineUtils;
using Aroka.Curves;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aroka.EaseUtils
{
    public enum ObjectType
    {
        Button,
        TextMeshProUGUI,
        Image,         
        SpriteRenderer, 
        MeshRenderer,   
        Normal         
    }
    public static class ArokaEaseUtils
    {
        private static Dictionary<(Transform, string), Coroutine> _coroutineMap = new Dictionary<(Transform, string), Coroutine>();
        private static Dictionary<(Component, string), Coroutine> _colorCoroutineMap = new Dictionary<(Component, string), Coroutine>();
        private static Coroutine _alphaCoroutine;


        #region Position and Movement Extensions

        public static void EasePos(this Transform transform, Vector3 targetPos, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_IN_AND_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                transform.position = targetPos;
                return;
            }
            StartOrReplaceCoroutine(transform, "position", EasePosRoutine(transform, targetPos, totalTime, curvName, delayTime, isWorld: true));
        }

        public static void EaseLocalPos(this Transform transform, Vector3 targetPos, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_IN_AND_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                transform.localPosition = targetPos;
                return;
            }
            StartOrReplaceCoroutine(transform, "localPosition", EasePosRoutine(transform, targetPos, totalTime, curvName, delayTime, isWorld: false));
        }

        private static IEnumerator EasePosRoutine(Transform transform, Vector3 targetPos, float totalTime, ArokaCurves.CurvName curvName, float delayTime, bool isWorld, Transform parent = null)
        {
            yield return new WaitForSeconds(delayTime);
            if (parent) transform.SetParent(parent);
            AnimationCurve curve = ArokaCurves.GetCurve(curvName);
            Vector3 initialPos = isWorld ? transform.position : transform.localPosition;
            float elapsed = 0;

            while (elapsed < totalTime)
            {
                elapsed += Time.deltaTime;
                float t = curve.Evaluate(elapsed / totalTime);
                Vector3 newPos = Vector3.Lerp(initialPos, targetPos, t);
                if (isWorld)
                    transform.position = newPos;
                else
                    transform.localPosition = newPos;
                yield return null;
            }

            if (isWorld)
                transform.position = targetPos;
            else
                transform.localPosition = targetPos;
        }

        #endregion

        #region Scale Extensions

        public static void EaseLocalScale(this Transform transform, Vector3 targetScale, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                transform.localScale = targetScale;
                return;
            }
            StartOrReplaceCoroutine(transform, "scale", EaseScaleRoutine(transform, targetScale, totalTime, curvName, delayTime));
        }

        private static IEnumerator EaseScaleRoutine(Transform transform, Vector3 targetScale, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            yield return new WaitForSeconds(delayTime);
            AnimationCurve curve = ArokaCurves.GetCurve(curvName);
            Vector3 initialScale = transform.localScale;
            float elapsed = 0;

            while (elapsed < totalTime)
            {
                elapsed += Time.deltaTime;
                float t = curve.Evaluate(elapsed / totalTime);
                transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                yield return null;
            }

            transform.localScale = targetScale;
        }

        #endregion

        #region Rotation Extensions

        public static void EaseRot(this Transform transform, Quaternion targetRotation, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                transform.rotation = targetRotation;
                return;
            }
            StartOrReplaceCoroutine(transform, "rotation", EaseRotationRoutine(transform, targetRotation, totalTime, curvName, delayTime, false));
        }

        public static void EaseLocalRot(this Transform transform, Quaternion targetRotation, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                transform.localRotation = targetRotation;
                return;
            }
            StartOrReplaceCoroutine(transform, "localRotation", EaseRotationRoutine(transform, targetRotation, totalTime, curvName, delayTime, true));
        }

        public static void EaseRotEuler(this Transform transform, Vector3 targetEuler, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                transform.rotation = Quaternion.Euler(targetEuler);
                return;
            }
            Quaternion targetRotation = Quaternion.Euler(targetEuler);
            StartOrReplaceCoroutine(transform, "rotation", EaseRotationRoutine(transform, targetRotation, totalTime, curvName, delayTime, false));
        }

        private static IEnumerator EaseRotationRoutine(Transform transform, Quaternion targetRot, float totalTime, ArokaCurves.CurvName curvName, float delayTime, bool isLocal)
        {
            yield return new WaitForSeconds(delayTime);
            AnimationCurve curve = ArokaCurves.GetCurve(curvName);
            Quaternion initialRot = isLocal ? transform.localRotation : transform.rotation;
            float elapsed = 0;

            while (elapsed < totalTime)
            {
                elapsed += Time.deltaTime;
                float t = curve.Evaluate(elapsed / totalTime);
                Quaternion newRotation = Quaternion.Slerp(initialRot, targetRot, t);

                if (isLocal)
                    transform.localRotation = newRotation;
                else
                    transform.rotation = newRotation;

                yield return null;
            }

            if (isLocal)
                transform.localRotation = targetRot;
            else
                transform.rotation = targetRot;
        }

        #endregion

        #region Anchored Position Extensions
        public static void EaseAnchoredPos(this RectTransform rectTransform, Vector2 targetPos, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_IN_AND_OUT, float delayTime = 0)
        {
            if (rectTransform == null)
            {
                Debug.LogWarning("rect transform이 없으므로 호출하지 않습니다");
                return;
            }
            if (totalTime == 0)
            {
                rectTransform.anchoredPosition = targetPos;
                return;
            }
            StartOrReplaceCoroutine(rectTransform, "anchoredPosition", EaseAnchoredPositionRoutine(rectTransform, targetPos, totalTime, curvName, delayTime));
        }

        private static IEnumerator EaseAnchoredPositionRoutine(RectTransform rectTransform, Vector2 targetPos, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0F)
        {
            yield return new WaitForSeconds(delayTime);
            AnimationCurve curve = ArokaCurves.GetCurve(curvName);
            Vector2 initialPos = rectTransform.anchoredPosition;
            float elapsed = 0;

            while (elapsed < totalTime)
            {
                elapsed += Time.deltaTime;
                float t = curve.Evaluate(elapsed / totalTime);
                rectTransform.anchoredPosition = Vector2.Lerp(initialPos, targetPos, t);
                yield return null;
            }

            rectTransform.anchoredPosition = targetPos;
        }

        #endregion

        #region Color Extensions


        public static void EaseColor(this Transform tr, Color targetColor, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            Component component = DetermineComponent(tr);
            string key = GetComponentKey(component);

            if (component == null || string.IsNullOrEmpty(key))
            {
                Debug.LogWarning("No applicable component found for color easing on this transform.");
                return;
            }

            if (totalTime == 0f)
            {
                // 즉시 목표 색상으로 설정
                SetColor(component, targetColor);
                return;
            }

            // 색상 변경 코루틴 시작
            IEnumerator routine = EaseColorRoutine(component, targetColor, totalTime, curvName, delayTime);
            StartOrReplaceColorCoroutine(component, key, routine);
        }
        public static void EaseSpriteColor(this SpriteRenderer spriteRend, Color targetColor, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_IN_AND_OUT, float delayTime = 0)
        {
            string key = GetComponentKey(spriteRend);

            if (spriteRend == null || string.IsNullOrEmpty(key))
            {
                Debug.LogWarning("No applicable component found for color easing on this transform.");
                return;
            }

            if (totalTime == 0f)
            {
                // 즉시 목표 색상으로 설정
                SetColor(spriteRend, targetColor);
                return;
            }

            // 색상 변경 코루틴 시작
            IEnumerator routine = EaseColorRoutine(spriteRend, targetColor, totalTime, curvName, delayTime);
            StartOrReplaceColorCoroutine(spriteRend, key, routine);
        }


        public static void EaseCanvasGroupAlpha(this CanvasGroup canvasGroup, float targetAlpha, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_IN_AND_OUT, float delayTime = 0)
        {
            if (canvasGroup == null)
            {
                Debug.LogWarning("CanvasGroup component is null.");
                return;
            }

            if (totalTime == 0f)
            {
                canvasGroup.alpha = targetAlpha;
                return;
            }

            if (_alphaCoroutine != null)
            {
                CoroutineUtils.CoroutineUtils.StopCoroutine(_alphaCoroutine);
            }

            _alphaCoroutine = CoroutineUtils.CoroutineUtils.StartCoroutine(EaseAlphaRoutine(canvasGroup, targetAlpha, totalTime, curvName, delayTime));
        }

        private static IEnumerator EaseAlphaRoutine(CanvasGroup canvasGroup, float targetAlpha, float totalTime, ArokaCurves.CurvName curvName, float delayTime)
        {
            if (delayTime > 0)
            {
                yield return new WaitForSeconds(delayTime);
            }

            float startAlpha = canvasGroup.alpha;
            float elapsedTime = 0f;
            AnimationCurve curv = ArokaCurves.GetCurve(curvName);
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / totalTime);
                float curvedT = curv.Evaluate(t);
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, curvedT);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
        }

        private static string GetComponentKey(Component component)
        {
            if (component is SpriteRenderer)
                return "spriteColor";
            if (component is Image)
                return "imageColor";
            if (component is TextMeshProUGUI)
                return "tmproColor";
            if (component is Button)
                return "buttonImageColor"; // Button 이미지 색상 변경을 위한 키

            return string.Empty;
        }
        public static Color GetColor(Component component)
        {
            if (component is Image img)
                return img.color;
            if (component is TextMeshProUGUI tmp)
                return tmp.color;
            if (component is SpriteRenderer spr)
                return spr.color;
            if (component is Button btn)
                return btn.image.color; // Button의 Image 컴포넌트 색상

            Debug.LogWarning("GetColor called on unsupported component type.");
            return Color.clear;
        }

        public static void SetColor(Component component, Color color)
        {
            if (component is Image img)
                img.color = color;
            if (component is TextMeshProUGUI tmp)
                tmp.color = color;
            if (component is SpriteRenderer spr)
                spr.color = color;
            if (component is Button btn)
                btn.image.color = color;
        }

        private static Component DetermineComponent(Transform tr)
        {
            if (tr.GetComponent<Button>() != null)
                return tr.GetComponent<Button>().image; // Button 케이스 추가
            if (tr.GetComponent<Image>() != null)
                return tr.GetComponent<Image>();
            if (tr.GetComponent<TextMeshProUGUI>() != null)
                return tr.GetComponent<TextMeshProUGUI>();
            if (tr.GetComponent<SpriteRenderer>() != null)
                return tr.GetComponent<SpriteRenderer>();

            return null;
        }
        private static IEnumerator EaseColorRoutine(Component component, Color targetColor, float totalTime, ArokaCurves.CurvName curvName , float delayTime )
        {
            // 지연 시간 처리
            yield return new WaitForSeconds(delayTime);

            // 애니메이션 커브 가져오기
            AnimationCurve curve = ArokaCurves.GetCurve(curvName);

            // 초기 색상 값 가져오기
            Color initialColor = GetColor(component);
            float elapsed = 0; // 경과 시간 초기화

            while (elapsed < totalTime)
            {
                // 경과 시간 갱신
                elapsed += Time.deltaTime;
                float t = curve.Evaluate(elapsed / totalTime); // 커브를 이용해 시간 비율 계산


                // 색상 보간
                Color newColor = Color.Lerp(initialColor, targetColor, t);

                // 컴포넌트가 여전히 유효한지 확인
                if (component != null)
                {
                    SetColor(component, newColor); // 새로운 색상 설정
                }
                else
                {
                    yield break; // 컴포넌트가 파괴되었다면 코루틴 종료
                }

                yield return null;
            }

            // 최종 색상 설정
            if (component != null)
            {
                SetColor(component, targetColor);
            }
        }

        #endregion

        #region Utility Methods

        private static void StartOrReplaceCoroutine(Transform transform, string key, IEnumerator routine)
        {
            (Transform, string) coroutineKey = (transform, key);
            if (_coroutineMap.TryGetValue(coroutineKey, out UnityEngine.Coroutine currentCoroutine))
            {
                CoroutineUtils.CoroutineUtils.StopCoroutine(currentCoroutine);
                _coroutineMap.Remove(coroutineKey);
            }

            if (transform == null)
            {
                Debug.LogWarning("Transform이 null입니다.");
                return;
            }

            UnityEngine.Coroutine newCoroutine = CoroutineUtils.CoroutineUtils.StartCoroutine(routine);
            _coroutineMap[coroutineKey] = newCoroutine;
        }


        private static void StartOrReplaceColorCoroutine(Component component, string key, IEnumerator routine)
        {
            (Component, string) coroutineKey = (component, key);

            if (_colorCoroutineMap.TryGetValue(coroutineKey, out Coroutine currentCoroutine))
            {
                CoroutineUtils.CoroutineUtils.StopCoroutine(currentCoroutine);
                _colorCoroutineMap.Remove(coroutineKey);
            }

            if (component == null)
            {
                Debug.LogWarning("Component가 null입니다. 코루틴을 시작할 수 없습니다.");
                return;
            }

            Coroutine newCoroutine = CoroutineUtils.CoroutineUtils.StartCoroutine(routine);
            _colorCoroutineMap[coroutineKey] = newCoroutine;
        }

        #endregion
    }


}
