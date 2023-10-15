using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [Serializable]
    public class ItemRarity : ScriptableObject
    {
        public uint ID;

        [Required]
        public new string name;

        public Color color = Color.white;

        /// <summary>
        ///     The item that is used when dropping something, leave null to use the object model itself.
        /// </summary>
        [Tooltip("The item that is used when dropping something, leave null to use the object model itself.")]
        public GameObject dropObject;
    }
}