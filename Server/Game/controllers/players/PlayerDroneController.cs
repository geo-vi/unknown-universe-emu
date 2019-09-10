using Server.Game.controllers.characters;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;

namespace Server.Game.controllers.players
{
    class PlayerDroneController : DroneController
    {
        private Player Player
        {
            get
            {
                var player = Character as Player;
                return player;
            }
        }
    }
}