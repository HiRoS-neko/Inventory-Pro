using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro.Demo
{
    [RequireComponent(typeof(Trigger))]
    public class MyCustomCollectionTrigger : MonoBehaviour, IInventoryItemContainer, ITriggerCallbacks
    {
        [SerializeField]
        private InventoryItemBase[] _items = new InventoryItemBase[0];


        [SerializeField]
        private string _uniqueName;

        private ItemCollectionBase _collection;

        private CollectionToArraySyncer _syncer;


        public void Awake()
        {
            // Create instance objects.
            for (var i = 0; i < items.Length; i++)
                if (items[i] != null)
                {
                    items[i] = Instantiate(items[i]);
                    items[i].transform.SetParent(transform);
                    items[i].gameObject.SetActive(false);
                }

            // The triggerHandler component, that is always there because of RequireComponent
            var trigger = GetComponent<Trigger>();

            // The collection we want to place the items into.
            _collection = trigger.window.window.GetComponent<ItemCollectionBase>();
        }

        public InventoryItemBase[] items
        {
            get => _items;
            set => _items = value;
        }

        public string uniqueName
        {
            get => _uniqueName;
            set => _uniqueName = value;
        }

        public bool OnTriggerUsed(Player player)
        {
            // When the user has triggered this object, set the items in the window
            _collection.SetItems(items, true);

            _syncer = new CollectionToArraySyncer(_collection, items);
            _syncer.StartSyncing();

            // And done!
            return false;
        }

        public bool OnTriggerUnUsed(Player player)
        {
            _syncer.StopSyncing();
            return false;
        }
    }
}