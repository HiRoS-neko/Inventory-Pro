using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro.Demo
{
    public class LoadLevel : MonoBehaviour
    {
        public void LoadALevel(string level)
        {
            SceneUtility.LoadScene(level);
        }
    }
}