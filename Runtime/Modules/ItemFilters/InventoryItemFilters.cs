﻿using System;
using System.Linq;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [Serializable]
    public class InventoryItemFilters
    {
        public enum FiltersMatchType
        {
            MatchAll,
            MatchAny
        }

        public ItemFilter[] filters = new ItemFilter[0];
        public FiltersMatchType matchType = FiltersMatchType.MatchAll;


        public bool IsItemAbidingFilters(InventoryItemBase item)
        {
            if (filters.Length == 0) return true;

            switch (matchType)
            {
                case FiltersMatchType.MatchAll:

                    return filters.All(filter => filter.IsItemAbidingFilter(item));

                case FiltersMatchType.MatchAny:

                    return filters.Any(filter => filter.IsItemAbidingFilter(item));

                default:
                    Debug.LogWarning("Type " + matchType + " not found");
                    break;
            }

            return false;
        }
    }
}