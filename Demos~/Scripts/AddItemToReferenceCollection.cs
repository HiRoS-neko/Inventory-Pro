﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Devdog.InventoryPro.Demo
{
    public class AddItemToReferenceCollection : MonoBehaviour
    {
        public InventoryItemBase item; // The item we wish to store
        public int amount = 1;
        public ItemCollectionBase referenceCollection; // The reference collection to move it to

        public void Start()
        {
            var
                i = Instantiate(
                    item); // Create a copy, remember, only instance objects are allowed to be stored in collections.
            i.currentStackSize = (uint)amount;

            var storedItems = new List<InventoryItemBase>();
            var added = InventoryManager.AddItem(i, storedItems); // Add the item to an inventory (loot to collection)
            if (added && storedItems.First().itemCollection != null)
                storedItems.First().itemCollection
                    .MoveItem(i, i.index, referenceCollection, 0,
                        false); // Move the item to a reference collection, a reference doesn't destroy the item from it's original location.

            // And remove it
            referenceCollection.SetItem(0, null, true);
        }
    }
}