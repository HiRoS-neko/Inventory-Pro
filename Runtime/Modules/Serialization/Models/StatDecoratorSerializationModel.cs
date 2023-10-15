using System.Linq;

namespace Devdog.InventoryPro
{
    public class StatDecoratorSerializationModel
    {
        public StatDecorator.ActionEffect actionEffect;
        public bool isFactor;
        public int statID;
        public string value;

        public StatDecoratorSerializationModel()
        {
        }

        public StatDecoratorSerializationModel(StatDecorator decorator)
        {
            FromStat(decorator);
        }

        public void FromStat(StatDecorator dec)
        {
            statID = dec.stat.ID;
            value = dec.value;
            actionEffect = dec.actionEffect;
            isFactor = dec.isFactor;
        }

        public StatDecorator ToStat()
        {
            var dec = new StatDecorator();
            ToStat(dec);

            return dec;
        }

        public void ToStat(StatDecorator dec)
        {
            dec.stat = ItemManager.database.statDefinitions.FirstOrDefault(o => o.ID == statID);
            dec.value = value;
            dec.actionEffect = actionEffect;
            dec.isFactor = isFactor;
        }
    }
}