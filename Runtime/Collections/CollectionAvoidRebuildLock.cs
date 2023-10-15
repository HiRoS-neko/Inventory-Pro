using System;

namespace Devdog.InventoryPro
{
    public class CollectionAvoidRebuildLock : IDisposable
    {
        public CollectionAvoidRebuildLock(params ItemCollectionBase[] collections)
        {
            this.collections = collections;
            foreach (var col in this.collections) col.disableCounterRebuildBlocks++;
        }

        public ItemCollectionBase[] collections { get; set; }

        public void Dispose()
        {
            foreach (var col in collections) col.disableCounterRebuildBlocks--;
        }
    }
}