using System;
using Devdog.General;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    /// <summary>
    ///     A single message inside the message displayer
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class NoticeMessageUI : MonoBehaviour, IPoolable
    {
        public Text title;
        public Text message;
        public Text time;

        public string format = "{0}";

        [Header("Animations")]
        public AnimationClip showAnimation;

        public AnimationClip hideAnimation;


        [NonSerialized]
        protected Animator animator;

        [NonSerialized]
        protected bool isHiding; // In the process of hiding

        [HideInInspector]
        [NonSerialized]
        public float showTime = 4.0f;

        public DateTime dateTime { get; private set; }

        public virtual void Awake()
        {
            animator = GetComponent<Animator>();

            if (showAnimation != null)
                animator.Play(showAnimation.name);
        }

        public void ResetStateForPool()
        {
            isHiding = false;
        }

        public virtual void Repaint(InventoryNoticeMessage message)
        {
            showTime = (int)message.duration;
            dateTime = message.time;

            if (string.IsNullOrEmpty(message.title) == false)
            {
                if (title != null)
                {
                    title.text = string.Format(string.Format(format, message.title), message.parameters);
                    title.color = message.color;
                }
            }
            else
            {
                if (title != null) title.gameObject.SetActive(false);
            }


            var msg = string.Format(format, message.message);
            this.message.text = string.Format(msg, message.parameters);
            this.message.color = message.color;

            if (time != null)
            {
                time.text = dateTime.ToShortTimeString();
                time.color = message.color;
            }
        }

        public virtual void Hide()
        {
            // Already hiding
            if (isHiding)
                return;

            isHiding = true;

            if (hideAnimation != null)
                animator.Play(hideAnimation.name);
        }
    }
}