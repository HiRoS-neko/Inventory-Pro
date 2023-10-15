using System;

namespace Devdog.InventoryPro
{
    public class DatabaseNotFoundException : Exception
    {
        public DatabaseNotFoundException(string msg)
            : base(msg)
        {
        }
    }
}