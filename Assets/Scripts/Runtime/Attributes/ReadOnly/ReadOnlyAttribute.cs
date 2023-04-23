using UnityEngine;

namespace PushCar.Runtime.Attributes.ReadOnly
{
    public enum ReadOnlyType
    {
        FULLY_DISABLED,
        EDITABLE_RUNTIME,
        EDITABLE_EDITOR,
    }

    public class ReadOnlyAttribute : PropertyAttribute
    {
        public readonly ReadOnlyType runtimeOnly;

        public ReadOnlyAttribute(ReadOnlyType runtimeOnly = ReadOnlyType.FULLY_DISABLED)
        {
            this.runtimeOnly = runtimeOnly;
        }
    }

    public class BeginReadOnlyAttribute : PropertyAttribute { }

    public class EndReadOnlyAttribute : PropertyAttribute { }
}