namespace Devdog.InventoryPro
{
    /// <summary>
    ///     A lookup for the InvenetoryManager to keep track of inventories.
    ///     !! Does not require serialization, it's runtime only
    /// </summary>
    public class ItemCollectionPriority<T>
    {
        public ItemCollectionPriority(T collection, int priority)
        {
            this.collection = collection;
            this.priority = priority;
        }

        public T collection { get; set; }

        /// <summary>
        ///     Range of 0-100
        /// </summary>
        public int priority { get; set; }
    }
}