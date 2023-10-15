using System;

namespace Devdog.InventoryPro
{
    [Serializable]
    public partial class CraftingBlueprintLayout
    {
        public int ID;
        public bool enabled = true;
        public Row[] rows = new Row[0];


        public Row this[int i]
        {
            get => rows[i];
            set => rows[i] = value;
        }

        [Serializable]
        public partial class Row
        {
            public int index;

            public Cell[]
                columns = new Cell[0]; // Named columns to avoid breaking previously serialized data (copied through issue detector)

            public Cell this[int i]
            {
                get => columns[i];
                set => columns[i] = value;
            }

            [Serializable]
            public class Cell
            {
                public int index;
                public InventoryItemBase item;
                public int amount;
            }
        }
    }
}