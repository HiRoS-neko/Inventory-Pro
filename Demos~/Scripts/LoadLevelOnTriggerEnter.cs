using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro.Demo
{
    public class LoadLevelOnTriggerEnter : MonoBehaviour
    {
        public string levelToLoad;


        public void OnTriggerEnter(Collider col)
        {
            LoadLevel();
        }


        public void LoadLevel()
        {
            SceneUtility.LoadScene(levelToLoad);
        }
    }
}