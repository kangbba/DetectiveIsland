using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using Aroka.Curves;
using UnityEngine;
using UnityEngine.UI;

namespace Aroka.EaseUtils{
    public static class ArokaEaseUtils
    {
        private static Dictionary<(Transform, string), Coroutine> _coroutineMap = new Dictionary<(Transform, string), Coroutine>();
        private static Dictionary<(Component, string), Coroutine> _colorCoroutineMap = new Dictionary<(Component, string), Coroutine>();


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
            if(rectTransform == null){
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

        public static void EaseSpriteRendererColor(this SpriteRenderer spriteRenderer, Color targetColor, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                spriteRenderer.color = targetColor;
                return;
            }
            StartOrReplaceColorCoroutine(spriteRenderer, "spriteColor", EaseColorRoutine(spriteRenderer, targetColor, totalTime, curvName, delayTime));
        }

        public static void EaseImageColor(this Image image, Color targetColor, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                image.color = targetColor;
                return;
            }
            StartOrReplaceColorCoroutine(image, "imageColor", EaseColorRoutine(image, targetColor, totalTime, curvName, delayTime));
        }

        public static void EaseMeshRendererColor(this MeshRenderer meshRenderer, Color targetColor, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            if (totalTime == 0)
            {
                meshRenderer.material.color = targetColor;
                return;
            }
            StartOrReplaceColorCoroutine(meshRenderer, "meshRendererColor", EaseColorRoutine(meshRenderer, targetColor, totalTime, curvName, delayTime));
        }

        private static IEnumerator EaseColorRoutine(Component component, Color targetColor, float totalTime, ArokaCurves.CurvName curvName = ArokaCurves.CurvName.EASE_OUT, float delayTime = 0)
        {
            yield return new WaitForSeconds(delayTime);
            AnimationCurve curve = ArokaCurves.GetCurve(curvName);
            Color initialColor = Color.white;

            if (component is SpriteRenderer)
                initialColor = ((SpriteRenderer)component).color;
            else if (component is Image)
                initialColor = ((Image)component).color;
            else if (component is MeshRenderer)
                initialColor = ((MeshRenderer)component).material.color;

            float elapsed = 0;

            while (elapsed < totalTime)
            {
                elapsed += Time.deltaTime;
                float t = curve.Evaluate(elapsed / totalTime);
                Color newColor = Color.Lerp(initialColor, targetColor, t);

                if (component is SpriteRenderer)
                    ((SpriteRenderer)component).color = newColor;
                else if (component is Image)
                    ((Image)component).color = newColor;
                else if (component is MeshRenderer)
                    ((MeshRenderer)component).material.color = newColor;

                yield return null;
            }

            if (component is SpriteRenderer)
                ((SpriteRenderer)component).color = targetColor;
            else if (component is Image)
                ((Image)component).color = targetColor;
            else if (component is MeshRenderer)
                ((MeshRenderer)component).material.color = targetColor;
        }

        #endregion


        #region Utility Methods

        private static void StartOrReplaceCoroutine(Transform transform, string key, IEnumerator routine)
        {
            (Transform, string) coroutineKey = (transform, key);
            if (_coroutineMap.TryGetValue(coroutineKey, out Coroutine currentCoroutine))
            {
                ArokaCoroutineUtils.StopCoroutine(currentCoroutine);
                _coroutineMap.Remove(coroutineKey);
            }

            Coroutine newCoroutine = ArokaCoroutineUtils.StartCoroutine(routine);
            _coroutineMap[coroutineKey] = newCoroutine;
        }
        private static void StartOrReplaceColorCoroutine(Component component, string key, IEnumerator routine)
            {
                (Component, string) coroutineKey = (component, key);
                if (_colorCoroutineMap.TryGetValue(coroutineKey, out Coroutine currentCoroutine))
                {
                    ArokaCoroutineUtils.StopCoroutine(currentCoroutine);
                    _colorCoroutineMap.Remove(coroutineKey);
                }

                Coroutine newCoroutine = ArokaCoroutineUtils.StartCoroutine(routine);
                _colorCoroutineMap[coroutineKey] = newCoroutine;
            }

        #endregion
    }

}

