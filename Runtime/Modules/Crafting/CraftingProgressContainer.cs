using System.Collections;
using System.Collections.Generic;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    public class CraftingProgressContainer
    {
        public CraftingProgressContainer(ICraftingActionValidator validator, int instanceID, AudioSource audioSource)
        {
            this.validator = validator;
            this.audioSource = audioSource;
            this.instanceID = instanceID;
            maxCraftQueueCount = 1;

            craftQueue = new Queue<CraftInfo>(maxCraftQueueCount);
        }

        public IEnumerable<CraftInfo> activeCraftQueueEnumerable => craftQueue;

        protected Queue<CraftInfo> craftQueue { get; set; }

        public CraftInfo activeCraft
        {
            get
            {
                if (craftQueue.Count > 0) return craftQueue.Peek();

                return null;
            }
        }

        public float currentCraftProgress { get; protected set; }

        public bool isCrafting => craftQueue.Count > 0;

        public bool removeItemsOnCraftStart { get; set; }
        public int maxCraftQueueCount { get; set; }


        public ICraftingActionValidator validator { get; set; }

        public AudioSource audioSource { get; set; }
//        public ItemCollectionBase removeFromCollection { get; set; }
//        public ItemCollectionBase rewardsCollection { get; set; }


        public int instanceID { get; protected set; }

        /// <summary>
        ///     Crafts the item and triggers the coroutine method to handle the crafting itself.
        /// </summary>
        public virtual bool AddBlueprintToCraftingQueue(InventoryPlayer player, CraftingCategory category,
            CraftingBlueprint blueprint, int amount, ItemCollectionBase storeRewardsCollection,
            ItemCollectionBase removeItemsFromCollection)
        {
            var craftInfo = new CraftInfo(null, validator, audioSource, category, blueprint)
            {
                storeItemsInCollection = storeRewardsCollection,
                removeItemsFromCollection = removeItemsFromCollection,
                craftAmount = amount
            };

            return CraftItem(player, craftInfo);
        }


        protected virtual bool CraftItem(InventoryPlayer player, CraftInfo craftInfo)
        {
            if (craftQueue.Count >= maxCraftQueueCount) return false; // At max amount

            if (validator.CanCraftBlueprint(player, craftInfo) == false)
            {
                craftInfo.StopAudioSource();
                if (craftInfo == activeCraft) craftQueue.Dequeue();

                return false;
            }

            craftInfo.activeCraft = _CraftItem(player, craftInfo, craftInfo.blueprint.craftingTimeDuration);
            if (removeItemsOnCraftStart)
            {
                for (var i = 0; i < craftInfo.craftAmount; i++)
                    validator.RemoveRequiredCraftItemsAndCurrency(craftInfo);

                craftInfo.removedCraftItems = true;
            }

            craftQueue.Enqueue(craftInfo);
            validator.StartCoroutine(craftInfo.activeCraft);
            return true;
        }

        protected virtual IEnumerator _CraftItem(InventoryPlayer player, CraftInfo craftInfo, float currentCraftTime)
        {
            NotifyCraftStart(craftInfo);

            var counter = currentCraftTime;
            while (true)
            {
                yield return null;
                counter -= Time.deltaTime;
                NotifyCraftProgress(craftInfo, 1.0f - Mathf.Clamp01(counter / currentCraftTime));

                if (counter <= 0.0f) break;
            }

            if (craftInfo.removedCraftItems == false && validator.CanCraftBlueprint(player, activeCraft) == false)
            {
                craftInfo.StopAudioSource();
                yield break;
            }

            if (craftInfo.blueprint.successChanceFactor >= Random.value)
            {
                if (activeCraft.removedCraftItems == false) validator.RemoveRequiredCraftItemsAndCurrency(activeCraft);

                validator.GiveCraftReward(activeCraft);
                NotifyCraftSuccess(craftInfo);
            }
            else
            {
                NotifyCraftFailed(craftInfo);
            }

            craftInfo.craftAmount--;
            if (craftInfo.craftAmount > 0)
            {
                if (removeItemsOnCraftStart == false) activeCraft.removedCraftItems = false; // For the next cycle.

                validator.StartCoroutine(_CraftItem(player, craftInfo,
                    Mathf.Clamp(currentCraftTime / craftInfo.blueprint.craftingTimeSpeedupFactor, 0.0f,
                        craftInfo.blueprint.craftingTimeDuration)));
            }
            else
            {
                craftQueue.Dequeue();
            }
        }

        public virtual void CancelActiveCraft()
        {
            if (activeCraft != null)
            {
                validator.StopCoroutine(activeCraft.activeCraft);
                NotifyCraftCanceled(activeCraft, currentCraftProgress);

                craftQueue.Dequeue(); // Dequeue the one we just cancelled.
            }
        }

        public virtual void CancelActiveCraftAndClearQueue()
        {
            CancelActiveCraft();
            craftQueue.Clear();
        }

        public class CraftInfo
        {
            public CraftInfo(IEnumerator activeCraft, ICraftingActionValidator validator, AudioSource audioSource,
                CraftingCategory category, CraftingBlueprint blueprint)
            {
                this.activeCraft = activeCraft;
                this.validator = validator;
                this.audioSource = audioSource;
                this.category = category;
                this.blueprint = blueprint;
                craftAmount = 1;
            }

            public IEnumerator activeCraft { get; set; }
            public AudioSource audioSource { get; set; }
            public CraftingBlueprint blueprint { get; set; }
            public CraftingCategory category { get; set; }
            public int craftAmount { get; set; }
            public bool removedCraftItems { get; set; }
            public ItemCollectionBase storeItemsInCollection { get; set; }
            public ItemCollectionBase removeItemsFromCollection { get; set; }
            public ICraftingActionValidator validator { get; set; }

            public void PlayAudioSource(AudioClipInfo clip)
            {
                if (clip != null)
                {
                    if (audioSource == null)
                    {
                        Debug.LogWarning(
                            "Can't play crafting audio clip because there is no audio source attached to the trigger.");
                    }
                    else
                    {
                        audioSource.clip = clip.audioClip;
                        audioSource.volume = clip.volume;
                        audioSource.pitch = clip.pitch;
                        audioSource.loop = clip.loop;
                        audioSource.Play();
                    }
                }
            }

            public void StopAudioSource()
            {
                if (audioSource != null) audioSource.Stop();
            }
        }

        #region Events

        public event CraftingDelegates.CraftStart OnCraftStart;
        public event CraftingDelegates.CraftSuccess OnCraftSuccess;
        public event CraftingDelegates.CraftFailed OnCraftFailed;
        public event CraftingDelegates.CraftProgress OnCraftProgress;
        public event CraftingDelegates.CraftCanceled OnCraftCancelled;

        #endregion

        #region Notifies

        protected virtual void NotifyCraftStart(CraftInfo craftInfo)
        {
            var clip = craftInfo.category.craftingAudioClip;
//            if (craftInfo.blueprint.overrideCategoryAudioClips)
//            {
//                clip = craftInfo.blueprint.craftingAudioClip;
//            }

            activeCraft.PlayAudioSource(clip);

            if (OnCraftStart != null)
                OnCraftStart(craftInfo);
        }

        protected virtual void NotifyCraftSuccess(CraftInfo craftInfo)
        {
            InventoryManager.langDatabase.craftedItem.Show(craftInfo.blueprint.name, craftInfo.blueprint.description);

            var clip = craftInfo.category.successAudioClip;
//            if (craftInfo.blueprint.overrideCategoryAudioClips)
//            {
//                clip = craftInfo.blueprint.successAudioClip;
//            }

            activeCraft.PlayAudioSource(clip);

            if (OnCraftSuccess != null)
                OnCraftSuccess(craftInfo);
        }

        protected virtual void NotifyCraftFailed(CraftInfo craftInfo)
        {
            InventoryManager.langDatabase.craftingFailed.Show(craftInfo.blueprint.name,
                craftInfo.blueprint.description);

            var clip = craftInfo.category.failedAudioClip;
//            if (craftInfo.blueprint.overrideCategoryAudioClips)
//            {
//                clip = craftInfo.blueprint.failedAudioClip;
//            }

            activeCraft.PlayAudioSource(clip);


            if (OnCraftFailed != null)
                OnCraftFailed(craftInfo);
        }

        public virtual void NotifyCraftProgress(CraftInfo craftInfo, float progress)
        {
            currentCraftProgress = progress;

            if (OnCraftProgress != null)
                OnCraftProgress(craftInfo, progress);
        }

        protected virtual void NotifyCraftCanceled(CraftInfo craftInfo, float progress)
        {
            InventoryManager.langDatabase.craftingCanceled.Show(craftInfo.blueprint.name,
                craftInfo.blueprint.description, progress);

            var clip = craftInfo.category.canceledAudioClip;
//            if (craftInfo.blueprint.overrideCategoryAudioClips)
//            {
//                clip = craftInfo.blueprint.canceledAudioClip;
//            }

            activeCraft.PlayAudioSource(clip);

            if (OnCraftCancelled != null)
                OnCraftCancelled(craftInfo, currentCraftProgress);
        }

        #endregion
    }
}