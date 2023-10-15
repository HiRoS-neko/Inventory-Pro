using UnityEngine;

namespace Devdog.InventoryPro
{
    [CreateAssetMenu(menuName = InventoryPro.CreateAssetMenuPath + "Child equipment handler")]
    public class ItemChildEquipmentHandler : ItemEquipmentHandlerBase
    {
        public override EquippableInventoryItem Equip(EquippableInventoryItem item, CharacterEquipmentTypeBinder binder,
            bool createCopy)
        {
            var copy = item;
            if (createCopy) copy = CreateDefaultCopy(item);

            copy.transform.SetParent(binder.equipTransform);
            copy.transform.localPosition = copy.equipmentPosition;
            copy.transform.localRotation = copy.equipmentRotation;

            HandleSkinnedMeshes(copy, binder);
            HandleClothMeshes(copy, binder);

            return copy;
        }


        public override void UnEquip(CharacterEquipmentTypeBinder binder, bool deleteItem)
        {
            if (deleteItem) Destroy(binder.currentItem);
        }
    }
}