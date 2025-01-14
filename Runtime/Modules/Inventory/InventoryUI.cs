﻿using System.Collections;
using System.Collections.Generic;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Windows/Inventory")]
    [RequireComponent(typeof(UIWindow))]
    public class InventoryUI : ItemCollectionBase, ICollectionPriority
    {
        //public AudioSource audioSource;
        //public UnityEngine.UI.Text goldText;
        [Header("Behavior")]
        public Button sortButton;

//        /// <summary>
//        /// The collection used to extend this bag, leave empty if there is none.
//        /// </summary>
//        [SerializeField]
//        private ItemCollectionBase _extenderCollection;
//        public ItemCollectionBase extenderCollection
//        {
//            get { return _extenderCollection; }
//        }

/// <summary>
///     Scrolling rect inside the inventory
/// </summary>
public ScrollRect scrollRect;

/// <summary>
///     When true all characters will be able to use this inventory, if false, it can be associated with a character.
/// </summary>
public bool isSharedCollection;


/// <summary>
///     How much priority does this inventory have when looting? When an item can be stored in multiple inventories, which
///     one should be chosen?
///     Range of 0 to 100
/// </summary>
[Range(0, 100)]
        [SerializeField]
        private int _collectionPriority;

        //public int lootPriority = 50;

        [SerializeField]
        private uint _initialCollectionSize = 20;


        /// <summary>
        ///     When the item is used and the bank is open the item will be stored.
        /// </summary>
        [Header("Item usage")]
        public bool useItemMoveToBank = true;

        /// <summary>
        ///     When the item is used and the vendor window is open, should the item be sold.
        /// </summary>
        public bool useItemSell = true;

        [Header("Audio & Visuals")]
        public AudioClipInfo swapItemAudioClip;

        public AudioClipInfo currencyChangeAudioClip;
        public AudioClipInfo sortAudioClip;
        public AudioClipInfo onAddItemAudioClip; // When an item is added to the inventory
        private UIWindow _window;

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

        public override uint initialCollectionSize => _initialCollectionSize;


        protected override void Awake()
        {
            base.Awake();

            if (isSharedCollection)
                InventoryManager.AddInventoryCollection(this, collectionPriority);

            if (sortButton != null)
                sortButton.onClick.AddListener(() =>
                {
                    SortCollection();

                    AudioManager.AudioPlayOneShot(sortAudioClip);
                });
        }

        protected override void Start()
        {
            base.Start();


            // Listen for events
            OnAddedItem += (items, amount, cameFromCollection) =>
            {
                AudioManager.AudioPlayOneShot(onAddItemAudioClip);
            };

            OnSwappedItems += (fromCollection, fromSlot, toCollection, toSlot) =>
            {
                AudioManager.AudioPlayOneShot(swapItemAudioClip);
            };

            OnResized += (fromSize, toSize) => { StartCoroutine(SetScrollToZero()); };

            OnCurrencyChanged += (before, dec) => { AudioManager.AudioPlayOneShot(currencyChangeAudioClip); };

            //OnDroppedItem += (InventoryItemBase item, uint slot, GameObject droppedObj) =>
            //{

            //};
            //OnSorted += () =>
            //{

            //};
            //OnGoldChanged += (float goldAdded) =>
            //{

            //};
        }

        public int collectionPriority
        {
            get => _collectionPriority;
            set => _collectionPriority = value;
        }

        private IEnumerator SetScrollToZero()
        {
            yield return
                null; // Wait a frame for the UI to update... (UI Layout updates at the end of update cycle, so wait 1 cycle, then update)

            if (scrollRect != null && scrollRect.verticalScrollbar != null)
                scrollRect.verticalNormalizedPosition = 1.0f;
        }

        public override IList<ItemUsability> GetExtraItemUsabilities(IList<ItemUsability> basicList)
        {
            var basic = base.GetExtraItemUsabilities(basicList);

            if (InventoryManager.instance.bank != null)
                if (InventoryManager.instance.bank.window.isVisible)
                    basic.Add(new ItemUsability("Store",
                        item => { InventoryManager.instance.bank.AddItemAndRemove(item); }));

            if (InventoryManager.instance.vendor != null)
                if (InventoryManager.instance.vendor.window.isVisible)
                    basic.Add(new ItemUsability("Sell",
                        item => { InventoryManager.instance.vendor.currentVendor.SellItemToVendor(item); }));

            return basic;
        }

        public override bool OverrideUseMethod(InventoryItemBase item)
        {
            // If both bank and vendor are open bank will take priority, probably the safest action...
            if (InventorySettingsManager.instance.settings.useContextMenu)
                return false;

            if (useItemMoveToBank)
                if (InventoryManager.instance.bank != null && InventoryManager.instance.bank.window.isVisible)
                {
                    if (item.isStorable)
                        InventoryManager.instance.bank.AddItemAndRemove(item);
                    else
                        InventoryManager.langDatabase.itemCannotBeStored.Show(item.name, item.description,
                            InventoryManager.instance.bank.collectionName);

                    return true;
                }

            if (useItemSell)
                if (InventoryManager.instance.vendor != null && InventoryManager.instance.vendor.window.isVisible)
                {
                    InventoryManager.instance.vendor.currentVendor.SellItemToVendor(item);
                    return true;
                }

            return false; // Didn't override anything
        }
    }
}