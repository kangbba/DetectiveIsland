using UnityEngine;

namespace ArokaInspector.Attributes
{
    public class HideIfAttribute : PropertyAttribute
    {
        public string ConditionName { get; private set; }

        public HideIfAttribute(string conditionName)
        {
            ConditionName = conditionName;
        }
    }
}
