using System.Linq;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [HelpURL("http://devdog.nl/documentation/lootables-generators/")]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Trigger))]
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Triggers/Lootable objet")]
    public class LootableObject : MonoBehaviour, IInventoryItemContainer, ITriggerWindowContainer, ITriggerCallbacks
    {
        public delegate void Empty();

        public delegate void LootedItem(InventoryItemBase item, uint itemID, uint slot, uint amount);


        [SerializeField]
        private string _uniqueName;


        [SerializeField]
        private InventoryItemBase[] _items = new InventoryItemBase[0];

        /// <summary>
        ///     The currencies we're holding
        /// </summary>
        public CurrencyDecorator[] currencies = new CurrencyDecorator[0];


        [SerializeField]
        private bool _destroyWhenEmpty;

        protected Animator animator;


        public LootUI lootUI { get; protected set; }
        public TriggerBase trigger { get; protected set; }


        protected virtual void Start()
        {
            lootUI = InventoryManager.instance.loot;
            if (lootUI == null)
            {
                Debug.LogWarning("No loot window set, yet there's a lootable object in the scene", transform);
                return;
            }

            if (GetComponent<IInventoryItemContainerGenerator>() == null)
                // Items were not generated -> Instantiate them
                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i] == null) continue;

                    var newItem = Instantiate(items[i]);
                    newItem.currentStackSize = items[i].currentStackSize;
                    items[i] = newItem;

                    items[i].transform.SetParent(transform);
                    items[i].gameObject.SetActive(false);
                }

            window = lootUI.window;
            trigger = GetComponent<TriggerBase>();
            animator = GetComponent<Animator>();
        }

        public string uniqueName
        {
            get => _uniqueName;
            set => _uniqueName = value;
        }

        public InventoryItemBase[] items
        {
            get => _items;
            set
            {
                _items = value.Where(o => o is CurrencyInventoryItem == false).ToArray();

                var cs = value.Select(o => o as CurrencyInventoryItem).Where(o => o != null).ToArray();
                currencies = new CurrencyDecorator[cs.Length];
                for (var i = 0; i < currencies.Length; i++)
                    currencies[i] = new CurrencyDecorator(cs[i].currency, cs[i].amount);
            }
        }

        public UIWindow window { get; protected set; }

        /// <summary>
        ///     Called when an item was looted by a player from this lootable object.
        /// </summary>
        public event LootedItem OnLootedItem;

        public event Empty OnEmpty;

        private void LootUIOnOnRemovedItem(InventoryItemBase item, uint itemID, uint slot, uint amount)
        {
            items[slot] = null;

            if (OnLootedItem != null)
                OnLootedItem(item, itemID, slot, amount);

            if (lootUI.isEmpty)
            {
                if (OnEmpty != null)
                    OnEmpty();

                if (_destroyWhenEmpty) Destroy(gameObject);
            }
        }

        private void LootUIOnOnCurrencyChanged(float before, CurrencyDecorator after)
        {
            var lookup = currencies.FirstOrDefault(o => o.currency == after.currency);
            if (lookup != null) lookup.amount = after.amount;

            if (lootUI.isEmpty)
            {
                trigger.UnUse();

                if (_destroyWhenEmpty) Destroy(gameObject);
            }
        }

        public virtual bool OnTriggerUsed(Player player)
        {
            // Set items
            lootUI.Clear();
            lootUI.SetItems(items, false);
            foreach (var cur in currencies) lootUI.AddCurrency(cur.currency, cur.amount);

            CopyItemsFromLootCollection();

            lootUI.OnRemovedItem += LootUIOnOnRemovedItem;
            lootUI.OnCurrencyChanged += LootUIOnOnCurrencyChanged;
            lootUI.window.Show();

            return false;
        }

        public virtual bool OnTriggerUnUsed(Player player)
        {
            CopyItemsFromLootCollection();

            lootUI.OnRemovedItem -= LootUIOnOnRemovedItem;
            lootUI.OnCurrencyChanged -= LootUIOnOnCurrencyChanged;
            lootUI.window.Hide();

            return false;
        }

        protected void CopyItemsFromLootCollection()
        {
            // Copy over the items from the lootUI to make sure our flat array matches that of the lootUI collection (in case layout sized items are used)
            items = new InventoryItemBase[lootUI.collectionSize];
            for (var i = 0; i < items.Length; i++) items[i] = lootUI[i].item;
        }
    }
}