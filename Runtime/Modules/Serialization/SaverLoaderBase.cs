using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Devdog.InventoryPro
{
    [HelpURL("http://devdog.nl/documentation/serialization-saving-loading/")]
    public abstract class SaverLoaderBase : MonoBehaviour, IItemsSaver, IItemsLoader
    {
        protected IItemsSerializer serializer { get; set; }
        protected IItemsSaver saver { get; set; }
        protected IItemsLoader loader { get; set; }

        protected const string SaveNamePrefix = "InventoryPro_";

        public abstract string saveName { get; }

        public bool loadOnStart = true;
        public bool loadOnLevelLoad;
        public bool saveOnApplicationQuit = true;
        public bool saveOnApplicationPause; // Useful for mobile devices.
        public bool loadOnApplicationResume;

        /// <summary>
        ///     How many frames to wait before loading the data.
        ///     This can be useful for 3rd party assets that take longer to initialize.
        /// </summary>
        public int waitFramesBeforeLoading;

        protected virtual void Awake()
        {
            SetSerializer();
            SetSaverLoader();
        }

        protected virtual IEnumerator Start()
        {
            for (var i = 0; i < waitFramesBeforeLoading; i++) yield return null;

            if (loadOnStart) Load();

#if UNITY_5_4_OR_NEWER
            SceneManager.sceneLoaded += DoInit;
#endif
        }

        protected void OnDestroy()
        {
#if UNITY_5_4_OR_NEWER
            SceneManager.sceneLoaded -= DoInit;
#endif
        }

#if UNITY_5_4_OR_NEWER
        protected void DoInit(Scene scene, LoadSceneMode loadMode)
        {
            StartCoroutine(Init());
        }
#endif

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
        public void OnLevelWasLoaded(int level)
        {
            StartCoroutine(Init());
        }

#endif

        protected virtual IEnumerator Init()
        {
            SetSerializer();
            SetSaverLoader();

            for (var i = 0; i < waitFramesBeforeLoading; i++) yield return null;

            if (loadOnLevelLoad && isActiveAndEnabled) Load();
        }


        protected virtual void OnApplicationQuit()
        {
            if (saveOnApplicationQuit && isActiveAndEnabled) Save();
        }

        protected virtual void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                if (saveOnApplicationPause && isActiveAndEnabled) Save();
            }
            else
            {
                if (loadOnApplicationResume && isActiveAndEnabled) Load();
            }
        }

        public abstract void Save();
        public abstract void Load();

        protected virtual void SetSerializer()
        {
            serializer = new JsonItemsSerializer();
        }

        protected virtual void SetSaverLoader()
        {
            saver = this;
            loader = this;
        }

        public virtual void SaveItems(object serializedData)
        {
            SaveItems(serializedData, b => { });
        }

        public abstract void SaveItems(object serializedData, Action<bool> callback);
        public abstract void LoadItems(Action<object> callback);
    }
}