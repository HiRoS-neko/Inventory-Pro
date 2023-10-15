namespace Devdog.InventoryPro
{
    public interface IInventoryItemContainer
    {
        string uniqueName { get; }

        InventoryItemBase[] items { get; set; }
    }
}