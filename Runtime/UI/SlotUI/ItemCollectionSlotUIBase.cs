using System;
using Devdog.General;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.InventoryPro
{
    public abstract class ItemCollectionSlotUIBase : MonoBehaviour, IPoolable, ISelectHandler, ICollectionItem
    {
        [NonSerialized]
        private uint _index;

        /// <summary>
        ///     Don't ever use this directly! Public because of the lack of friend classes...
        /// </summary>
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
        ///     Index of ItemCollection
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

        /// <summary>
        ///     Repaint this object, make sure to only call it when absolutely necessary as it is a rather heavy method.
        /// </summary>
        public abstract void Repaint();


        public abstract void TriggerContextMenu();

        /// <summary>
        ///     Trigger the unstacking of this wrapper.
        /// </summary>
        /// <param name="toCollection">The collection to unstack the item into.</param>
        /// <param name="toIndex">
        ///     The index the new item should be unstacked to. <b>-1 for the first empty slot available.</b>
        /// </param>
        public abstract void TriggerUnstack(ItemCollectionBase toCollection, int toIndex = -1);

        public abstract void TriggerDrop(bool useRaycast = true);
        public abstract void TriggerUse();

        public virtual void Select()
        {
            var selectable = gameObject.GetComponent<Selectable>();
            if (selectable != null && selectable != this) selectable.Select();
        }

        public virtual void ResetStateForPool()
        {
            //index = 0;
            //itemCollection = null;
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
        }
    }
}