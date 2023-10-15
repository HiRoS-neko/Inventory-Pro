using UnityEngine;

namespace Devdog.InventoryPro
{
    public class ExtendedInventoryItemSerializationModel : InventoryItemSerializationModel
    {
        public override void FromItem(InventoryItemBase item)
        {
            Debug.Log("From extended...");
            base.FromItem(item);
        }

        public override InventoryItemBase ToItem()
        {
            Debug.Log("From extended...");
            return base.ToItem();
        }
    }
}