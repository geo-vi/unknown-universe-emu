using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.player
{
    class CPU : IChecker
    {
        public enum Types
        {
            ROBOT,
            CLOAK,
            JCPU
        }

        private PlayerController baseController;

        public CPU(PlayerController controller)
        {
            baseController = controller;
        }

        DateTime LastTimeCheckedCpus = new DateTime(2017, 1, 18, 0, 0, 0);

        public void Check()
        {
            if (LastTimeCheckedCpus.AddSeconds(1) > DateTime.Now) return;
            LastTimeCheckedCpus = DateTime.Now;

            if (baseController.Repairing) Robot();
        }

        public void Update(Types type)
        {
            var gameSession = World.StorageManager.GetGameSession(baseController.Character.Id);
            switch (type)
            {
                case Types.CLOAK:
                    var cloak = baseController.Player.Extras.Values.FirstOrDefault(x => x is Cloak);
                    if (cloak != null)
                    {
                        Packet.Builder.LegacyModule(gameSession, "0|A|CPU|C|" + cloak.Amount);
                    }
                    break;
            }
        }

        public void Activate(Types type)
        {
            switch (type)
            {
                case Types.ROBOT:
                    baseController.Repairing = true;
                    break;
                case Types.CLOAK:
                    Cloak();
                    break;
            }
        }

        public void DeActivate(Types type)
        {
            switch (type)
            {
                case Types.ROBOT:
                    baseController.Repairing = false;
                    break;
            }
        }

        void Robot()
        {
            if (baseController.Character.Moving || !baseController.Repairing ||
                baseController.Character.LastCombatTime.AddSeconds(3) > DateTime.Now)
            {
                baseController.Repairing = false;
                return;
            }

            var player = (Player)baseController.Character;
            if (player.CurrentHealth == player.MaxHealth)
            {
                baseController.Repairing = false;
                return;
            }

            var robot = player.Extras.Values.FirstOrDefault(x => x is Robot) as Robot;
            if (robot == null) return;

            var repAmount = (player.MaxHealth / 100) * robot.GetLevel();

            baseController.Attack.Heal(repAmount);
        }

        void Cloak()
        {
            var cloakExtra = baseController.Player.Extras.FirstOrDefault(x => x.Value is Cloak);
            if (cloakExtra.Value != null && cloakExtra.Value.Amount > 0)
            {
                baseController.Invisible = true;
                baseController.Player.Extras.FirstOrDefault(x => x.Value is Cloak).Value.Amount -= 1;
                Update(Types.CLOAK);
                baseController.Effects.UpdatePlayerVisibility();
            }
        }

        void AutoRocketLauncher()
        {

        }

        void DroneRepair()
        {

        }

        void JCPU()
        {

        }
    }
}
