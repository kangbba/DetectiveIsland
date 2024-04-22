using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using Aroka.Curves;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aroka.EaseUtils
{
    public static class ArokaEaseUtils
    {
        private static Dictionary<(Transform, string), UnityEngine.Coroutine> _coroutineMap = new Dictionary<(Transform, string), UnityEngine.Coroutine>();
        private static Dictionary<(Component, string), UnityEngine.Coroutine> _colorCoroutineMap = new Dictionary<(Component, string), UnityEngine.Coroutine>();

        #region Position and Movement Extensions

        public static void EasePos(this Transform transform, Vector3 targetPos, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                transform.position = targetPos;
                return;
            }
            StartOrReplaceCoroutine(transform, "position", EasePosRoutine(transform, targetPos, totalTime, curvName, delayTime, isWorld: true));
        }

        public static void EaseLocalPos(this Transform transform, Vector3 targetPos, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
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

        public static void EaseAnchoredPos(this Image image, Vector2 targetPos, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            RectTransform rectTransform = image.rectTransform;
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

        private static IEnumerator EaseAnchoredPositionRoutine(RectTransform rectTransform, Vector2 targetPos, float totalTime, ArokaCurves.CurvName curvName, float delayTime)
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
        public static void EaseColor(this Component component, Color targetColor, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0){
            string componentKey = GetColorComponentKey(component);
            if (totalTime == 0)
            {
                SetColor(component, targetColor);
                return;
            }
            StartOrReplaceColorCoroutine(component, componentKey, EaseColorRoutine(component, targetColor, totalTime, curvName, delayTime));
        }

        private static IEnumerator EaseColorRoutine(Component component, Color targetColor, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            yield return new WaitForSeconds(delayTime);
            AnimationCurve curve = ArokaCurves.GetCurve(curvName);
            Color initialColor = GetColor(component);
            float elapsed = 0;
            while (elapsed < totalTime)
            {
                elapsed += Time.deltaTime;
                float t = curve.Evaluate(elapsed / totalTime);
                Color newColor = Color.Lerp(initialColor, targetColor, t);

                // GameObject가 파괴되지 않았는지 확인 후 컬러 설정
                if (component != null)
                {
                    SetColor(component, newColor);
                }
                else
                {
                    // GameObject가 파괴되었다면 코루틴 종료
                    yield break;
                }

                yield return null;
            }

            // 코루틴 종료 후 최종 색상 설정
            if (component != null)
            {
                SetColor(component, targetColor);
            }
        }
        private static string GetColorComponentKey(Component component)
            {
                if (component is SpriteRenderer)
                {
                    return "spriteColor";
                }
                else if (component is TextMeshProUGUI)
                {
                    return "textMeshProColor";
                }
                else if (component is Image)
                {
                    return "imageColor";
                }
                else if (component is MeshRenderer)
                {
                    return "meshRendererColor";
                }
                else
                {
                    // 기본 키 또는 예외 처리
                    Debug.LogWarning("GetColorComponentKey called on a component that does not handle colors.");
                    return null;
                }
            }
        public static Color GetColor(Component component)
        {
            if (component is SpriteRenderer spriteRenderer)
            {
                return spriteRenderer.color;
            }
            else if (component is TextMeshProUGUI textMeshPro)
            {
                return textMeshPro.color;
            }
            else if (component is Image image)
            {
                return image.color;
            }
            else if (component is MeshRenderer meshRenderer)
            {
                // 주의: MeshRenderer는 Mesh에 적용된 첫 번째 Material의 색상을 반환합니다.
                // 다중 Material을 사용하는 경우, 이 방법은 첫 번째 Material에만 적용됩니다.
                return meshRenderer.material.color;
            }
            else
            {
                Debug.LogWarning("GetColor called on a component that does not have a color property.");
                return default(Color); // 색상 속성이 없는 경우 기본 색상 반환
            }
        }
        public static void SetColor(Component component, Color targetColor)
        {
            if (component is SpriteRenderer spriteRenderer)
            {
                spriteRenderer.color = targetColor;
            }
            else if (component is TextMeshProUGUI textMeshPro)
            {
                textMeshPro.color = targetColor;
            }
            else if (component is Image image)
            {
                image.color = targetColor;
            }
            else if (component is MeshRenderer meshRenderer)
            {
                meshRenderer.material.color = targetColor;
            }
            else
            {
                Debug.LogWarning("SetColor called on a component that does not have a color property.");
            }
        }
        public static System.Type GetComponentType(this Transform transform)
        {
        if (transform.GetComponent<SpriteRenderer>() != null)
        {
            return typeof(SpriteRenderer);
        }
        else if (transform.GetComponent<TextMeshProUGUI>() != null)
        {
            return typeof(TextMeshProUGUI);
        }
        else if (transform.GetComponent<Image>() != null)
        {
            return typeof(Image);
        }
        else if (transform.GetComponent<MeshRenderer>() != null)
        {
            return typeof(MeshRenderer);
        }
        else
        {
            return null; // 색상 속성을 가진 컴포넌트가 없는 경우
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
            if (_colorCoroutineMap.TryGetValue(coroutineKey, out UnityEngine.Coroutine currentCoroutine))
            {
                CoroutineUtils.CoroutineUtils.StopCoroutine(currentCoroutine);
                _colorCoroutineMap.Remove(coroutineKey);
            }

            if (component == null)
            {
                Debug.LogWarning("Component가 null입니다.");
                return;
            }

            UnityEngine.Coroutine newCoroutine = CoroutineUtils.CoroutineUtils.StartCoroutine(routine);
            _colorCoroutineMap[coroutineKey] = newCoroutine;
        }

        #endregion
    }
}
