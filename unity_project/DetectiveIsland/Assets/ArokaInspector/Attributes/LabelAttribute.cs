using System;
using UnityEngine;

namespace ArokaInspector.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class LabelAttribute : PropertyAttribute
    {
        public string LabelText { get; private set; }

        public LabelAttribute(string labelText)
        {
            LabelText = labelText;
        }
    }
}
