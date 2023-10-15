using System;
using System.Collections.Generic;
using System.Linq;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Windows/Skillbar")]
    public class SkillbarUI : ItemCollectionBase
    {
        [Header("General")]
        public SkillbarSlot[] keys;

        /// <summary>
        ///     When a consumable stack (or any other type) is depleted remove the stack and clear the slot.
        /// </summary>
        public bool clearSlotWhenStackDepleted;


        private ItemCollectionBase[] _subscribedToCollection = new ItemCollectionBase[0];

        public override uint initialCollectionSize => (uint)keys.Length;


        protected override void Awake()
        {
            base.Awake();

            InitSlots();
        }

        protected override void Start()
        {
            base.Start();

            PlayerManager.instance.OnPlayerChanged += InstanceOnPlayerChanged;
            if (PlayerManager.instance.currentPlayer != null)
                InstanceOnPlayerChanged(null, PlayerManager.instance.currentPlayer);
        }

        protected virtual void Update()
        {
            if (UIUtility.isFocusedOnInput)
                return;

            for (var i = 0; i < keys.Length; i++)
            {
                uint keysDown = 0;
                foreach (var k in keys[i].keyCombination)
                    if (Input.GetKeyDown(k))
                        keysDown++;

                if (keysDown == keys[i].keyCombination.Length && keys[i].keyCombination.Length > 0)
                {
                    // All keys down
                    items[i].TriggerUse();
                    items[i].Repaint();

                    return; // Only 1 key input per frame...
                }
            }
        }


        protected virtual void InitSlots()
        {
            if (manuallyDefineCollection)
                Assert.IsTrue(items.Length == keys.Length,
                    "More / less wrappers than there are keys defined in the SkillbarUI component! (wrappers: " +
                    items.Length + " - keys: " + keys.Length + ")");
            else
                // Fill the container on startup, can add / remove later on
                for (var i = 0; i < initialCollectionSize; i++)
                {
                    var keySlot = items[i] as ItemCollectionSlotUIKeyTrigger;
                    if (keySlot != null) keySlot.keyCombination = keys[i].name;

                    items[i].Repaint(); // First time
                }
        }

        private void InstanceOnPlayerChanged(Player oldPlayer, Player newPlayer)
        {
            if (oldPlayer != null)
                foreach (var col in _subscribedToCollection)
                {
                    col.OnAddedItem -= ColOnOnAddedItem;
                    col.OnRemovedItem -= ColOnOnRemovedItem;
                }

            _subscribedToCollection = InventoryManager.GetLootToCollections();
            foreach (var col in _subscribedToCollection)
            {
                col.OnAddedItem += ColOnOnAddedItem;
                col.OnRemovedItem += ColOnOnRemovedItem;
            }
        }

        private void ColOnOnRemovedItem(InventoryItemBase item, uint itemID, uint slot, uint amount)
        {
            RepaintOfID(itemID);
        }

        private void ColOnOnAddedItem(IEnumerable<InventoryItemBase> items, uint amount, bool cameFromCollection)
        {
            RepaintOfID(items.First().ID);
        }

        private void RepaintOfID(uint id)
        {
            foreach (var item in items)
                if (item.item != null && item.item.ID == id)
                {
                    if (item.item.currentStackSize == 0 && clearSlotWhenStackDepleted)
                        item.item = null;

                    item.Repaint();
                }
        }


        public override bool CanMergeSlots(uint slot1, ItemCollectionBase collection2, uint slot2)
        {
            var can = base.CanMergeSlots(slot1, collection2, slot2);
            if (can == false)
                return false;

            return useReferences == false;
        }

        [Serializable]
        public class SkillbarSlot
        {
            public KeyCode[] keyCombination;
            public string name;
        }
    }
}