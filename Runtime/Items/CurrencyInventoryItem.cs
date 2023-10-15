using System.Collections.Generic;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    /// <summary>
    ///     This is used to represent gold as an item, once the item is picked up gold will be added to the inventory
    ///     collection.
    /// </summary>
    public class CurrencyInventoryItem : InventoryItemBase
    {
        [SerializeField]
        private float _amount;

        [SerializeField]
        [Required]
        private CurrencyDefinition _currency;

        public float amount
        {
            get => _amount;
            protected set => _amount = value;
        }

        public override uint currentStackSize
        {
            get => (uint)_amount;
            set
            {
                base.currentStackSize = value;
                _amount = value;
            }
        }

        public CurrencyDefinition currency
        {
            get => _currency;
            protected set => _currency = value;
        }

        public override bool PickupItem()
        {
            InventoryManager.AddCurrency(currency, _amount);

            if (IsInstanceObject())
                Destroy(gameObject); // Don't need to store gold objects

            return true;
        }

        public override LinkedList<ItemInfoRow[]> GetInfo()
        {
            var info = base.GetInfo();
            info.Clear();

            info.AddLast(new ItemInfoRow[]
            {
                new("Amount", currency.ToString(_amount, 0f, float.MaxValue))
            });

            return info;
        }

        public override int Use()
        {
            return -2; // Can't use currencies
        }
    }
}