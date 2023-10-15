using System;
using System.Linq;
using Devdog.General;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Devdog.InventoryPro
{
    [Serializable]
    public class StatLevel
    {
        public float maxValue;
        public float experienceRequired;

        [Header("Unlock")]
//        public float changeValueOnUnlock = 0f;
        public GameObject effectOnUnlock;

        public StatLevel()
        {
        }

        public StatLevel(float maxValue, float experienceRequired)
        {
            this.maxValue = maxValue;
            this.experienceRequired = experienceRequired;
        }

        public void Unlock(IStat parent)
        {
#if UNITY_EDITOR
            DevdogLogger.LogVerbose("Unlocked stat level on stat: " + parent.definition.statName + " Level is now: " +
                                    parent.definition.levels.ToList().IndexOf(this));
#endif
            if (effectOnUnlock != null) Object.Instantiate(effectOnUnlock);
        }
    }
}