// /**
// * Written By Joris Huijbregts
// * Some legal stuff --- Copyright!
// */

using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Managers/Inventory Settings manager")]
    public class InventorySettingsManager : ManagerBase<InventorySettingsManager>
    {
        [Required]
        [SerializeField]
        private InventorySettingsDatabase _settings;

        public InventorySettingsDatabase settings
        {
            get => _settings;
            set => _settings = value;
        }

        protected override void Awake()
        {
            base.Awake();
        }
    }
}