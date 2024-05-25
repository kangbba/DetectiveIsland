using System;

namespace ArokaInspector.Attributes
{
    public class ArokaButtonAttribute : Attribute
    {
        public string Error { get; set; } = "Cannot execute this function.";

        public bool PerformCheck(UnityEngine.Object obj)
        {
            // Here you can add additional checks, for now it's always true
            return true;
        }
    }
}
