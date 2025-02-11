﻿using Devdog.General;
using Devdog.General.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    /// <summary>
    ///     Used to define a category of stats.
    /// </summary>
    public class StatCategoryUI : MonoBehaviour, IPoolable
    {
        /// <summary>
        ///     Name of the category
        /// </summary>
        [SerializeField]
        protected Text categoryName;

        [SerializeField]
        protected Button foldButton;

        [Required]
        public RectTransform container;

        public bool expandToParentSize;

        private LayoutElement containerLayoutElement;

        /// <summary>
        ///     Check if the item is folded or not.
        /// </summary>
        protected bool isVisible = true;


        protected virtual void Awake()
        {
            containerLayoutElement = container.GetComponent<LayoutElement>();
            if (containerLayoutElement == null)
                containerLayoutElement = container.gameObject.AddComponent<LayoutElement>();

            if (foldButton != null) foldButton.onClick.AddListener(OnFoldButtonClicked);

            if (expandToParentSize) UIUtility.InheritParentSize(transform);
        }

        public virtual void ResetStateForPool()
        {
            isVisible = true;
        }

        protected virtual void OnFoldButtonClicked()
        {
            isVisible = !isVisible;
            containerLayoutElement.gameObject.SetActive(isVisible);
        }

        public virtual void Repaint(string categoryName)
        {
            this.categoryName.text = categoryName;
        }
    }
}