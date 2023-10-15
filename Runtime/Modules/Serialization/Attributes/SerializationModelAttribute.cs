using System;

namespace Devdog.InventoryPro
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class SerializationModelAttribute : Attribute
    {
        public Type type;

        public SerializationModelAttribute(Type type)
        {
            this.type = type;
        }
    }
}