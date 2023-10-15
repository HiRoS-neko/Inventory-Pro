using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [RequireComponent(typeof(ItemCollectionBase))]
    public class CollectionPopulator : MonoBehaviour
    {
        public ItemAmountRow[] items = Array.Empty<ItemAmountRow>();

        /// <summary>
        ///     This will ignore layout sizes, and force the items into the slots.
        /// </summary>
        public bool useForceSet;

        private InventoryItemBase[] _items = Array.Empty<InventoryItemBase>();

        protected void Awake()
        {
            _items = InventoryItemUtility.RowsToItems(items, true);
            for (var i = 0; i < _items.Length; i++)
            {
                _items[i].transform.SetParent(transform);
                _items[i].gameObject.SetActive(false);
            }
        }


        protected void Start()
        {
            var col = GetComponent<ItemCollectionBase>();
            if (col == null)
            {
                DevdogLogger.LogError("CollectionPopulator can only be used on a collection.", transform);
                return;
            }

            if (useForceSet)
                for (uint i = 0; i < _items.Length; i++)
                    col.SetItem(i, _items[i], true);
            else
                col.AddItems(_items);
        }
    }
}