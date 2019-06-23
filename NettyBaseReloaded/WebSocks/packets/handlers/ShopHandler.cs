using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using Newtonsoft.Json;

namespace NettyBaseReloaded.WebSocks.packets.handlers
{
    class ShopHandler : IHandler
    {
        public void execute(WebSocketReceiver receiver, string[] packet)
        {
            try
            {
                var userId = int.Parse(packet[1]);
                var playerSession = World.StorageManager.GetGameSession(userId);
                if (playerSession == null) return;

                var player = playerSession.Player;
                switch (packet[2])
                {
                    case "update_ammo":
                        player.Information.Ammunitions = World.DatabaseManager.LoadAmmunition(player);
                        Packet.Builder.SendSlotbars(playerSession);
                        break;
                    case "new_drone":
                        player.Equipment.Reload();
                        Packet.Builder.DronesCommand(playerSession, player);
                        Packet.Builder.DroneFormationAvailableFormationsCommand(playerSession);
                        break;
                    case "new_pet":
                        player.Pet = World.DatabaseManager.LoadPet(player);
                        Packet.Builder.PetInitializationCommand(playerSession, player.Pet);
                        break;
                    case "refuel":
                        //todo
                        //player.Pet.Fuel = World.DatabaseManager.GetPetFuel();
                        break;
                    case "update_boosters":
                        player.Boosters = World.DatabaseManager.LoadBoosters(player);
                        Packet.Builder.AttributeBoosterUpdateCommand(playerSession);
                        break;
                    case "update_keys":
                        player.Information.UpdateExtraData();
                        player.Information.DisplayBootyKeys();
                        break;
                }
            }
            catch
            {

            }
        }
    }
}
