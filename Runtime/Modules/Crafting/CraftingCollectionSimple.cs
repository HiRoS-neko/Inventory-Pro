using UnityEngine;

namespace Devdog.InventoryPro
{
    public class CraftingCollectionSimple : ItemCollectionBase
    {
        [SerializeField]
        private uint _initialCollectionSize;

        public override uint initialCollectionSize => _initialCollectionSize;


        public override bool OverrideUseMethod(InventoryItemBase item)
        {
//            InventoryManager.AddItemAndRemove(item);
            return true;
        }
    }
}