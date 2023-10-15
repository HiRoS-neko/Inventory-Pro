using Devdog.General;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
#endif

namespace Devdog.InventoryPro
{
    [AddComponentMenu(InventoryPro.AddComponentMenuPath + "Managers/Item manager")]
    public class ItemManager : ManagerBase<ItemManager>
    {
        private static InventoryDatabaseLookup<ItemDatabase> _itemDatabaseLookup;

        [Required]
        [SerializeField]
        [FormerlySerializedAs("itemDatabase")]
        private ItemDatabase _sceneItemDatabase;

        public ItemDatabase sceneItemDatabase
        {
            get => _sceneItemDatabase;
            set => _sceneItemDatabase = value;
        }

        public static InventoryDatabaseLookup<ItemDatabase> itemDatabaseLookup
        {
            get
            {
                if (_itemDatabaseLookup == null)
                    _itemDatabaseLookup =
                        new InventoryDatabaseLookup<ItemDatabase>(instance != null ? instance.sceneItemDatabase : null,
                            CurrentItemDBPathKey);

                return _itemDatabaseLookup;
            }
        }

        private static string CurrentDBPrefixName
        {
            get
            {
                var path = Application.dataPath;
                if (path.Length > 0)
                {
                    var pathElems = path.Split('/');
                    return pathElems[pathElems.Length - 2];
                }

                return "";
            }
        }

        private static string CurrentItemDBPathKey => CurrentDBPrefixName + "InventorySystem_CurrentItemDatabasePath";

        public static ItemDatabase database
        {
            get => itemDatabaseLookup.GetDatabase();
            private set => itemDatabaseLookup.defaultDatabase = value;
        }

        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            if (itemDatabaseLookup == null)
                Debug.LogError("Item Database is not assigned!", transform);

#endif
        }

        public static void ResetItemDatabaseLookup()
        {
            _itemDatabaseLookup = null;
            _instance = null;
        }
    }
}

// using UnityEditor;