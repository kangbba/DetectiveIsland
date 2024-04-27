using UnityEngine;

namespace Aroka.Curves
{
    public static class ArokaCurves
    {
        public enum CurvName
        {
            LINEAR,
            EASE_OUT,
            EASE_IN,
            EASE_IN_AND_OUT,
            EXPONENTIAL,
            SINE,
            QUADRATIC,
            CUBIC
        }

        public static AnimationCurve GetCurve(CurvName curveName)
        {
            switch (curveName)
            {
                case CurvName.LINEAR:
                    return AnimationCurve.Linear(0, 0, 1, 1);
                case CurvName.EASE_OUT:
                    return new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1, -2, -2));
                case CurvName.EASE_IN:
                    return new AnimationCurve(new Keyframe(0, 0, 2, 2), new Keyframe(1, 1));
                case CurvName.EASE_IN_AND_OUT:
                    return AnimationCurve.EaseInOut(0, 0, 1, 1);
                case CurvName.EXPONENTIAL:
                    return ExponentialCurve();
                case CurvName.SINE:
                    return SineCurve();
                case CurvName.QUADRATIC:
                    return QuadraticCurve();
                case CurvName.CUBIC:
                    return CubicCurve();
                default:
                    Debug.LogWarning("Unknown curve name: " + curveName);
                    return null;
            }
        }

        private static AnimationCurve ExponentialCurve()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, 0);
            curve.AddKey(0.25f, 0.1f);
            curve.AddKey(0.5f, 0.3f);
            curve.AddKey(0.75f, 0.7f);
            curve.AddKey(1, 1);
            return curve;
        }

        private static AnimationCurve SineCurve()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, 0);
            curve.AddKey(0.5f, 0.5f);
            curve.AddKey(1, 1);
            return curve;
        }

        private static AnimationCurve QuadraticCurve()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, 0);
            curve.AddKey(0.5f, 0.25f);
            curve.AddKey(1, 1);
            return curve;
        }

        private static AnimationCurve CubicCurve()
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, 0);
            curve.AddKey(0.3f, 0.1f);
            curve.AddKey(0.7f, 0.9f);
            curve.AddKey(1, 1);
            return curve;
        }
    }
}
