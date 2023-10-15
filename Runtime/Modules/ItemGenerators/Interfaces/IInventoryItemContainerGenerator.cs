namespace Devdog.InventoryPro
{
    public interface IInventoryItemContainerGenerator
    {
        IInventoryItemContainer container { get; }

        IItemGenerator generator { get; }
    }
}