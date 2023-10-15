using System;
using Devdog.General;
using Devdog.InventoryPro.UI;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [Serializable]
    public class StatDefinition : ScriptableObject, IStatDefinition
    {
        //[HideInInspector]
        public int ID;

        [SerializeField]
        private bool _enabled = true;

        [SerializeField]
        [Required]
        private string _statName = "";

        [SerializeField]
        [Required]
        private string _category = "Default";

        [SerializeField]
        private bool _showInUI = true;

        [SerializeField]
        private StatRowUI _uiPrefab;

        [Tooltip("How the value is shown.\n{0} = Current amount\n{1} = Max amount\n{2} = Property name")]
        [SerializeField]
        private string _valueStringFormat = "{0}";

        [SerializeField]
        private Color _color = Color.white;

        [SerializeField]
        private Sprite _icon;

        /// <summary>
        ///     The base value is the start value of this property.
        /// </summary>
        [Tooltip("The base value is the start value of this property")]
        [SerializeField]
        private float _baseValue;

        [SerializeField]
        private float _maxValue = 100.0f;

        [SerializeField]
        private int _startLevel;

        [SerializeField]
        private bool _autoProgressLevels = true;


        [SerializeField]
        private StatLevel[] _levels = new StatLevel[0];

        public bool enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        public new string name
        {
            get => _statName;
            set => _statName = value;
        }

        public bool autoProgressLevels
        {
            get => _autoProgressLevels;
            set => _autoProgressLevels = value;
        }

        public string statName
        {
            get => _statName;
            set => _statName = value;
        }

        public string category
        {
            get => _category;
            set => _category = value;
        }

        public bool showInUI
        {
            get => _showInUI;
            set => _showInUI = value;
        }

        public StatRowUI uiPrefab
        {
            get => _uiPrefab;
            set => _uiPrefab = value;
        }

        public string valueStringFormat
        {
            get => _valueStringFormat;
            set => _valueStringFormat = value;
        }

        public Color color
        {
            get => _color;
            set => _color = value;
        }

        public Sprite icon
        {
            get => _icon;
            set => _icon = value;
        }

        public float baseValue
        {
            get => _baseValue;
            set => _baseValue = value;
        }

        public float maxValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        public int startLevel
        {
            get => _startLevel;
            set => _startLevel = value;
        }

        public StatLevel[] levels
        {
            get => _levels;
            set => _levels = value;
        }

        public bool Equals(IStatDefinition other)
        {
            return statName == other.statName &&
                   category == other.category;
        }

        public string ToString(IStat stat)
        {
            return ToString(stat, valueStringFormat);
        }

        /// <summary>
        ///     {0} = The current amount
        ///     {1} = The max amount
        ///     {2} = The stat name
        ///     {3} = The stat level
        ///     {4} = The stat experience
        ///     {5} = The stat required experience to next level (empty if last level)
        /// </summary>
        public string ToString(IStat stat, string overrideFormat)
        {
            try
            {
                if (stat == null) return string.Format(overrideFormat, 0f, maxValue, statName, 1, 0, 0);

                return string.Format(overrideFormat, stat.currentValue, maxValue, statName, stat.currentLevelIndex + 1,
                    stat.currentExperience,
                    levels.Length > stat.currentLevelIndex + 1
                        ? levels[stat.currentLevelIndex + 1].experienceRequired
                        : 0);
            }
            catch (Exception)
            {
                // Ignored
            }

            return "(Formatting not valid)";
        }

        public string ToString(object value)
        {
            return ToString(value, valueStringFormat);
        }

        public string ToString(object value, string overrideFormat)
        {
            try
            {
                return string.Format(overrideFormat, value, maxValue, statName, 1, 0, 0);
            }
            catch (Exception)
            {
                // Ignored
            }

            return "(Formatting not valid)";
        }

        public override string ToString()
        {
            return statName;
        }
    }
}