using System.Collections.Generic;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Windows/Bank")]
    [RequireComponent(typeof(UIWindow))]
    public class BankUI : ItemCollectionBase
    {
        [Header("Behavior")]
        public Button sortButton;

        [SerializeField]
        private uint _initialCollectionSize = 80;

        /// <summary>
        ///     When the item is used from this collection, should the item be moved to the inventory?
        /// </summary>
        [Header("Item usage")]
        public bool useMoveToInventory = true;


        [Header("Audio & Visuals")]
        public AudioClipInfo swapItemAudioClip;

        public AudioClipInfo sortAudioClip;
        public AudioClipInfo onAddItemAudioClip; // When an item is added to the inventory

        private UIWindow _window;

        /// <inheritdoc />
        public override uint initialCollectionSize => _initialCollectionSize;

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


        protected override void Awake()
        {
            base.Awake();

            InventoryManager.AddBankCollection(this);

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
            OnAddedItem += (item, amount, cameFromCollection) => { AudioManager.AudioPlayOneShot(onAddItemAudioClip); };
            OnSwappedItems += (fromCollection, fromSlot, toCollection, toSlot) =>
            {
                AudioManager.AudioPlayOneShot(swapItemAudioClip);
            };
        }

        // Items from the bank go straight to the inventory
        public override bool OverrideUseMethod(InventoryItemBase item)
        {
            if (InventorySettingsManager.instance.settings.useContextMenu)
                return false;

            if (useMoveToInventory)
                InventoryManager.AddItemAndRemove(item);

            return useMoveToInventory;
        }

        public override IList<ItemUsability> GetExtraItemUsabilities(IList<ItemUsability> basic)
        {
            var l = base.GetExtraItemUsabilities(basic);

            l.Add(new ItemUsability("To inventory", item =>
            {
                var oldCollection = item.itemCollection;
                var oldIndex = item.index;

                var added = InventoryManager.AddItem(item);
                if (added)
                {
                    oldCollection.SetItem(oldIndex, null);
                    oldCollection[oldIndex].Repaint();

                    oldCollection.NotifyItemRemoved(item, item.ID, oldIndex, item.currentStackSize);
                }
            }));

            return l;
        }

        public override bool CanSetItem(uint slot, InventoryItemBase item)
        {
            var set = base.CanSetItem(slot, item);
            if (set == false)
                return false;

            if (item == null)
                return true;

            return item.isStorable;
        }
    }
}