using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro.UI
{
    public class CurrencyUI : MonoBehaviour
    {
        public ItemCollectionBase collection;

        [SerializeField]
        [Required]
        private CurrencyDefinition _currency;

        public CurrencyUIElement currencyUIElement;

        public CurrencyDefinition currency
        {
            get => _currency;
            protected set => _currency = value;
        }

        protected virtual void Awake()
        {
            currencyUIElement.Reset();
        }

        protected virtual void Start()
        {
            if (collection != null)
            {
                collection.OnCurrencyChanged += OnCurrencyChanged;
                currencyUIElement.Repaint(collection.currenciesGroup.GetCurrency(currency));
            }
        }

        protected virtual void OnDestroy()
        {
            if (collection != null) collection.OnCurrencyChanged -= OnCurrencyChanged;
        }

        protected virtual void OnCurrencyChanged(float amountBefore, CurrencyDecorator decorator)
        {
            if (decorator.currency == currency) currencyUIElement.Repaint(decorator);
        }
    }
}