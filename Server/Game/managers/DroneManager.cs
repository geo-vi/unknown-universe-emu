using Server.Game.controllers.characters;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.entities.ships;
using Server.Game.objects.entities.ships.items;
using Server.Game.objects.enums;

namespace Server.Game.managers
{
    class DroneManager
    {
        public static DroneManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DroneManager();
                }

                return _instance;
            }
        }

        private static DroneManager _instance;

        public void DamageAllDrones(Player player, int damagePoints)
        {
            foreach (var drone in player.Hangar.Drones)
            {
                var currentDroneDamage = drone.Value.Damage;
                drone.Value.SetDroneDamage(currentDroneDamage - damagePoints);
            }
        }

        public void DestroyAllDrones(Player player)
        {
            
        }
        
        public void DestroyDrone(Character character, Drone drone)
        {
            character.Controller.GetInstance<DroneController>().DestroyDrone(drone);
        }

        public void ChangeDroneFormation(Character character, DroneFormation formation)
        {
            character.ChangeDroneFormation(formation);
        }

        public void ChangeDroneFormation(Player player, int formationId)
        {
            var droneFormation = ItemMap.FormationIds[formationId];
            if (GameItemManager.Instance.ExistInInventory(player, droneFormation) && formationId != 0)
            {
                player.ChangeDroneFormation((DroneFormation) formationId);
                PrebuiltPlayerCommands.Instance.ChangeDroneFormationCommand(player);
            }
        }
    }
}