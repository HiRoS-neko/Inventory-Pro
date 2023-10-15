using System.Linq;
using UnityEngine;

namespace Devdog.InventoryPro
{
    public class InventoryItemSerializationModel
    {
        public uint amount;
        public string collectionName;

        /// <summary>
        ///     ID is -1 if no item is in the given slot.
        /// </summary>
        public int itemID;

        public StatDecoratorSerializationModel[] stats = new StatDecoratorSerializationModel[0];

        public InventoryItemSerializationModel()
        {
        }

        public InventoryItemSerializationModel(InventoryItemBase item)
        {
            FromItem(item);
        }

        public virtual bool isReference => string.IsNullOrEmpty(collectionName) == false;

        public virtual void FromItem(InventoryItemBase item)
        {
            if (item == null)
            {
                itemID = -1;
                amount = 0;
                collectionName = string.Empty;
                stats = new StatDecoratorSerializationModel[0];
                return;
            }

            itemID = (int)item.ID;
            amount = item.currentStackSize;
            collectionName = item.itemCollection != null ? item.itemCollection.collectionName : string.Empty;
            stats = item.stats.Select(o => new StatDecoratorSerializationModel(o)).ToArray();
        }

        public virtual InventoryItemBase ToItem()
        {
            if (itemID < 0 || itemID > ItemManager.database.items.Length - 1)
                //                DevdogLogger.LogWarning("ItemID is out of range, trying to deserialize item " + itemID);
                return null;

            var item = ItemManager.database.items[itemID];
            var inst = Object.Instantiate(item);
            var s = stats.Select(o => o.ToStat()).ToArray();

            inst.currentStackSize = amount;
            inst.stats = s;
            if (string.IsNullOrEmpty(collectionName) == false)
                inst.itemCollection = ItemCollectionBase.FindByName(collectionName);

            return inst;
        }
    }
}