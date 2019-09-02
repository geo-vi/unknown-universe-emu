using System;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships.items;

namespace Server.Game.objects.errors
{
    class ItemException : Exception
    {
        private Player Player { get; set; }
        
        private string LootId { get; set; }
        
        public ItemException(Player player, string lootId)
        {
            Player = player;
            LootId = lootId;
        }

        public override string Message => "Item " + LootId + " dropped an error for Player " + Player.Name + " / ID: " + Player.Id;
    }
}