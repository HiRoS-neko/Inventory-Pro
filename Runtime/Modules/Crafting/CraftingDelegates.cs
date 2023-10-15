namespace Devdog.InventoryPro
{
    public class CraftingDelegates
    {
        public delegate void CraftCanceled(CraftingProgressContainer.CraftInfo craftInfo, float progress);

        public delegate void CraftFailed(CraftingProgressContainer.CraftInfo craftInfo);

        public delegate void CraftProgress(CraftingProgressContainer.CraftInfo craftInfo, float progress);

        public delegate void CraftStart(CraftingProgressContainer.CraftInfo craftInfo);

        public delegate void CraftSuccess(CraftingProgressContainer.CraftInfo craftInfo);
    }
}