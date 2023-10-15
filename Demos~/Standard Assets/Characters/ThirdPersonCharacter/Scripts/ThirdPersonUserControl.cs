using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro.UnityStandardAssets
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour, IPlayerInputCallbacks
    {
        public float walkSpeedMultilpier = 0.5f;
        private Transform m_Cam; // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward; // The current forward direction of the camera
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Vector3 m_Move;

        protected void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
                m_Cam = Camera.main.transform;
            else
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();

            var player = PlayerManager.instance.currentPlayer;
            if (player != null)
            {
                player.InventoryPlayer().stats.OnStatValueChanged += CharacterCollectionOnOnStatChanged;

                var stat = player.InventoryPlayer().stats.Get("Default", "Run speed");
                if (stat != null) CharacterCollectionOnOnStatChanged(stat);
            }
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            var h = Input.GetAxis("Horizontal"); // CrossPlatformInputManager
            var v = Input.GetAxis("Vertical"); // CrossPlatformInputManager
            //bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }

            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift))
                m_Move *= walkSpeedMultilpier * 2;
            else
                m_Move *= walkSpeedMultilpier;

            // pass all parameters to the character control script
            m_Character.Move(m_Move, false, false);
        }


        public void SetInputActive(bool active)
        {
            enabled = active;
            m_Character.enabled = active;
//            if (active == false)
//            {
//                GetComponent<Rigidbody>().velocity = Vector3.zero;
//            }
        }

        private void CharacterCollectionOnOnStatChanged(IStat stat)
        {
            if (stat.definition.statName == "Run speed") walkSpeedMultilpier = stat.currentValue / 100f;
        }
    }
}