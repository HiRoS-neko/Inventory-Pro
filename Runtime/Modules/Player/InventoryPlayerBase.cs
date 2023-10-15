using UnityEngine;
using UnityEngine.Assertions;

namespace Devdog.InventoryPro
{
    public abstract class InventoryPlayerBase : MonoBehaviour
    {
        [SerializeField]
        private CharacterUI _characterUI;

        [SerializeField]
        private ItemCollectionBase[] _inventoryCollections = new ItemCollectionBase[0];

        [SerializeField]
        private SkillbarUI _skillbarCollection;

        ///////// Instantiation stuff
        public bool dynamicallyFindUIElements;

        public string characterCollectionName = "Character";
        public string[] inventoryCollectionNames = { "Inventory" };
        public string skillbarCollectionName = "Skillbar";


        [SerializeField]
        private CharacterEquipmentTypeBinder[] _equipmentBinders = new CharacterEquipmentTypeBinder[0];

        public CharacterUI characterUI
        {
            get => _characterUI;
            set => _characterUI = value;
        }

        public ItemCollectionBase[] inventoryCollections
        {
            get => _inventoryCollections;
            set => _inventoryCollections = value;
        }

        public SkillbarUI skillbarCollection
        {
            get => _skillbarCollection;
            set => _skillbarCollection = value;
        }

        public CharacterEquipmentTypeBinder[] equipmentBinders
        {
            get => _equipmentBinders;
            set => _equipmentBinders = value;
        }


        protected virtual void Awake()
        {
        }

        protected virtual void UpdateEquipLocations(Transform rootTransform)
        {
            foreach (var equipLoc in equipmentBinders)
                if (equipLoc.findDynamic && string.IsNullOrEmpty(equipLoc.equipTransformPath) == false)
                {
                    Transform equipTransform = null;
                    InventoryUtility.FindChildTransform(rootTransform, equipLoc.equipTransformPath, ref equipTransform);
                    equipLoc.equipTransform = equipTransform;

                    Assert.IsNotNull(equipLoc.equipTransform,
                        "Equip transform path is not valid (" + equipLoc.equipTransformPath + ")");
                }
        }

        protected virtual void SetAsActivePlayer()
        {
        }

        public virtual void FindUIElements(bool warnWhenNotFound = true)
        {
            characterUI = FindElement<CharacterUI>(characterCollectionName, warnWhenNotFound);
            inventoryCollections = FindUIElements<ItemCollectionBase>(inventoryCollectionNames, warnWhenNotFound);
            skillbarCollection = FindElement<SkillbarUI>(skillbarCollectionName, warnWhenNotFound);
        }

        public T[] FindUIElements<T>(string[] collectionNames, bool warnWhenNotFound) where T : ItemCollectionBase
        {
            var comps = new T[collectionNames.Length];
            for (var i = 0; i < collectionNames.Length; i++)
                comps[i] = FindElement<T>(collectionNames[i], warnWhenNotFound);

            return comps;
        }

        public T FindElement<T>(string collectionName, bool warnWhenNotFound) where T : ItemCollectionBase
        {
            if (string.IsNullOrEmpty(collectionName)) return null;

            var a = ItemCollectionBase.FindByName<T>(collectionName);
            if (a == null && warnWhenNotFound)
                Debug.LogWarning("Player instantiation :: Collection with name (" + collectionName + ") not found!");

            return a;
        }
    }
}