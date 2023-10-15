using Devdog.General;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    /// <summary>
    ///     An item in the context menu (visual item)
    /// </summary>
    public class InventoryContextMenuRowUI : MonoBehaviour, IPointerClickHandler, IPoolable
    {
        public Button button;
        public Text text;

        [HideInInspector]
        public InventoryItemBase item;

        public AudioClipInfo onUse;

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (onUse == null)
                return;

            button.onClick.AddListener(() => { AudioManager.AudioPlayOneShot(onUse); });
        }

        public void ResetStateForPool()
        {
            //item = null; // No need to reset
            button.onClick.RemoveAllListeners();
        }
    }
}