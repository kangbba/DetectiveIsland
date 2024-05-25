using System;
using UnityEngine;

namespace ArokaInspector.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionName { get; private set; }
        public object ConditionValue { get; private set; }
        public bool HasConditionValue { get; private set; }

        public ShowIfAttribute(string conditionName)
        {
            ConditionName = conditionName;
            HasConditionValue = false;
        }

        public ShowIfAttribute(string conditionName, object conditionValue)
        {
            ConditionName = conditionName;
            ConditionValue = conditionValue;
            HasConditionValue = true;
        }
    }
}
