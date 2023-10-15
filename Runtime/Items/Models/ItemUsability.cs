using System;

namespace Devdog.InventoryPro
{
    public class ItemUsability
    {
        public string actionName;
        public bool isActive;
        public Action<InventoryItemBase> useItemCallback;

        public ItemUsability(string actionName, Action<InventoryItemBase> useItemCallback)
        {
            this.actionName = actionName;
            this.useItemCallback = useItemCallback;
        }
    }
}