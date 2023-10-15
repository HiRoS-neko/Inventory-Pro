using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro.Demo
{
    public class InventoryLookAtPlayer : MonoBehaviour
    {
        public Vector3 rotationOffset;

        public void Update()
        {
            if (PlayerManager.instance.currentPlayer == null)
                return;

            transform.LookAt(PlayerManager.instance.currentPlayer.transform, Vector3.up);
            transform.Rotate(rotationOffset);
        }
    }
}