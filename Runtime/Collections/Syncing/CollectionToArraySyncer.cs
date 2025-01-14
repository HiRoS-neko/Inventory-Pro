﻿using System.Collections.Generic;
using UnityEngine;

namespace Devdog.InventoryPro
{
    public class CollectionToArraySyncer
    {
        public CollectionToArraySyncer(ItemCollectionBase fromCollection, InventoryItemBase[] toArr)
        {
            this.fromCollection = fromCollection;
            this.toArr = toArr;
        }

        public ItemCollectionBase fromCollection { get; }
        public InventoryItemBase[] toArr { get; }

        public void StartSyncing()
        {
            RegisterEvents();
        }

        public void StopSyncing()
        {
            UnRegisterEvents();
        }

        private void RegisterEvents()
        {
            Debug.Log("Start syncing events");

            fromCollection.OnAddedItem += OnAddedItem;
            fromCollection.OnRemovedItem += OnRemovedItem;
            fromCollection.OnUsedItem += OnUsedItem;
            fromCollection.OnSorted += OnSorted;
            fromCollection.OnRemovedReference += OnRemovedReference;
            fromCollection.OnSwappedItems += OnSwappedItems;
            fromCollection.OnUnstackedItem += OnUnstackedItem;
            fromCollection.OnMergedSlots += OnMergedSlots;
        }

        private void UnRegisterEvents()
        {
            Debug.Log("Removing sync events");

            fromCollection.OnAddedItem -= OnAddedItem;
            fromCollection.OnRemovedItem -= OnRemovedItem;
            fromCollection.OnUsedItem -= OnUsedItem;
            fromCollection.OnSorted -= OnSorted;
            fromCollection.OnRemovedReference -= OnRemovedReference;
            fromCollection.OnSwappedItems -= OnSwappedItems;
            fromCollection.OnUnstackedItem -= OnUnstackedItem;
            fromCollection.OnMergedSlots -= OnMergedSlots;
        }


        private void OnUnstackedItem(ItemCollectionBase fromColl, uint startslot, ItemCollectionBase toCollection,
            uint endslot, uint amount)
        {
            toArr[startslot] = fromColl[startslot].item;

            if (fromColl == toCollection)
                toArr[endslot] = fromColl[endslot].item;
        }

        private void OnSwappedItems(ItemCollectionBase from, uint fromSlot, ItemCollectionBase to, uint toSlot)
        {
            if (from == fromCollection)
                toArr[fromSlot] = fromCollection[fromSlot].item;

            if (to == fromCollection)
                toArr[toSlot] = fromCollection[toSlot].item;
        }

        private void OnRemovedReference(InventoryItemBase item, uint slot)
        {
            toArr[slot] = fromCollection[slot].item;
        }

        private void OnSorted()
        {
            var items = new InventoryItemBase[fromCollection.items.Length];
            for (var i = 0; i < items.Length; i++)
                toArr[i] = items[i];
        }

        private void OnUsedItem(InventoryItemBase item, uint itemid, uint slot, uint amount)
        {
            toArr[slot] = fromCollection[slot].item;
        }

        private void OnRemovedItem(InventoryItemBase item, uint itemid, uint slot, uint amount)
        {
            toArr[slot] = fromCollection[slot].item;
        }

        private void OnAddedItem(IEnumerable<InventoryItemBase> inventoryItemBases, uint amount,
            bool camefromcollection)
        {
            foreach (var item in inventoryItemBases)
                toArr[item.index] = fromCollection[item.index].item;
        }

        private void OnMergedSlots(ItemCollectionBase from, uint fromSlot, ItemCollectionBase to, uint toSlot)
        {
            if (from == fromCollection)
                toArr[fromSlot] = fromCollection[fromSlot].item;

            if (to == fromCollection)
                toArr[toSlot] = fromCollection[toSlot].item;
        }
    }
}