using Devdog.General;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    /// <summary>
    ///     A single row in the infobox.
    /// </summary>
    public class InfoBoxRowUI : MonoBehaviour, IPoolable
    {
        public Text title;
        public Text message;


        public void ResetStateForPool()
        {
            // Item has no specific states, no need to reset
        }
    }
}