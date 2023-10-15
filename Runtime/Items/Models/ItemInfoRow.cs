using UnityEngine;

namespace Devdog.InventoryPro
{
    public class ItemInfoRow
    {
        /// <summary>
        ///     Text of the label.
        /// </summary>
        public string text;

        public Color textColor;

        /// <summary>
        ///     Title of the label.
        /// </summary>
        public string title;

        public Color titleColor;


        public ItemInfoRow()
        {
        }

        public ItemInfoRow(string title, Color color)
            : this(title, string.Empty, color, Color.white)
        {
        }

        public ItemInfoRow(string title, string text)
            : this(title, text, Color.white, Color.white)
        {
        }

        public ItemInfoRow(string title, string text, Color titleColor, Color textColor)
        {
            this.title = title;
            this.text = text;
            this.titleColor = titleColor;
            this.textColor = textColor;
        }
    }
}