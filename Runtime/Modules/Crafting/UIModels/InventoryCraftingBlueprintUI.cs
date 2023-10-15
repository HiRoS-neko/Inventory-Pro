using Devdog.General;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    /// <summary>
    ///     A single row in the infobox.
    /// </summary>
    public class InventoryCraftingBlueprintUI : MonoBehaviour, IPoolable
    {
        [SerializeField]
        protected Text blueprintName;

        [SerializeField]
        protected Text blueprintDescription;

        [SerializeField]
        protected Image blueprintIcon;

        [Required]
        public Button button;

        public void ResetStateForPool()
        {
            button.onClick.RemoveAllListeners();
            // Item has no specific states, no need to reset
        }


        public virtual void Repaint(CraftingBlueprint blueprint)
        {
            if (blueprintName != null)
                blueprintName.text = blueprint.name;

            if (blueprintDescription != null)
                blueprintDescription.text = blueprint.description;

            if (blueprintIcon != null && blueprint.resultItems.Length > 0)
                blueprintIcon.sprite = blueprint.resultItems[0].item.icon;
        }
    }
}