using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro.Demo
{
    [RequireComponent(typeof(Toggle))]
    public class UISetContextMenu : MonoBehaviour
    {
        private Toggle t;

        public void Awake()
        {
            t = GetComponent<Toggle>();
            t.onValueChanged.AddListener(c => { InventorySettingsManager.instance.settings.useContextMenu = c; });
        }
    }
}