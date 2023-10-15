using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [Serializable]
    public class CraftingCategory : ScriptableObject
    {
        [HideInInspector]
        public int ID;

        [Required]
        public new string name;

        public string description;
        public Sprite icon;

        /// <summary>
        ///     Also scan through the bank for items to use when crafting the item.
        /// </summary>
        public bool alsoScanBankForRequiredItems;

        public uint rows = 3;
        public uint cols = 3;

        public CraftingBlueprint[] blueprints = new CraftingBlueprint[0];

        public AudioClipInfo successAudioClip = new();
        public AudioClipInfo craftingAudioClip = new() { loop = true };
        public AudioClipInfo canceledAudioClip = new();
        public AudioClipInfo failedAudioClip = new();

        public override string ToString()
        {
            return name;
        }
    }
}