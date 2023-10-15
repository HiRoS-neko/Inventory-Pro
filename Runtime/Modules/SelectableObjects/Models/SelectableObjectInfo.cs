using UnityEngine;

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Other/Selectable Object Info")]
    public class SelectableObjectInfo : MonoBehaviour, ISelectableObjectInfo
    {
        [SerializeField]
        private string _name;

        [SerializeField]
        private bool _useHealth = true;

        [SerializeField]
        private float _health = 100;

        [SerializeField]
        private float _maxHealth = 100;

        public bool isDead => health <= 0;

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            health = maxHealth;
        }

        public new string name
        {
            get => _name;
            set => _name = value;
        }

        public bool useHealth
        {
            get => _useHealth;
            set => _useHealth = value;
        }

        public float health
        {
            get => _health;
            set => _health = value;
        }

        public float maxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }


        public float healthFactor => health / maxHealth;

        public void ChangeHealth(float changeBy, bool fireEvents = true)
        {
            health += changeBy;
        }
    }
}