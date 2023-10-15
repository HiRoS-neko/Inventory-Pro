using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.InventoryPro
{
//    [RequireComponent(typeof(ItemCollectionSlotUI))]
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "UI Helpers/Equippable slot")]
    public class EquippableSlot : MonoBehaviour
    {
        [SerializeField]
        private EquipmentType[] _equipmentTypes = new EquipmentType[0];

        private ICharacterCollection _characterCollection;

        private ItemCollectionSlotUIBase _slot;

        /// <summary>
        ///     Index of this slot
        /// </summary>
        public uint index => slot.index;

        public EquipmentType[] equipmentTypes
        {
            get => _equipmentTypes;
            protected set => _equipmentTypes = value;
        }

        public ICharacterCollection characterCollection
        {
            get
            {
                if (_characterCollection == null)
                {
                    _characterCollection = GetComponentsInParent<ICharacterCollection>(true).FirstOrDefault();
                    Assert.IsNotNull(_characterCollection,
                        "ICharacterCollection couldn't be found in parent. Equippable slot error!");
                }

                return _characterCollection;
            }
        }

        public ItemCollectionSlotUIBase slot
        {
            get
            {
                if (_slot == null)
                    _slot = GetComponent<ItemCollectionSlotUIBase>();

                return _slot;
            }
        }


        protected void Awake()
        {
        }

        protected void Start()
        {
        }
    }
}