using System;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [Serializable]
    public class InventoryNoticeMessage : InventoryMessage
    {
        public Color color = Color.white;
        public NoticeDuration duration = NoticeDuration.Medium;
        public DateTime time;


        /// <summary>
        ///     Required for PlayMaker...
        /// </summary>
        public InventoryNoticeMessage()
        {
        }

        public InventoryNoticeMessage(string title, string message, NoticeDuration duration, params object[] parameters)
            : this(title, message, duration, Color.white, DateTime.Now, parameters)
        {
        }

        public InventoryNoticeMessage(string title, string message, NoticeDuration duration, Color color,
            params object[] parameters)
            : this(title, message, duration, color, DateTime.Now, parameters)
        {
        }

        public InventoryNoticeMessage(string title, string message, NoticeDuration duration, Color color, DateTime time,
            params object[] parameters)
        {
            this.title = title;
            this.message = message;
            this.color = color;
            this.time = time;
            this.parameters = parameters;
        }

        public override void Show(params object[] param)
        {
            base.Show(param);

            time = DateTime.Now;

            if (InventoryManager.instance.notice != null)
                InventoryManager.instance.notice.AddMessage(this);
        }
    }
}