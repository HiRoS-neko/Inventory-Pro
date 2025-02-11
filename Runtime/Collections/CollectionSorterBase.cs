﻿using System.Collections.Generic;
using UnityEngine;

namespace Devdog.InventoryPro
{
    public abstract class CollectionSorterBase : ScriptableObject
    {
        public abstract IList<InventoryItemBase> Sort(IList<InventoryItemBase> items);
    }
}