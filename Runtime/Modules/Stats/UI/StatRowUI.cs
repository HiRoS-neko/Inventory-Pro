﻿using Devdog.General;
using UnityEngine;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    /// <summary>
    ///     Used to define a row of stats.
    /// </summary>
    public class StatRowUI : MonoBehaviour, IPoolable
    {
        [SerializeField]
        protected Text statName;

        [SerializeField]
        protected UIShowValue statValue = new();

        [SerializeField]
        protected Image statIcon;

        [SerializeField]
        protected bool showLevels;

        public bool hideStatNameIfIconIsPresent;

        public IStat currentStat { get; protected set; }

        public void ResetStateForPool()
        {
            if (statName != null) statName.gameObject.SetActive(false);

            statValue.Repaint(0, 1f);
            if (statValue.textField != null) statValue.textField.color = Color.white;

            if (statIcon != null) statIcon.gameObject.SetActive(false);
        }

        public virtual void Repaint(IStat stat)
        {
            currentStat = stat;
            if (statName != null)
            {
                statName.text = currentStat.definition.statName;
                statName.color = currentStat.definition.color;

                statName.gameObject.SetActive(true);
            }

            if (statValue.textField != null) statValue.textField.color = currentStat.definition.color;


            if (showLevels)
                statValue.Repaint(currentStat.currentLevelIndex + 1, currentStat.definition.levels.Length);
            else
                statValue.Repaint(currentStat.currentValue, currentStat.currentMaxValue);

            if (string.IsNullOrEmpty(statValue.textFormat) && statValue.textField != null)
                statValue.textField.text = stat.ToString();

            if (statIcon != null)
            {
                statIcon.sprite = currentStat.definition.icon;
                statIcon.color = currentStat.definition.color;
                statIcon.gameObject.SetActive(true);

                if (currentStat.definition.icon == null)
                {
                    statIcon.gameObject.SetActive(false);
                }
                else
                {
                    if (hideStatNameIfIconIsPresent && statName != null) statName.gameObject.SetActive(false);
                }
            }
        }
    }
}