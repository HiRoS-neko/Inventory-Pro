using System;

namespace Devdog.InventoryPro
{
    [Serializable]
    public class InventoryMessage
    {
        public string title;
        public string message;

        public object[] parameters;

        /// <summary>
        ///     Required for PlayMaker...
        /// </summary>
        public InventoryMessage()
        {
        }

        public InventoryMessage(string title, string message, params object[] parameters)
        {
            this.title = title;
            this.message = message;
            this.parameters = parameters;
        }


        public virtual void Show(params object[] param)
        {
            if (param != null && param.Length > 0)
                parameters = param;

            // Not much going on here...
        }
    }
}