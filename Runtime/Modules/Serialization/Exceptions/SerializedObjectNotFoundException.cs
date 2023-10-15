using System;

namespace Devdog.InventoryPro
{
    public class SerializedObjectNotFoundException : Exception
    {
        public SerializedObjectNotFoundException(string message)
            : base(message)
        {
        }
    }
}