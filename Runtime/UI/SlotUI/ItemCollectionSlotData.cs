using System;
using Devdog.General;

namespace Devdog.InventoryPro
{
    /// <summary>
    ///     Used as data object, doesn't show any info.
    /// </summary>
    public class ItemCollectionSlotData : ICollectionItem
    {
        [NonSerialized]
        private uint _index;

        [NonSerialized]
        private InventoryItemBase _item;

        [NonSerialized]
        private ItemCollectionBase _itemCollection;

        /// <summary>
        ///     The item we're wrapping.
        /// </summary>
        public virtual InventoryItemBase item
        {
            get => _item;
            set
            {
                _item = value;
                if (_item != null && itemCollection != null)
                    if (itemCollection.useReferences == false)
                    {
                        _item.itemCollection = itemCollection;
                        _item.index = index;
                    }
            }
        }

        /// <summary>
        ///     Our index in ItemCollection
        /// </summary>
        public virtual uint index
        {
            get => _index;
            set
            {
                _index = value;
                if (item != null && itemCollection && itemCollection.useReferences == false)
                    item.index = value;
            }
        }

        /// <summary>
        ///     The collection that holds this item.
        ///     this == itemCollection[index]
        /// </summary>
        public virtual ItemCollectionBase itemCollection
        {
            get => _itemCollection;
            set
            {
                _itemCollection = value;
                if (item != null && itemCollection != null && itemCollection.useReferences == false)
                    item.itemCollection = value;
            }
        }

        public void TriggerContextMenu()
        {
            DevdogLogger.LogWarning("Trigger actions can't be used on data wrapper.");
        }

        // <inheritdoc />
        public void TriggerUnstack(ItemCollectionBase toCollection, int toIndex = -1)
        {
            DevdogLogger.LogWarning("Trigger actions can't be used on data wrapper.");
        }

        public void TriggerDrop(bool useRaycast = true)
        {
            DevdogLogger.LogWarning("Trigger actions can't be used on data wrapper.");
        }

        public void TriggerUse()
        {
            DevdogLogger.LogWarning("Trigger actions can't be used on data wrapper.");
        }


        /// <summary>
        ///     Repaints the item icon and amount.
        /// </summary>
        public void Repaint()
        {
        }

        public void Select()
        {
        }
    }
}