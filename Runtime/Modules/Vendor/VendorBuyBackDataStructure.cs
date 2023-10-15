using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Devdog.InventoryPro
{
    public class VendorBuyBackDataStructure<T> where T : InventoryItemBase
    {
        private static Dictionary<string, List<T>> _dict;
        private readonly List<T> _list;


        private string _category;

        public VendorBuyBackDataStructure(int reserve, bool isShared, string category, int maxCount)
        {
            _list = new List<T>(reserve);
            _dict = new Dictionary<string, List<T>>(reserve);
            _dict[category] = new List<T>();
//            AddCategory(category);

            this.isShared = isShared;
            this.category = category;

            this.maxCount = maxCount;
        }

        public bool isShared { get; set; }

        public string category
        {
            get => _category;
            set
            {
                if (_dict.ContainsKey(value) == false)
                    _dict.Add(value, new List<T>(maxCount));

                _category = value;
            }
        }

        public int maxCount { get; set; }

        public void Add(T item)
        {
            if (isShared)
            {
                _dict[category].Add(item);
                if (_dict[category].Count > maxCount)
                {
                    Object.Destroy(_dict[category][0].gameObject);
                    _dict[category].RemoveAt(0);
                }
            }
            else
            {
                // Not shared add to object array
                _list.Add(item);
                if (_list.Count > maxCount)
                {
                    Object.Destroy(_list[0].gameObject);
                    _list.RemoveAt(0);
                }
            }
        }

        public void Remove(InventoryItemBase item, uint amount)
        {
            if (isShared)
            {
                _dict[category][(int)item.index].currentStackSize -= amount;
                if (_dict[category][(int)item.index].currentStackSize < 1)
                {
                    Object.Destroy(_dict[category][(int)item.index].gameObject);
                    _dict[category].RemoveAt((int)item.index);
                }
            }
            else
            {
                _list[(int)item.index].currentStackSize -= amount;
                if (_list[(int)item.index].currentStackSize < 1)
                {
                    Object.Destroy(_list[(int)item.index].gameObject);
                    _list.RemoveAt((int)item.index);
                }
            }
        }

        public T[] ToArray()
        {
            if (isShared) return _dict[category].ToArray();

            return _list.ToArray();
        }


        public int ItemCount(uint itemID)
        {
            if (isShared) return _dict[category].Sum(o => o.ID == itemID ? (int)o.currentStackSize : 0);

            return _list.Sum(o => o.ID == itemID ? (int)o.currentStackSize : 0);
        }
    }
}