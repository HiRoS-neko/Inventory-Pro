using System.Linq;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Windows/Inventory bags")]
    public class InventoryBagsUI : ItemCollectionBase, ICollectionExtender
    {
        [SerializeField]
        private uint _initialCollectionSize = 4;


        /// <summary>
        ///     The collection we're extending.
        /// </summary>
        [SerializeField]
        [Required]
        private ItemCollectionBase _extendingCollection;

        private ItemCollectionBase _extenderCollection;
        public override uint initialCollectionSize => _initialCollectionSize;


        protected override void Start()
        {
            base.Start();


            OnAddedItem += (items, amount, collection) =>
            {
                if (items.FirstOrDefault() != null)
                {
                    var bag = items.First() as BagInventoryItem;
                    if (bag != null) bag.NotifyItemEquipped();
                }
            };
            OnRemovedItem += (item, id, slot, amount) =>
            {
                var bag = item as BagInventoryItem;
                if (bag != null) bag.NotifyItemUnEquipped();
            };
        }

        public ItemCollectionBase extendingCollection => _extendingCollection;

        public ItemCollectionBase extenderCollection
        {
            get
            {
                if (_extenderCollection == null) _extenderCollection = GetComponent<ItemCollectionBase>();
                return _extenderCollection;
            }
        }

        public override bool MoveItem(InventoryItemBase item, uint fromSlot, ItemCollectionBase toCollection,
            uint toSlot, bool clearOld, bool doRepaint = true)
        {
            if (item == null)
                return true;

            if (this != toCollection)
            {
                // Moving to another collection       
                var bag = item as BagInventoryItem;
                if (bag == null)
                    return false;

                if (toCollection[toSlot].item != null)
                    return false; // Slot is not empty, swap should have been called?

                var canMove = bag.CanUnEquip(toCollection, toSlot);
                if (canMove == false)
                    return false;

                if (toSlot >= toCollection.items.Length - bag.extendBySlots) return false;
            }

            return base.MoveItem(item, fromSlot, toCollection, toSlot, clearOld, doRepaint);
        }

        public override bool OverrideUseMethod(InventoryItemBase item)
        {
            var bag = item as BagInventoryItem;
            if (bag != null && item.itemCollection == this)
                if (bag.CanUnEquip())
                    InventoryManager.AddItemAndRemove(bag);

            return true;
        }

        public override bool CanSetItem(uint slot, InventoryItemBase item)
        {
            if (base.CanSetItem(slot, item) == false)
                return false;

            if (item == null)
                return true;

            return items[slot].item == null; // Avoid swapping
        }

        public override bool SwapOrMerge(uint slot1, ItemCollectionBase handler2, uint slot2, bool repaint = true)
        {
            return SwapSlots(slot1, handler2, slot2, repaint);
        }
    }
}