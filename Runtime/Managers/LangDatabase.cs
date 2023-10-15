using System;
using Devdog.General;
using UnityEngine;

namespace Devdog.InventoryPro
{
    [Serializable]
    [CreateAssetMenu(fileName = "LanguageDatabase.asset", menuName = InventoryPro.ProductName + "/Language Database")]
    public class LangDatabase : ScriptableObject
    {
        [Category("Item actions")]
        public InventoryNoticeMessage itemCannotBeDropped =
            new("", "Item {0} cannot be dropped", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage itemCannotBeStored =
            new("", "Item {0} cannot be stored", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage itemCannotBeUsed =
            new("", "Item {0} cannot be used", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage itemCannotBeUsedLevelToLow = new("",
            "Item {0} cannot be used required level is {2}", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage itemCannotBeSold =
            new("", "Item {0} cannot be sold", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage itemCannotBePickedUpToFarAway =
            new("", "Item {0} is too far away to pick up", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage itemIsInCooldown = new("", "Item {0} is in cooldown {2:0.00} more seconds",
            NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage itemCannotBeUsedToLowStat =
            new("", "Item {0} cannot be used {2} is to low", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage itemCannotBeUsedStatNotValid = new("",
            "Item {0} cannot be used {2} is to high or to low", NoticeDuration.Medium, Color.white);

        //public InventoryNoticeMessage cannotDropItem;

        [Category("Collections")]
        public InventoryNoticeMessage collectionDoesNotAllowType =
            new("", "Does not allow type", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage collectionFull = new("", "{2} is full", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage collectionExceedingMaxWeight =
            new("", "Item {0} is to heavy to pick up", NoticeDuration.Medium, Color.white);
        //public InventoryNoticeMessage collection;
        //public InventoryNoticeMessage collectionDoesNotAllowType;

        [Category("Triggers")]
        public InventoryNoticeMessage triggerCantBeUsedMissingItems =
            new("", "{0} can't be used. Missing {1}", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage triggerCantBeUsedFailedStatRequirements = new("",
            "{0} can't be used. {1} is to high or to low", NoticeDuration.Medium, Color.white);


        [Category("User actions")]
        public InventoryNoticeMessage
            userNotEnoughGold = new("", "Not enough gold", NoticeDuration.Medium, Color.white);


        [Category("Crafting")]
        public InventoryNoticeMessage craftedItem =
            new("", "Successfully crafted {0}", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage craftingFailed =
            new("", "Crafting item {0} failed", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage craftingCanceled =
            new("", "Crafting item {0} canceled", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage craftingDontHaveRequiredItems = new("",
            "You don't have the required items to craft {2}", NoticeDuration.Long, Color.white);

        public InventoryNoticeMessage craftingCannotStatNotValid = new("",
            "Item {0} cannot be crafted {2} is to high or to low", NoticeDuration.Medium, Color.white);


        [Category("Vendor")]
        public InventoryNoticeMessage vendorCannotSellItem =
            new("", "Cannot sell item {0} to this vendor.", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage vendorSoldItemToVendor =
            new("", "Sold {2}x {0} to vendor {3} for {4}.", NoticeDuration.Medium, Color.white);

        public InventoryNoticeMessage vendorBoughtItemFromVendor = new("", "Bought {2}x {0} from vendor {3} for {4}.",
            NoticeDuration.Medium, Color.white);


        [Category("Dialogs")]
        public InventoryMessage confirmationDialogDrop = new("Are you sure?", "Are you sure you want to drop {0}?");

        public InventoryMessage unstackDialog = new("Unstack item {0}", "How many do you want to unstack?");
    }
}