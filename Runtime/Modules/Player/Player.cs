using System.Collections.Generic;
using Devdog.InventoryPro;

namespace Devdog.General
{
    public static class PlayerExtensions
    {
        private static readonly Dictionary<Player, InventoryPlayer> _inventoryPlayer = new();

        public static InventoryPlayer InventoryPlayer(this Player player)
        {
            if (!_inventoryPlayer.ContainsKey(player))
                _inventoryPlayer.Add(player, player.GetComponent<InventoryPlayer>());

            return _inventoryPlayer[player];
        }
    }
}