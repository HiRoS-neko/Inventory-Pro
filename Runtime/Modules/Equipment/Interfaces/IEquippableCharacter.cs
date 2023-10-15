namespace Devdog.InventoryPro
{
    public interface IEquippableCharacter : ICharacterStats
    {
        CharacterEquipmentTypeBinder[] equipmentBinders { get; set; }
        CharacterEquipmentHandlerBase equipmentHandler { get; }
    }
}