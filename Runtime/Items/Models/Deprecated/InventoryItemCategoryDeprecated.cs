using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.InventoryPro
{
    /// <summary>
    ///     Used to define categories for items, categories can have a global cooldown, this can be usefull to cooldown all
    ///     potions for example.
    /// </summary>
    [Serializable]
    [Obsolete("Replaced by ItemCategory")]
    public class InventoryItemCategoryDeprecated
    {
        public uint ID;
        public string name;
        public Sprite icon;

        /// <summary>
        ///     If you don't want a cooldown leave it at 0.0
        /// </summary>
        [Range(0, 999)]
        public float cooldownTime;

        [HideInInspector]
        [NonSerialized]
        public float lastUsageTime;


        [HideInInspector]
        [NonSerialized]
        public List<OverrideCooldownRow> overrideCooldownList = new();

        public class OverrideCooldownRow
        {
            public uint itemID;
            public float lastUsageTime;
        }
    }
}