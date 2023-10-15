using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro.Demo
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character2DDemoController : MonoBehaviour, IPlayerInputCallbacks
    {
        public float speed = 3.0f;
        public float jumpSpeed = 10.0f;

        public float maxSpeed = 20.0f;
        private bool _inputEnabled = true;


        private Rigidbody2D _rigid { get; set; }


        public void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            if (_inputEnabled == false)
                return;

            if (Input.GetKey(KeyCode.D))
                // Move right
                AddVelocity(Vector2.right * (speed * Time.deltaTime));
            else if (Input.GetKey(KeyCode.A))
                // Move left
                AddVelocity(-Vector2.right * (speed * Time.deltaTime));


            if (Input.GetKeyDown(KeyCode.Space))
                // Jump
                AddVelocity(Vector2.up * jumpSpeed);
        }

        public void SetInputActive(bool enable)
        {
            _inputEnabled = enable;
        }

        private void AddVelocity(Vector2 velocity)
        {
            _rigid.velocity = _rigid.velocity + velocity;
            _rigid.velocity = Vector3.ClampMagnitude(_rigid.velocity, maxSpeed);
        }
    }
}