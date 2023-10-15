using System;

namespace Devdog.InventoryPro
{
    public interface IItemsLoader
    {
        void LoadItems(Action<object> callback);
    }
}