using System;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Devdog.InventoryPro.UI
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "UI Helpers/UI Element Key Actions")]
    public class UIElementKeyActions : MonoBehaviour
    {
        [Header("Action")]
        public KeyCode keyCode;

        public KeyAction keyActions = new();

        /// <summary>
        ///     The time the action has to be active before invoking the action.
        /// </summary>
        [Header("Timers")]
        public float activationTime;

        public bool continous;

        [Header("Visuals")]
        public UIShowValue visualizer = new();


        /// <summary>
        ///     The time (duration) the action has been activated.
        /// </summary>
        private float _activeTime { get; set; }

        private bool _firedInActiveTime { get; set; }
        private UIWindow window { get; set; }


        protected virtual void Awake()
        {
            window = GetComponent<UIWindow>();
        }

        protected virtual void Update()
        {
            if (gameObject.activeInHierarchy == false)
                return;

            if (window != null && window.isVisible == false)
                return;

            if (activationTime <= 0.01f)
            {
                if (continous)
                {
                    if (Input.GetKey(keyCode)) Activate();
                }
                else
                {
                    if (Input.GetKeyDown(keyCode)) Activate();
                }

                return;
            }

            // Got a timer.


            // Timer
            if (Input.GetKey(keyCode))
            {
                _activeTime += Time.deltaTime;

                visualizer.Repaint(_activeTime, activationTime);
            }
            else
            {
                // No key, reset timer.
                _activeTime = 0.0f;
                _firedInActiveTime = false;

                visualizer.Repaint(0, 1);
            }

            // Timer check
            if (_activeTime < activationTime)
                return;

            // Time set, it's activated...

            if (continous)
            {
                if (Input.GetKey(keyCode)) keyActions.Invoke();
            }
            else
            {
                // Already fired / invoked events?
                if (_firedInActiveTime)
                    return;

                if (Input.GetKey(keyCode))
                {
                    keyActions.Invoke();
                    _firedInActiveTime = true;
                }
            }
        }

        protected virtual void Activate()
        {
            if (InputManager.CanReceiveUIInput(gameObject) == false)
                return;

            keyActions.Invoke();
            visualizer.Activate();
        }

        [Serializable]
        public class KeyAction : UnityEvent
        {
        }
    }
}