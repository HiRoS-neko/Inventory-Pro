using System;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Devdog.InventoryPro
{
    [Obsolete("Replaced by Trigger and TriggerBase", true)]
    [ReplacedBy(typeof(Trigger))]
    public class ObjectTriggerer : ObjectTriggererBase
    {
        [SerializeField]
        private bool _triggerMouseClick = true;

        [SerializeField]
        private KeyCode _triggerKeyCode = KeyCode.None;

        /// <summary>
        ///     Toggle when triggered
        /// </summary>
        public bool toggleWhenTriggered = true;

        /// <summary>
        ///     Only required if handling the window directly
        /// </summary>
        [Header("The window")]
        [FormerlySerializedAs("window")]
        [SerializeField]
        private UIWindowField _window;

        [Header("Requirements")]
        public ItemBoolPair[] requiredItems = new ItemBoolPair[0];

        public StatRequirement[] statRequirements = new StatRequirement[0];

        [Header("Animations & Audio")]
        public AnimationClip useAnimation;

        public AnimationClip unUseAnimation;

        public AudioClipInfo useAudioClip;
        public AudioClipInfo unUseAudioClip;

        /// <summary>
        ///     When true the window will be triggered directly, if false, a 2nd party will have to handle it through events.
        /// </summary>
        [HideInInspector]
        [NonSerialized]
        public bool handleWindowDirectly = true;

        public override bool triggerMouseClick
        {
            get => _triggerMouseClick;
            set => _triggerMouseClick = value;
        }

        public override KeyCode triggerKeyCode
        {
            get => _triggerKeyCode;
            set => _triggerKeyCode = value;
        }

        public UIWindowField window
        {
            get => _window;
            set => _window = value;
        }

        public override CursorIcon cursorIcon
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        public override Sprite uiIcon
        {
            get => throw new NotImplementedException();

            set => throw new NotImplementedException();
        }

        [Serializable]
        public struct ItemBoolPair
        {
            public ItemAmountRow item;
            public bool remove;
        }
    }
}