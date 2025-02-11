﻿using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [Serializable]
    [Obsolete("Replaced by CraftingCategory")]
    public class InventoryCraftingCategoryDeprecated
    {
        /// <summary>
        ///     The unique ID as well as the Index in the ItemManager
        /// </summary>
        [HideInInspector]
        public int ID;

        /// <summary>
        ///     The name of this category.
        /// </summary>
        public string name;

        /// <summary>
        ///     The description of this category.
        /// </summary>
        public string description;

        /// <summary>
        ///     The category icon.
        /// </summary>
        public Sprite icon;

        /// <summary>
        ///     Also scan through the bank for items to use when crafting the item.
        /// </summary>
        public bool alsoScanBankForRequiredItems;

        /// <summary>
        ///     Amount of rows for layouts
        /// </summary>
        public uint rows = 3;

        /// <summary>
        ///     Amount of cols for layouts
        /// </summary>
        public uint cols = 3;

        /// <summary>
        ///     All available blueprints. Blueprints are craftable objects.
        /// </summary>
        public InventoryCraftingBlueprintDeprecated[] blueprints = new InventoryCraftingBlueprintDeprecated[0];

        /// <summary>
        ///     The audio clip played when the craft has succeeded
        /// </summary>
        public AudioClipInfo successAudioClip = new();

        /// <summary>
        ///     The audio clip played on the crafting trigger while crafting an item. The audio clip can be looped.
        /// </summary>
        public AudioClipInfo craftingAudioClip = new() { loop = true };

        /// <summary>
        ///     The audio clip played when the craft was canceled.
        /// </summary>
        public AudioClipInfo canceledAudioClip = new();

        /// <summary>
        ///     The audio cilp played when the craft has failed.
        /// </summary>
        public AudioClipInfo failedAudioClip = new();


        public override string ToString()
        {
            return name;
        }
    }
}