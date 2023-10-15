using System.Collections.Generic;
using System.Linq;
using Devdog.General;

namespace Devdog.InventoryPro
{
    /// <summary>
    ///     Used to represent a bag that extend a collection.
    /// </summary>
    public class BagInventoryItem : InventoryItemBase
    {
        public uint extendBySlots = 4;
        public AudioClipInfo playOnEquip;

        //public bool isEquipped { get; protected set; }

        public override LinkedList<ItemInfoRow[]> GetInfo()
        {
            var list = base.GetInfo();

            list.AddFirst(new[]
            {
                new ItemInfoRow("Extra slots", extendBySlots.ToString())
            });

            return list;
        }

        public override int Use()
        {
            var used = base.Use();
            if (used < 0)
                return used; // Item cannot be used

            var extenderCollection = GetExtenderCollection();
            if (extenderCollection == null)
            {
                DevdogLogger.LogWarning("Can't use bag, no collection found with interface " +
                                        typeof(ICollectionExtender));
                return -2;
            }

            var added = extenderCollection.extenderCollection.AddItemAndRemove(this);
            if (added) return 1;

            return -2;
        }

        private ICollectionExtender GetExtenderCollection()
        {
            var player = PlayerManager.instance.currentPlayer;
            if (player == null)
            {
                DevdogLogger.LogWarning("No current player, can't get collections.");
                return null;
            }

            var collectionExtenders = FindObjectsOfType<ItemCollectionBase>();
            var interfaces = collectionExtenders.OfType<ICollectionExtender>();
            foreach (var i in interfaces)
                if (player.InventoryPlayer().inventoryCollections.Contains(i.extendingCollection))
                    return i;

            return null;
        }

        public void NotifyItemEquipped()
        {
            NotifyItemUsed(1, false);

            var extenderCollection = GetExtenderCollection();
            if (extenderCollection == null)
            {
                DevdogLogger.LogWarning("Can't use bag, no inventory found with extender collection");
                return;
            }

            // Used from some collection, equip
            var added = extenderCollection.extendingCollection.AddSlots(extendBySlots);
            if (added) AudioManager.AudioPlayOneShot(playOnEquip);
        }

        public bool NotifyItemUnEquipped()
        {
            var extenderCollection = GetExtenderCollection();
            if (extenderCollection == null)
            {
                DevdogLogger.LogWarning("Can't unequip bag, no inventory found with extender collection");
                return false;
            }

            return extenderCollection.extendingCollection.RemoveSlots(extendBySlots);
        }

        public bool CanUnEquip()
        {
            var extenderCollection = GetExtenderCollection();
            if (extenderCollection == null)
            {
                DevdogLogger.LogWarning("Can't unequip bag, no inventory found with extender collection");
                return false;
            }

            var slot = extenderCollection.extendingCollection.FindFirstEmptySlot(this);
            if (slot == -1) return false;

            return CanUnEquip(extenderCollection.extendingCollection, (uint)slot);
        }

        public bool CanUnEquip(ItemCollectionBase toCollection, uint toIndex)
        {
            var extenderCollection = GetExtenderCollection();
            if (extenderCollection == null)
            {
                DevdogLogger.LogWarning("Can't unequip bag, no inventory found with extender collection");
                return false;
            }

            // If the item is placed inside the slots it's supposed to rmove it should fail...
            var clearSlots = extendBySlots + layoutSize;
            var c = extenderCollection.extendingCollection;
            if (toIndex > toCollection.items.Length - clearSlots) return false;

            return c.CanRemoveSlots(clearSlots);
        }
    }
}