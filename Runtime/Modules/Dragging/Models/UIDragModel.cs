namespace Devdog.InventoryPro.UI
{
    public class UIDragModel
    {
        public int endIndex = -1;
        public ItemCollectionBase endItemCollection;
        public int startIndex = -1;
        public ItemCollectionBase startItemCollection;

        public bool endOnSlot => endItemCollection != null;


        public void Reset()
        {
            startIndex = -1;
            startItemCollection = null;

            endIndex = -1;
            endItemCollection = null;
        }
    }
}