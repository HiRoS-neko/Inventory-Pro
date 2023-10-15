using System.Linq;

namespace Devdog.InventoryPro
{
    public class StatSerializationModel
    {
        public float currentExperience;
        public float currentFactor;
        public float currentFactorMax;

        public float currentMaxValue;
        public float currentValueRaw;
        public int statID;

        public StatSerializationModel()
        {
        }

        public StatSerializationModel(Stat from)
        {
            FromStat(from);
        }

        public void FromStat(Stat from)
        {
            statID = ((StatDefinition)from.definition).ID;
            currentMaxValue = from.currentMaxValueRaw;
            currentFactorMax = from.currentFactorMax;
            currentFactor = from.currentFactor;
            currentValueRaw = from.currentValueRaw;
            currentExperience = from.currentExperience;
        }

        public Stat ToStat()
        {
            Stat dec;
            ToStat(out dec);

            return dec;
        }

        public void ToStat(out Stat to)
        {
            to = new Stat(ItemManager.database.statDefinitions.FirstOrDefault(o => o.ID == statID));
            to.SetMaxValueRaw(currentMaxValue, false, false);
            to.SetFactorMax(currentFactorMax, false, false);
            to.SetFactor(currentFactor, false);
            to.SetCurrentValueRaw(currentValueRaw, false);
            to.SetExperience(currentExperience);
        }
    }
}