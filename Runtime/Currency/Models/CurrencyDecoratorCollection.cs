using System.Collections.Generic;
using System.Linq;

namespace Devdog.InventoryPro
{
    //[Serializable]
    public class CurrencyDecoratorCollection
    {
        public CurrencyDecoratorCollection(bool loadAllCurrencies)
        {
            lookups = new List<CurrencyDecorator>(ItemManager.database.currencies.Length);

            if (loadAllCurrencies) LoadCurrencyDecorators();
        }

        public List<CurrencyDecorator> lookups { get; set; }

        public void AddCurrency(CurrencyDecorator decorator)
        {
            decorator.parentGroup = this;
            lookups.Add(decorator);
        }

        public void RemoveCurrency(CurrencyDecorator decorator)
        {
            lookups.Remove(decorator);
        }

        public CurrencyDecorator GetCurrency(CurrencyDefinition currency)
        {
            return lookups.FirstOrDefault(o => o.currency == currency);
        }

        private void LoadCurrencyDecorators()
        {
            foreach (var currency in ItemManager.database.currencies)
                lookups.Add(new CurrencyDecorator(currency) { parentGroup = this });
        }
    }
}