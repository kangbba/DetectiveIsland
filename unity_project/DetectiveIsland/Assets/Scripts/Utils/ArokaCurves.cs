using System.Collections.Generic;
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
            EXPONENTIAL,
            SINE,
            QUADRATIC,
            CUBIC
        }

        private static Dictionary<CurvName, AnimationCurve> curves = new Dictionary<CurvName, AnimationCurve>()
        {
            { CurvName.LINEAR, AnimationCurve.Linear(0, 0, 1, 1) },
            { CurvName.EASE_OUT, AnimationCurve.EaseInOut(0, 0, 1, 1) },
            { CurvName.EASE_IN, AnimationCurve.EaseInOut(0, 1, 1, 1) },
            { CurvName.EXPONENTIAL, ExponentialCurve() },
            { CurvName.SINE, SineCurve() },
            { CurvName.QUADRATIC, QuadraticCurve() },
            { CurvName.CUBIC, CubicCurve() }
        };

        public static AnimationCurve GetCurve(CurvName curveName)
        {
            if (curves.TryGetValue(curveName, out AnimationCurve curve))
            {
                return curve;
            }
            else
            {
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
