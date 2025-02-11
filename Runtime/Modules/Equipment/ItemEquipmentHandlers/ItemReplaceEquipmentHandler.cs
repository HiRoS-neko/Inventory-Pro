﻿using UnityEngine;

namespace Devdog.InventoryPro
{
    [CreateAssetMenu(menuName = InventoryPro.CreateAssetMenuPath + "Replace equipment handler")]
    public class ItemReplaceEquipmentHandler : ItemEquipmentHandlerBase
    {
        public override EquippableInventoryItem Equip(EquippableInventoryItem item, CharacterEquipmentTypeBinder binder,
            bool createCopy)
        {
            var copy = item;
            if (createCopy) copy = CreateDefaultCopy(item);

            copy.transform.SetParent(binder.equipTransform.parent); // Same level
            copy.transform.SetSiblingIndex(binder.equipTransform.GetSiblingIndex());
            binder.equipTransform.gameObject.SetActive(false); // Swap the item by disabling the original

            copy.transform.localPosition = copy.equipmentPosition;
            copy.transform.localRotation = copy.equipmentRotation;

            HandleSkinnedMeshes(copy, binder);
            HandleClothMeshes(copy, binder);

            return copy;
        }

        public override void UnEquip(CharacterEquipmentTypeBinder binder, bool deleteItem)
        {
            binder.equipTransform.gameObject.SetActive(true); // Re-enable the original
            if (deleteItem) Destroy(binder.currentItem);
        }
    }
}