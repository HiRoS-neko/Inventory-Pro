using System.Collections.Generic;
using Devdog.General;

namespace Devdog.InventoryPro
{
    public class SkillExampleInventoryItemType : InventoryItemBase
    {
        public AudioClipInfo audioClipWhenUsed = new();


        public override LinkedList<ItemInfoRow[]> GetInfo()
        {
            var info = base.GetInfo();
            info.Remove(info.First.Next);

            return info;
        }


        public override void NotifyItemUsed(uint amount, bool alsoNotifyCollection)
        {
            base.NotifyItemUsed(amount, alsoNotifyCollection);

            PlayerManager.instance.currentPlayer.InventoryPlayer().stats.SetAll(stats);
        }

        public override int Use()
        {
            var used = base.Use();
            if (used < 0)
                return used;

            NotifyItemUsed(1, true);
            AudioManager.AudioPlayOneShot(audioClipWhenUsed);

            return 1; // 1 item used
        }
    }
}