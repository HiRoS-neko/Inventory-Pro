using UnityEngine;

namespace Devdog.InventoryPro.Demo
{
    public class ObjectPingPong : MonoBehaviour
    {
        public float speed = 1.0f;
        public float length = 5.0f;

        public Vector3 rotation;
        private float sinTime;


        public void Update()
        {
            sinTime += Time.deltaTime;
            transform.Rotate(rotation * Time.deltaTime);

            transform.Translate(0, Mathf.Sin(sinTime * speed) * length, 0);
        }
    }
}