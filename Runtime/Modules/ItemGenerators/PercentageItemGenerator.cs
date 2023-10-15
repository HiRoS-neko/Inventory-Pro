using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace Devdog.InventoryPro
{
    public class PercentageItemGenerator : IItemGenerator
    {
        protected static Random randomGen;

        public PercentageItemGenerator()
        {
            RandomizeSeed();
        }

        public InventoryItemGeneratorItem[] items { get; set; }

        public InventoryItemBase[] result { get; set; }

        public void RandomizeSeed()
        {
            randomGen = new Random(DateTime.Now.Millisecond);
        }

        /// <summary>
        ///     Generate an array of items.
        ///     InventoryItemGeneratorItem's chance is only affected after all the filters are applied, so the item might still be
        ///     rejected by type, category, etc.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public InventoryItemBase[] Generate(int amount, bool createInstances = false)
        {
            return Generate(amount, amount, createInstances);
        }

        /// <summary>
        ///     Generate an array of items.
        ///     InventoryItemGeneratorItem's chance is only affected after all the filters are applied, so the item might still be
        ///     rejected by type, category, etc.
        /// </summary>
        /// <param name="minAmount"></param>
        /// <param name="maxAmount"></param>
        /// <returns></returns>
        public InventoryItemBase[] Generate(int minAmount, int maxAmount, bool createInstances = false)
        {
            RandomizeSeed();

            var toReturn = new List<InventoryItemBase>(maxAmount);
            foreach (int i in Enumerable.Range(0, items.Count()).OrderBy(x => randomGen.Next()))
            {
                if (toReturn.Count >= maxAmount)
                    break;

                if (UnityEngine.Random.value > items[i].chanceFactor)
                    continue;

                if (createInstances)
                {
                    var instanceItem = Object.Instantiate(items[i].item);
                    instanceItem.currentStackSize =
                        (uint)UnityEngine.Random.Range(items[i].minStackSize, items[i].maxStackSize);
                    instanceItem.gameObject.SetActive(false);
                    toReturn.Add(instanceItem);
                }
                else
                {
                    toReturn.Add(items[i].item);
                }
            }

            result = toReturn.ToArray();
            return result;
        }

        public void SetItems(InventoryItemBase[] toSet, float chance = 1.0f)
        {
            items = new InventoryItemGeneratorItem[toSet.Length];
            for (var i = 0; i < items.Length; i++) items[i] = new InventoryItemGeneratorItem(toSet[i], chance);
        }
    }
}