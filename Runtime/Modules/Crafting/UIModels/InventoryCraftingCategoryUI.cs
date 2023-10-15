using Devdog.General;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    /// <summary>
    ///     A single row in the infobox.
    /// </summary>
    public class InventoryCraftingCategoryUI : MonoBehaviour, IPoolable
    {
        [SerializeField]
        protected Text title;

        [SerializeField]
        protected Image icon;

        [Required]
        public RectTransform container;

        public void ResetStateForPool()
        {
            // Item has no specific states, no need to reset
        }

        public virtual void Repaint(CraftingCategory category, ItemCategory itemCategory)
        {
            title.text = category.name;

            if (icon != null)
                icon.sprite = itemCategory.icon;
        }
    }
}