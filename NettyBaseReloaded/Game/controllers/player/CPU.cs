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
            JCPU,
            AUTO_ROK,
            AUTO_ROCKLAUNCHER
        }

        public List<Types> Active = new List<Types>();

        private PlayerController baseController;

        public CPU(PlayerController controller)
        {
            baseController = controller;
            
            var activeCPUs = baseController.Player.Settings.OldClientShipSettingsCommand.activeCpus;
            foreach(string activeCPU in activeCPUs)
            {
                switch (activeCPU)
                {
                    case "equipment_extra_cpu_ajp-01":
                        // TODO add Jump
                        break;
                    case "equipment_extra_repbot_rep-s":
                    case "equipment_extra_repbot_rep-1":
                    case "equipment_extra_repbot_rep-2":
                    case "equipment_extra_repbot_rep-3":
                    case "equipment_extra_repbot_rep-4":
                        Active.Add(Types.ROBOT);
                        break;
                    case "equipment_extra_cpu_aim-01":
                    case "equipment_extra_cpu_aim-02":
                        // TODO: Add aim cpu
                        break;
                    case "equipment_extra_cpu_jp-01":
                    case "equipment_extra_cpu_jp-02":
                        // TODO: Add jump2base
                        break;
                    case "equipment_extra_cpu_cl04k-xl":
                    case "equipment_extra_cpu_cl04k-m":
                    case "equipment_extra_cpu_cl04k-xs":
                        Active.Add(Types.CLOAK);
                        break;
                    case "equipment_extra_cpu_arol-x":
                        Active.Add(Types.AUTO_ROK);
                        break;
                    case "equipment_extra_cpu_rllb-x":
                        Active.Add(Types.AUTO_ROCKLAUNCHER);
                        break;
                    case "equipment_extra_cpu_dr-01":
                    case "equipment_extra_cpu_dr-02":
                        // TODO: add drone rep
                        break;
                }
            }
            
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
                case Types.AUTO_ROK:
                    var arExtra = baseController.Player.Extras.Values.FirstOrDefault(x => x is AutoRocket);
                    if (arExtra != null)
                    {
                        Packet.Builder.LegacyModule(gameSession, "0|A|CPU|R|" + Convert.ToInt32(arExtra.Active));
                    }
                    break;

                case Types.AUTO_ROCKLAUNCHER:
                    var arlExtra = baseController.Player.Extras.Values.FirstOrDefault(x => x is AutoRocketLauncher);
                    if (arlExtra != null)
                    {
                        Packet.Builder.LegacyModule(gameSession, "0|A|CPU|Y|" + Convert.ToInt32(arlExtra.Active));
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
                case Types.AUTO_ROK:
                    AutoRocket();
                    break;
                case Types.AUTO_ROCKLAUNCHER:
                    AutoRocketLauncher();
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

            baseController.Heal.Execute(repAmount);
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

        void AutoRocket() {
            var arExtra = baseController.Player.Extras.FirstOrDefault(x => x.Value is AutoRocket);
            var autoRocket = arExtra.Value;
            if (autoRocket != null && autoRocket.Amount > 0)
            {
                if (autoRocket.Active)
                {
                    Active.Remove(Types.AUTO_ROK);
                    autoRocket.Active = false;
                }
                else
                {
                    Active.Add(Types.AUTO_ROK);
                    autoRocket.Active = true;
                }

                Update(Types.AUTO_ROK);
            }
        }

        void AutoRocketLauncher()
        {
            var arlExtra = baseController.Player.Extras.FirstOrDefault(x => x.Value is AutoRocketLauncher);
            var autoRocketLauncher = arlExtra.Value;
            if (autoRocketLauncher != null && autoRocketLauncher.Amount > 0)
            {
                if (autoRocketLauncher.Active)
                {
                    Active.Remove(Types.AUTO_ROCKLAUNCHER);
                    autoRocketLauncher.Active = false;
                }
                else
                {
                    Active.Add(Types.AUTO_ROCKLAUNCHER);
                    autoRocketLauncher.Active = true;
                }

                Update(Types.AUTO_ROCKLAUNCHER);
            }
        }

        void DroneRepair()
        {

        }

        void JCPU()
        {

        }
    }
}
