using System;
using System.Collections;
using System.Collections.Generic;
using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Devdog.InventoryPro.UI
{
    [Obsolete("Use Devdog.General.UI.UIWindow instead", true)]
    [ReplacedBy(typeof(UIWindow))]
    public class UIWindowDeprecated : MonoBehaviour
    {
        [Header("Behavior")]
        public string windowName = "MyWindow";

        /// <summary>
        ///     Should the window be hidden when the game starts?
        /// </summary>
        public bool hideOnStart = true;

        /// <summary>
        ///     Set the position to 0,0 when the game starts
        /// </summary>
        public bool resetPositionOnStart = true;


        /// <summary>
        ///     The animation played when showing the window, if null the item will be shown without animation.
        /// </summary>
        [Header("Audio & Visuals")]
        [SerializeField]
        [FormerlySerializedAs("showAnimation")]
        private AnimationClip _showAnimation;

        /// <summary>
        ///     The animation played when hiding the window, if null the item will be hidden without animation.
        /// </summary>
        [SerializeField]
        [FormerlySerializedAs("hideAnimation")]
        private AnimationClip _hideAnimation;

        public AudioClipInfo showAudioClip;
        public AudioClipInfo hideAudioClip;


        [Header("Actions")]
        public UIWindowActionEvent onShowActions = new();

        public UIWindowActionEvent onHideActions = new();

        private IEnumerator _animationCoroutine;

        private Animator _animator;


        private List<UIWindowPage> _pages;

        private RectTransform _rectTransform;
        public int showAnimationHash { get; protected set; }
        public int hideAnimationHash { get; protected set; }


        /// <summary>
        ///     Is the window visible or not? Used for toggling.
        /// </summary>
        public bool isVisible { get; protected set; }

        public List<UIWindowPage> pages
        {
            get
            {
                if (_pages == null)
                    _pages = new List<UIWindowPage>();

                return _pages;
            }
            protected set => _pages = value;
        }


        public UIWindowPage currentPage { get; set; }

        public Animator animator
        {
            get
            {
                if (_animator == null)
                    _animator = GetComponent<Animator>();

                return _animator;
            }
        }

        protected RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }
        }

        [Serializable]
        public class UIWindowActionEvent : UnityEvent
        {
        }
    }
}