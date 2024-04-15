using UnityEngine;

namespace Sayne.Curves
{
    public class CurveManager : MonoBehaviour
    {
        public enum CurvName
        {
            LINEAR,
            EASE_OUT,
            EASE_IN
            // 필요에 따라 추가적인 커브 이름들을 여기에 추가할 수 있습니다.
        }

        private static CurveManager instance;

        public static CurveManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CurveManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("CurveManager");
                        instance = obj.AddComponent<CurveManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public AnimationCurve GetCurve(CurvName curveName)
        {
            switch (curveName)
            {
                case CurvName.LINEAR:
                    return LinearCurve();
                case CurvName.EASE_OUT:
                    return AnimationCurve.EaseInOut(0, 0, 1, 1);
                case CurvName.EASE_IN:
                    return AnimationCurve.EaseInOut(0, 1, 1, 1);
                // 다른 커브들을 필요에 따라 추가할 수 있습니다.
                default:
                    Debug.LogWarning("Unknown curve name: " + curveName);
                    return null;
            }
        }

        private AnimationCurve LinearCurve()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, 0);
            curve.AddKey(1, 1);
            return curve;
        }
    }
}
