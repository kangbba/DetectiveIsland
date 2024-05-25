using UnityEngine;

namespace ArokaInspector.Attributes
{
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionName { get; private set; }

        public ShowIfAttribute(string conditionName)
        {
            ConditionName = conditionName;
        }
    }
}
