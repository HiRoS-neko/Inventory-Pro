using UnityEngine;

namespace Devdog.InventoryPro
{
    public class CraftingWindowLayoutCollectionUI : ItemCollectionBase
    {
        //[Header("Behavior")] // Moved to custom editor

        [SerializeField]
        private uint _initialCollectionSize = 9;

        public override uint initialCollectionSize => _initialCollectionSize;

        protected override void Awake()
        {
            base.Awake();

//            this._craftingWindow = GetComponent<CraftingWindowLayoutUI>();
//            _canDragInCollectionDefault = canDragInCollection;
        }
    }
}