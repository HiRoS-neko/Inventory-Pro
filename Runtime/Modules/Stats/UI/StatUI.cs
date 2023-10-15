using System.Collections;
using Devdog.General;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Devdog.InventoryPro.UI
{
    public class StatUI : MonoBehaviour
    {
        [SerializeField]
        [Required]
        [ForceCustomObjectPicker]
        private StatDefinition _stat;

        [Header("Player")]
        public bool useCurrentPlayer = true;

        [Header("Interpolation")]
        public bool useValueInterpolation;

        public AnimationCurve interpolationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float interpolationSpeed = 1f;

        [Header("Visuals")]
        public Text statName;

        public UIShowValue visualizer = new();

        /// <summary>
        ///     The aim value used for interpolation.
        /// </summary>
        private float _aimStatValue;

        private float _deltaStatValue;
        public Player player;

        public StatDefinition stat
        {
            get => _stat;
            protected set => _stat = value;
        }

        protected virtual void Start()
        {
            if (useCurrentPlayer)
            {
                player = PlayerManager.instance.currentPlayer;

                PlayerManager.instance.OnPlayerChanged += OnPlayerChanged;
            }

            // Force a repaint.
            OnPlayerChanged(null, player);
        }

        protected void OnDestroy()
        {
//            if (useCurrentPlayer)
//            {
//                PlayerManager.instance.OnPlayerChanged -= OnPlayerChanged;
//            }

            if (player != null && player.InventoryPlayer() != null)
                player.InventoryPlayer().stats.Get(stat).OnValueChanged -= Repaint;
        }

        private void OnPlayerChanged(Player oldPlayer, Player newPlayer)
        {
            // Remove the old
            if (oldPlayer != null && oldPlayer.InventoryPlayer() != null)
                oldPlayer.InventoryPlayer().stats.Get(stat).OnValueChanged -= Repaint;

            player = newPlayer;

            // Add the new
            if (player != null && player.InventoryPlayer() != null)
            {
                var s = player.InventoryPlayer().stats.Get(stat);
                Assert.IsNotNull(s, "Given stat " + stat + " could not be found on player.");
                s.OnValueChanged += Repaint;
                Repaint(s);
            }
        }

        protected virtual void Repaint(IStat stat)
        {
            if (stat == null) return;

            if (statName != null) statName.text = stat.definition.statName;

            if (useValueInterpolation && gameObject.activeInHierarchy)
                StartCoroutine(_RepaintInterpolated(stat));
            else
                visualizer.Repaint(stat.currentValue, stat.currentMaxValue);
        }

        private IEnumerator _RepaintInterpolated(IStat stat)
        {
            _aimStatValue = stat.currentValue;
            var timer = 0f;
            while (timer < 1f)
            {
                var val = Mathf.Lerp(_deltaStatValue, _aimStatValue, interpolationCurve.Evaluate(timer));
                visualizer.Repaint(val, stat.currentMaxValue);

                timer += Time.deltaTime * interpolationSpeed;
                yield return null;
            }

            _deltaStatValue = _aimStatValue;
        }
    }
}