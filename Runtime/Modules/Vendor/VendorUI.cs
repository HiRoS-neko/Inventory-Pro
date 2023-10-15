using System.Collections.Generic;
using System.Linq;
using Devdog.General;
using Devdog.General.UI;
using Devdog.InventoryPro.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Windows/Vendor UI")]
    [RequireComponent(typeof(UIWindow))]
    public class VendorUI : ItemCollectionBase, IInventoryDragAccepter
    {
        public VendorUIBuyBack buyBackCollection;
        public Text vendorNameText;


        /// <summary>
        ///     Prices can be modified per vendor, 0 will generate an int 1 will generate a value of 1.1 and so forth.
        /// </summary>
        //public int roundPriceToDecimals = 0;
        public AudioClipInfo audioWhenSoldItemToVendor;

        public AudioClipInfo audioWhenBoughtItemFromVendor;


        [SerializeField]
        protected uint _initialCollectionSize = 20;


        protected VendorTrigger _currentVendor;


        private ItemCollectionBase[] _subscribedToCollection = new ItemCollectionBase[0];


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

        public VendorTrigger currentVendor
        {
            get => _currentVendor;
            set
            {
                if (_currentVendor != null)
                    _currentVendor.items =
                        items.Select(o => o.item).Where(o => o != null)
                            .ToArray(); // Set items from this collection to the vendor to store them.

                _currentVendor = value;
                if (_currentVendor != null)
                {
                    if (window.isVisible == false)
                        return;

                    RepaintWindow();
                }
            }
        }

        public override uint initialCollectionSize => _initialCollectionSize;

        protected override void Awake()
        {
            base.Awake();
            canPutItemsInCollection = false;
            canUseFromCollection = true; // Overwritten below
            canUseItemsFromReference = false;

            PlayerManager.instance.OnPlayerChanged += InstanceOnPlayerChanged;
            if (PlayerManager.instance.currentPlayer != null)
                InstanceOnPlayerChanged(null, PlayerManager.instance.currentPlayer);
        }

        protected override void Start()
        {
            base.Start();

            window.OnShow += RepaintWindow;
        }

        // <inheritdoc />
        public virtual bool AcceptsDragItem(InventoryItemBase item)
        {
            if (currentVendor == null)
                return false;

            return item.isSellable && currentVendor.canSellToVendor;
        }

        /// <summary>
        ///     Called by the InventoryDragAccepter, when an item is dropped on the window / a specific location, this method is
        ///     called to add a custom behavior.
        /// </summary>
        /// <param name="item"></param>
        public virtual bool AcceptDragItem(InventoryItemBase item)
        {
            if (currentVendor == null || AcceptsDragItem(item) == false)
                return false;

            currentVendor.SellItemToVendor(item);
            return true;
        }

        private void InstanceOnPlayerChanged(Player oldPlayer, Player newPlayer)
        {
            if (oldPlayer != null)
                foreach (var col in _subscribedToCollection)
                    col.OnCurrencyChanged -= OnPlayerCurrencyChanged;

            _subscribedToCollection = InventoryManager.GetLootToCollections();
            foreach (var col in _subscribedToCollection) col.OnCurrencyChanged += OnPlayerCurrencyChanged;
        }

        private void OnPlayerCurrencyChanged(float amountBefore, CurrencyDecorator decorator)
        {
            if (window.isVisible == false)
                return;

            if (window.isVisible)
                RepaintWindow();
        }


        protected virtual void RepaintWindow()
        {
            foreach (var item in items)
                item.Repaint();

            if (vendorNameText != null)
                vendorNameText.text = _currentVendor.name;

            if (items.Length > 0) items[0].Select();
        }


        public override bool AddItem(InventoryItemBase item, ICollection<InventoryItemBase> storedItems = null,
            bool repaint = true, bool fireEvents = true)
        {
//            if (base.CanAddItem(item) == false)
//            {
//                AddSlots(1); // Make room for the new item
//            }

            var before = canPutItemsInCollection;
            canPutItemsInCollection = true;
            var set = base.AddItem(item, storedItems, repaint, fireEvents);
            canPutItemsInCollection = before;

            return set;
        }

        public override bool CanAddItem(InventoryItemBase item)
        {
            return true; // TODO: Add limit to vendor collection - OR Add currency, see if vendor can purchase items.
//            return base.CanAddItem(item);
        }


        public override bool OverrideUseMethod(InventoryItemBase item)
        {
            currentVendor.BuyItemFromVendor(item, false);
            return true;
        }

        public override bool CanMergeSlots(uint slot1, ItemCollectionBase collection2, uint slot2)
        {
            return false;
        }

        public override bool SwapOrMerge(uint slot1, ItemCollectionBase handler2, uint slot2, bool repaint = true)
        {
            return SwapSlots(slot1, handler2, slot2, repaint);
        }

        #region Events

        public delegate void VendorItemAction(InventoryItemBase item, uint amount, VendorTrigger vendor);


        /// <summary>
        ///     Fired when an item is sold.
        /// </summary>
        public event VendorItemAction OnSoldItemToVendor;

        /// <summary>
        ///     Fired when an item is bought, also fired when an item is bought back.
        /// </summary>
        public event VendorItemAction OnBoughtItemFromVendor;

        /// <summary>
        ///     Fired when an item is bought back from a vendor.
        /// </summary>
        public event VendorItemAction OnBoughtItemBackFromVendor;

        #endregion


        #region Notifies

        public virtual void NotifyItemSoldToVendor(InventoryItemBase item, uint amount)
        {
            InventoryManager.langDatabase.vendorSoldItemToVendor.Show(item.name, item.description, amount,
                currentVendor.name, item.sellPrice.ToString(amount));

            AudioManager.AudioPlayOneShot(audioWhenSoldItemToVendor);

            if (OnSoldItemToVendor != null)
                OnSoldItemToVendor(item, amount, currentVendor);
        }

        public virtual void NotifyItemBoughtFromVendor(InventoryItemBase item, uint amount)
        {
            InventoryManager.langDatabase.vendorBoughtItemFromVendor.Show(item.name, item.description, amount,
                currentVendor.name, item.buyPrice.ToString(amount));

            AudioManager.AudioPlayOneShot(audioWhenBoughtItemFromVendor);

            if (OnBoughtItemFromVendor != null)
                OnBoughtItemFromVendor(item, amount, currentVendor);
        }

        public virtual void NotifyItemBoughtBackFromVendor(InventoryItemBase item, uint amount)
        {
            if (OnBoughtItemBackFromVendor != null)
                OnBoughtItemBackFromVendor(item, amount, currentVendor);
        }

        #endregion
    }
}