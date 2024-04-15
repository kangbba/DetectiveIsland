using System.Collections;
using System.Collections.Generic;
using Sayne.Curves;
using UnityEngine;
using UnityEngine.UI;

public class ArokaTransform : MonoBehaviour
{
    Coroutine nowPosRoutine;
    Coroutine nowRotRoutine;
    Coroutine nowScaleRoutine;
    Coroutine nowActiveRoutine;
    Coroutine nowColorRoutine;


    #region POSITION
    public void SetPos(Vector3 targetPos, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0)
    {
        SetPosStop();
        nowPosRoutine = StartCoroutine(SetPosRoutine(true, transform.parent, targetPos, totalTime, curvName, delayTime));
    }
    public void SetLocalPos(Vector3 targetPos, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0)
    {
        SetPosStop();
        nowPosRoutine = StartCoroutine(SetPosRoutine(false, transform.parent, targetPos, totalTime, curvName, delayTime));
    }
    public void SetLocalPosWithParent(Vector3 targetPos, Transform parent, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0)
    {
        SetPosStop();
        nowPosRoutine = StartCoroutine(SetPosRoutine(false, parent, targetPos, totalTime, curvName, delayTime));
    }
    public void SetPosWithCurv(Vector3 targetPos, float totalTime, CurveManager.CurvName curvName, float delayTime = 0)
    {
        SetPosStop();
        nowPosRoutine = StartCoroutine(SetPosRoutine(true, transform.parent, targetPos, totalTime, curvName, delayTime));
    }
    public void SetLocalPosWithCurv(Vector3 targetPos, float totalTime, CurveManager.CurvName curvName, float delayTime = 0)
    {
        SetPosStop();
        nowPosRoutine = StartCoroutine(SetPosRoutine(false, transform.parent, targetPos, totalTime, curvName, delayTime));
    }
    public void SetLocalPosWithCurv_WithParent(Vector3 targetPos, Transform parent, float totalTime, CurveManager.CurvName curvName, float delayTime = 0)
    {
        SetPosStop();
        nowPosRoutine = StartCoroutine(SetPosRoutine(false, parent, targetPos, totalTime, curvName, delayTime));
    }

    public void SetPosStop()
    {
        if (nowPosRoutine != null)
        {
            StopCoroutine(nowPosRoutine);
        }
    }

    //MOVE 함수
    private IEnumerator SetPosRoutine(bool isWorld, Transform parent, Vector3 targetLocalPos, float totalTime, CurveManager.CurvName curvName, float delayTime = 0f)
    {
        yield return new WaitForSeconds(delayTime);
        transform.SetParent(parent);
        AnimationCurve animCurv = CurveManager.Instance.GetCurve(curvName);

        Vector3 initialLocalPos = isWorld ? transform.position : transform.localPosition;
        float accumTime = 0;
        while (transform)
        {
            accumTime += Time.deltaTime;
            float perone = totalTime == 0 ? 1f : Mathf.Clamp01(accumTime / totalTime);
            float curvPerone = animCurv.Evaluate(perone);


            if (isWorld)
            {
                transform.position = Vector3.Lerp(initialLocalPos, targetLocalPos, curvPerone);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(initialLocalPos, targetLocalPos, curvPerone);
            }
            if (perone >= 1)
            {
                break;
            }
            yield return null;
        }
    }
    #endregion

    //MOVE 함수
    #region Scale
    public void SetLocalScale(Vector3 targetScale, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        SetLocalScaleStop();
        nowScaleRoutine = StartCoroutine(SetScaleRoutine(targetScale, transform.parent, totalTime, curvName, delayTime));
    }

    public void SetLocalScaleWithParent(Vector3 targetScale, Transform parent, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        SetLocalScaleStop();
        nowScaleRoutine = StartCoroutine(SetScaleRoutine(targetScale, parent, totalTime, curvName, delayTime));
    }

    public void SetLocalScaleStop()
    {
        if (nowScaleRoutine != null)
        {
            StopCoroutine(nowScaleRoutine);
        }
    }

    private IEnumerator SetScaleRoutine(Vector3 targetScale, Transform parent, float totalTime, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        yield return new WaitForSeconds(delayTime);
        transform.SetParent(parent);
        AnimationCurve animCurv = CurveManager.Instance.GetCurve(curvName);
        Vector3 initialLocalScale = transform.localScale;
        float accumTime = 0;
        while (transform)
        {
            accumTime += Time.deltaTime;
            float perone = totalTime == 0 ? 1f : Mathf.Clamp01(accumTime / totalTime);
            float curvPerone = animCurv.Evaluate(perone);
            transform.localScale = Vector3.LerpUnclamped(initialLocalScale, targetScale, curvPerone);
            if (perone >= 1)
            {
                break;
            }
            yield return null;
        }
    }
    #endregion


    #region Rotation

    public void SetRot(Quaternion targetRot, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        SetRotStop();
        nowRotRoutine = StartCoroutine(SetRotRoutine(true, targetRot, totalTime, curvName, delayTime));
    }
    public void SetLocalRot(Quaternion targetRot, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        SetRotStop();
        nowRotRoutine = StartCoroutine(SetRotRoutine(false, targetRot, totalTime, curvName, delayTime));
    }


    public void SetRotEuler(Vector3 targetEuler, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        SetRot(Quaternion.Euler(targetEuler), totalTime, curvName , delayTime);
    }
    public void SetLocalRotEuler(Vector3 targetEuler, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        SetLocalRot(Quaternion.Euler(targetEuler), totalTime, curvName, delayTime);
    }

    public void SetRotInfinity(Vector3 dirVec, float speed, Space space = Space.Self)
    {
        SetRotStop();
        nowRotRoutine = StartCoroutine(SetRotInfinityRoutine(dirVec, speed, space));
    }
    public void SetRotStop()
    {
        if (nowRotRoutine != null)
        {
            StopCoroutine(nowRotRoutine);
        }
    }

    #region Color

    public void SetColor(Color targetColor, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        SetColorStop();
        if(totalTime == 0f){
            Image image = GetComponent<Image>();
            image.color = targetColor;
        }
        else{
           nowColorRoutine = StartCoroutine(SetColorRoutine(targetColor, totalTime, curvName, delayTime));
        }
    }

    public void SetColorStop()
    {
        if (nowColorRoutine != null)
        {
            StopCoroutine(nowColorRoutine);
        }
    }

    private IEnumerator SetColorRoutine(Color targetColor, float totalTime, CurveManager.CurvName curvName, float delayTime = 0f)
    {
        yield return new WaitForSeconds(delayTime);
        Image image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("ArokaTransform: This function can only be used with UI elements that have Image component.");
            yield break;
        }
        Color initialColor = image.color;
        AnimationCurve animCurv = CurveManager.Instance.GetCurve(curvName);
        float accumTime = 0;
        while (image)
        {
            accumTime += Time.deltaTime;
            float perone = totalTime == 0 ? 1f : Mathf.Clamp01(accumTime / totalTime);
            float curvPerone = animCurv.Evaluate(perone);
            image.color = Color.Lerp(initialColor, targetColor, curvPerone);
            if (perone >= 1)
            {
                break;
            }
            yield return null;
        }
    }

    #endregion

    private IEnumerator SetRotRoutine(bool isWorld, Quaternion targeRot, float totalTime, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        yield return new WaitForSeconds(delayTime);
        AnimationCurve animCurv = CurveManager.Instance.GetCurve(curvName);
        Quaternion initialRot = isWorld ? transform.rotation : transform.localRotation;
        float accumTime = 0;
        while (transform)
        {
            accumTime += Time.deltaTime;
            float perone = (totalTime == 0f) ? 1f : Mathf.Clamp01(accumTime / totalTime);
            float curvPerone = animCurv.Evaluate(perone);
            if (isWorld)
            {
                transform.rotation = Quaternion.Lerp(initialRot, targeRot, curvPerone);
            }
            else
            {
                transform.localRotation = Quaternion.Lerp(initialRot, targeRot, curvPerone);
            }
            if (perone >= 1)
            {
                break;
            }
            yield return null;
        }
    }


    private IEnumerator SetRotInfinityRoutine(Vector3 direction, float speed, Space space = Space.Self, bool isFixedUpdate = false)
    {
        if (isFixedUpdate)
        {
            WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
            while (gameObject)
            {
                transform.Rotate(direction, Time.deltaTime * speed, space);
                yield return fixedUpdate;
            }
        }
        else
        {
            while (gameObject)
            {
                transform.Rotate(direction, Time.deltaTime * speed, space);
                yield return null;
            }
        }
    }


    #endregion

    #region UI_Position

    public void SetAnchoredPos(Vector2 targetPos, float totalTime = 0f, CurveManager.CurvName curvName = CurveManager.CurvName.EASE_OUT, float delayTime = 0f)
    {
        SetAnchoredPosStop();
        nowPosRoutine = StartCoroutine(SetAnchoredPosRoutine(targetPos, totalTime, curvName, delayTime));
    }

    public void SetAnchoredPosStop()
    {
        if (nowPosRoutine != null)
        {
            StopCoroutine(nowPosRoutine);
        }
    }

    private IEnumerator SetAnchoredPosRoutine(Vector2 targetPos, float totalTime, CurveManager.CurvName curvName, float delayTime = 0f)
    {
        yield return new WaitForSeconds(delayTime);
        RectTransform rectTransform = transform as RectTransform;
        if (rectTransform == null)
        {
            Debug.LogError("ArokaTransform: This function can only be used with UI elements.");
            yield break;
        }
        AnimationCurve animCurv = CurveManager.Instance.GetCurve(curvName);
        Vector2 initialAnchoredPos = rectTransform.anchoredPosition;
        float accumTime = 0;
        while (rectTransform)
        {
            accumTime += Time.deltaTime;
            float perone = totalTime == 0 ? 1f : Mathf.Clamp01(accumTime / totalTime);
            float curvPerone = animCurv.Evaluate(perone);
            rectTransform.anchoredPosition = Vector2.Lerp(initialAnchoredPos, targetPos, curvPerone);
            if (perone >= 1)
            {
                break;
            }
            yield return null;
        }
    }

    #endregion


    #region TRANSFORM
    public void SetTransform(Transform parent, Vector3 localPos, Quaternion localRot, Vector3 localScale, float totalTime, CurveManager.CurvName curvName, float delayTime = 0f)
    {
        SetTransformStop();
        transform.SetParent(parent);
        SetLocalPos(localPos, totalTime, curvName, delayTime);
        SetLocalRot(localRot, totalTime, curvName, delayTime);
        SetLocalScale(localScale, totalTime, curvName, delayTime);
    }
    public void SetTransformStop()
    {
        SetPosStop();
        SetLocalScaleStop();
        SetRotStop();
    }
    #endregion


    #region ETC
    public void SetActiveWithDelay(bool b, float delay, Transform afterParent)
    {
        if (nowActiveRoutine != null)
        {
            StopCoroutine(nowActiveRoutine);
        }
        nowActiveRoutine = StartCoroutine(SetActiveWithDelayRoutine(b, delay, afterParent));
    }
    public void SetActiveWithDelay(bool b, float delay)
    {
        if (nowActiveRoutine != null)
        {
            StopCoroutine(nowActiveRoutine);
        }
        nowActiveRoutine = StartCoroutine(SetActiveWithDelayRoutine(b, delay, transform.parent));
    }
    private IEnumerator SetActiveWithDelayRoutine(bool b, float delay, Transform afterParent)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(b);
        transform.SetParent(afterParent);
    }
    #endregion




}