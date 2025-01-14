﻿namespace Devdog.InventoryPro
{
    public interface ICharacterCollection
    {
//        StatsCollection statsCollection { get; }
        IEquippableCharacter character { get; set; }
        EquippableSlot[] equippableSlots { get; }

        bool useReferences { get; }


        EquippableSlot[] GetEquippableSlots(EquippableInventoryItem item);
        bool EquipItem(EquippableSlot slot, EquippableInventoryItem item);
        bool UnEquipItem(EquippableInventoryItem item, bool addToInventory);
    }
}