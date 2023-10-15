using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [Serializable]
    public class StatDecorator : IEquatable<StatDecorator>
    {
        public enum ActionEffect
        {
            /// <summary>
            ///     Add to the bonus, increases the maximum
            /// </summary>
            Add,

            /// <summary>
            ///     Add to the bonus, increases the maximum
            /// </summary>
            AddExperience,

            ///// <summary>
            ///// Add to the maximum value
            ///// </summary>
            //IncreaseMax,

            /// <summary>
            ///     Restore the value (for example consumables, when eating an apple, restore the health).
            /// </summary>
            Restore,

            /// <summary>
            ///     Decrease the value by a set amount, if the user doesn't have enough of the property the action will be canceled.
            /// </summary>
            Decrease
        }

        [SerializeField]
        [Required]
        private StatDefinition _stat;

        public string value;

        /// <summary>
        ///     (1 = value * 1.0f, 0.1f = value * 0.1f so 10%).
        /// </summary>
        public bool isFactor;

        //public bool increaseMax = false; // Increase max or add to?
        public ActionEffect actionEffect = ActionEffect.Restore;


        public StatDecorator()
        {
        }

        public StatDecorator(StatDecorator copyFrom)
        {
            stat = copyFrom.stat;
            value = copyFrom.value;
            isFactor = copyFrom.isFactor;
            actionEffect = copyFrom.actionEffect;
        }

        public StatDefinition stat
        {
            get => _stat;
            set => _stat = value;
        }


        public int intValue
        {
            get
            {
                var v = 0;
                int.TryParse(value, out v);

                return v;
            }
            set => this.value = value.ToString();
        }

        public float floatValue
        {
            get
            {
                var v = 0.0f;
                float.TryParse(value, out v);

                return v;
            }
            set => this.value = value.ToString();
        }

        public bool isSingleValue
        {
            get
            {
                float v;
                return float.TryParse(value, out v);
            }
        }

        public string stringValue
        {
            get => value;
            set => this.value = value;
        }

        public bool boolValue
        {
            get => bool.Parse(value);
            set => this.value = value ? "true" : "false";
        }

        public bool Equals(StatDecorator other)
        {
            if (other == null) return false;

            if (stat == null && other.stat == null) return true;

            if (stat == null || other.stat == null) return false;

            if (stat.Equals(other.stat) == false) return false;

            return value == other.value &&
                   isFactor == other.isFactor &&
                   actionEffect == other.actionEffect;
        }

        public bool CanDoDecrease(InventoryPlayer player)
        {
            var prop = player.stats.Get(stat.category, stat.statName);
            if (prop != null)
                if (prop.currentValue >= floatValue)
                    return true;

            return false;
        }

        public override string ToString()
        {
            return stat.ToString(floatValue);
        }
    }
}