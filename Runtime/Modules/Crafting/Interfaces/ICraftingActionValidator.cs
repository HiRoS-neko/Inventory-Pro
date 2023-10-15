using System.Collections;
using UnityEngine;

namespace Devdog.InventoryPro
{
    public interface ICraftingActionValidator
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        CraftingProgressContainer progressContainer { get; }

//        bool CanCraftBlueprint(InventoryCraftingBlueprint blueprint, bool alsoScanBank, int craftCount);
        bool CanCraftBlueprint(InventoryPlayer player, CraftingProgressContainer.CraftInfo craftInfo);
        void RemoveRequiredCraftItemsAndCurrency(CraftingProgressContainer.CraftInfo craftInfo);
        void GiveCraftReward(CraftingProgressContainer.CraftInfo craftInfo);

        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(IEnumerator coroutine);
    }
}