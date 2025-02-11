﻿using System.Collections.Generic;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Windows/Loot")]
    [RequireComponent(typeof(UIWindow))]
    public class LootUI : ItemCollectionBase
    {
        // Resize the collection so there's no empty slots
        public bool removeEmptySlots = true;

        private UIWindow _window;

        public override uint initialCollectionSize => 0;

        public virtual UIWindow window
        {
            get
            {
                if (_window == null)
                    _window = GetComponent<UIWindow>();

                return _window;
            }
            protected set => _window = value;
        }


        protected override void Start()
        {
            base.Start();

            OnRemovedItem += (item, itemID, slot, amount) => { HideWindowIfEmpty(); };

            window.OnShow += WindowOnOnShow;
        }

        private void WindowOnOnShow()
        {
            if (items.Length > 0) items[0].Select();
        }

        // <inheritcdoc />
        public override void SetItems(InventoryItemBase[] newItems, bool setParent, bool repaint = true)
        {
            var canPutIn = canPutItemsInCollection;
            canPutItemsInCollection = true;

            if (removeEmptySlots)
                Resize((uint)newItems.Length, true); // Force resize, SetItems() doesn't force, hence the extra call.

            base.SetItems(newItems, setParent, repaint);
            canPutItemsInCollection = canPutIn;
        }

        public override bool SetItem(uint slot, InventoryItemBase item)
        {
            var c = item as CurrencyInventoryItem;
            if (c != null) return AddCurrency(c.currency, c.amount);

            return base.SetItem(slot, item);
        }

        public virtual void TakeCurrencies()
        {
            foreach (var c in currenciesGroup.lookups)
            {
                var added = InventoryManager.AddCurrency(c);
                if (added) c.amount = 0f;
            }

            HideWindowIfEmpty();
        }

        public virtual void TakeAll()
        {
            TakeCurrencies();
            foreach (var slot in items)
                if (slot != null && slot.item != null)
                {
                    var item = slot.item;
                    var success = InventoryManager.AddItem(item);
                    if (success)
                    {
                        slot.item = null;
                        NotifyItemRemoved(item, item.ID, slot.index, item.currentStackSize);

                        slot.Repaint();
                    }
                }

            HideWindowIfEmpty();
        }

        protected virtual void HideWindowIfEmpty()
        {
            if (isEmpty) window.Hide();
        }

        public override IList<ItemUsability> GetExtraItemUsabilities(IList<ItemUsability> basic)
        {
            var l = base.GetExtraItemUsabilities(basic);

            l.Add(new ItemUsability("Loot", item => { InventoryManager.AddItemAndRemove(item); }));

            return l;
        }


        public override bool CanMergeSlots(uint slot1, ItemCollectionBase collection2, uint slot2)
        {
            return false;
        }

        public override bool SwapOrMerge(uint slot1, ItemCollectionBase handler2, uint slot2, bool repaint = true)
        {
            return false;
        }
    }
}