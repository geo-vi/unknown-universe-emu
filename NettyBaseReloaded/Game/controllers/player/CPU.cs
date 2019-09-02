using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;

namespace NettyBaseReloaded.Game.controllers.player
{
    class CPU : IChecker
    {
        public enum Types
        {
            ROBOT,
            CLOAK,
            JCPU,
            ADVANCED_JCPU,
            AUTO_ROK,
            AUTO_ROCKLAUNCHER,
            AIM_CPU,
            AUTO_REPAIR,
            DRO_CPU,
            TRADE_DRONE
        }

        public List<Types> Active = new List<Types>();

        private PlayerController baseController;

        private object ThreadLock = new object();

        public CPU(PlayerController controller)
        {
            baseController = controller;
        }

        public void LoadCpus()
        {
            foreach (var extra in baseController.Player.Extras)
            {
                if (extra.Value is AutoRocket && extra.Value.Active)
                {
                    Active.Add(Types.AUTO_ROK);
                    Update(Types.AUTO_ROK);
                }
                else if (extra.Value is AutoRocketLauncher && extra.Value.Active)
                {
                    Active.Add(Types.AUTO_ROCKLAUNCHER);
                    Update(Types.AUTO_ROCKLAUNCHER);
                }
                else if (extra.Value is Cloak)
                {
                    Update(Types.CLOAK);
                }
                else if (extra.Value is AimCpu)
                {
                    Update(Types.AIM_CPU);
                }
                else if (extra.Value is DROCpu)
                {
                    Update(Types.DRO_CPU);
                }
                else if (extra.Value is TradeDrone)
                {
                    Update(Types.TRADE_DRONE);
                }
                else if (extra.Value is JumpCpu)
                {
                    Update(Types.JCPU);
                }
                extra.Value.initiate();
            }
        }


        DateTime LastTimeCheckedCpus;
        public void Check()
        {
            if (LastTimeCheckedCpus.AddSeconds(1) > DateTime.Now) return;
            LastTimeCheckedCpus = DateTime.Now;

            if (baseController.Player.Extras.Any(x => x.Value is AutoRepair)) baseController.Repairing = true;
            if (baseController.Repairing) Robot();
            if (JumpSequenceActive) AdvancedJCPU();
        }

        public void Update(Types type)
        {
            lock (ThreadLock)
            {
                var gameSession = World.StorageManager.GetGameSession(baseController.Character.Id);
                switch (type)
                {
                    case Types.CLOAK:
                        var cloak = baseController.Player.Extras.Values.FirstOrDefault(x => x is Cloak);
                        if (cloak != null)
                        {
                            Packet.Builder.LegacyModule(gameSession, "0|A|CPU|C|" + cloak.Amount, true);
                        }

                        break;
                    case Types.AUTO_ROK:
                        var arExtra = baseController.Player.Extras.Values.FirstOrDefault(x => x is AutoRocket);
                        if (arExtra != null)
                        {
                            Packet.Builder.LegacyModule(gameSession, "0|A|CPU|R|" + Convert.ToInt32(arExtra.Active),
                                true);
                        }

                        break;

                    case Types.AUTO_ROCKLAUNCHER:
                        var arlExtra = baseController.Player.Extras.Values.FirstOrDefault(x => x is AutoRocketLauncher);
                        if (arlExtra != null)
                        {
                            Packet.Builder.LegacyModule(gameSession, "0|A|CPU|Y|" + Convert.ToInt32(arlExtra.Active),
                                true);
                        }

                        break;
                    case Types.TRADE_DRONE:
                        var tradeDrone = baseController.Player.Extras.Values.FirstOrDefault(x => x is TradeDrone);
                        if (tradeDrone != null)
                        {
                            Packet.Builder.LegacyModule(gameSession, "0|A|CPU|T|" + tradeDrone.Amount, true);
                        }

                        break;
                    case Types.AIM_CPU:
                        var aimExtra = baseController.Player.Extras.Values.FirstOrDefault(x => x is AimCpu);
                        if (aimExtra != null)
                        {
                            Packet.Builder.LegacyModule(gameSession,
                                "0|A|CPU|A|" + aimExtra.Amount + "|" + Convert.ToInt32(aimExtra.Active), true);
                        }

                        break;
                    case Types.JCPU:
                        var jumpCpu = baseController.Player.Extras.Values.FirstOrDefault(x => x is JumpCpu);
                        if (jumpCpu != null)
                        {
                            Packet.Builder.LegacyModule(gameSession, "0|A|CPU|J|0|0|" + jumpCpu.Amount, true);
                        }

                        break;
                    case Types.DRO_CPU:
                        var droCpu = baseController.Player.Extras.Values.FirstOrDefault(x => x is DROCpu);
                        if (droCpu != null)
                        {
                            Packet.Builder.LegacyModule(gameSession, "0|A|CPU|D|" + droCpu.Amount, true);
                        }

                        break;
                }
            }
        }

        public void Activate(Types type)
        {
            lock (ThreadLock)
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

                        break;
                    case Types.AUTO_ROCKLAUNCHER:
                        var arlExtra = baseController.Player.Extras.FirstOrDefault(x => x.Value is AutoRocketLauncher);
                        if (arlExtra.Value != null && arlExtra.Value.Amount > 0)
                        {
                            if (arlExtra.Value.Active)
                            {
                                Active.Remove(Types.AUTO_ROCKLAUNCHER);
                                arlExtra.Value.Active = false;
                            }
                            else
                            {
                                Active.Add(Types.AUTO_ROCKLAUNCHER);
                                arlExtra.Value.Active = true;
                            }

                            Update(Types.AUTO_ROCKLAUNCHER);
                        }

                        break;
                }
            }
        }

        void Robot()
        {
            lock (ThreadLock)
            {
                if (baseController.Character.Moving || !baseController.Repairing ||
                    baseController.Character.LastCombatTime.AddSeconds(3) > DateTime.Now)
                {
                    baseController.Repairing = false;
                    return;
                }

                var player = (Player) baseController.Character;
                if (player.CurrentHealth == player.MaxHealth)
                {
                    baseController.Repairing = false;
                    return;
                }

                var robot = player.Extras.Values.FirstOrDefault(x => x is Robot) as Robot;
                if (robot == null)
                {
                    return;
                }

                var repAmount = (player.MaxHealth / 100) * robot.GetLevel();

                baseController.Heal.Execute(repAmount);
            }
        }

        void Cloak()
        {
            lock (ThreadLock)
            {
                var cloakExtra = baseController.Player.Extras.FirstOrDefault(x => x.Value is Cloak);
                if (cloakExtra.Value != null && cloakExtra.Value.Amount > 0)
                {
                    baseController.Character.Invisible = true;
                    if (baseController.Player.Pet != null && baseController.Player.Pet.Controller.Active)
                    {
                        baseController.Player.Pet.Invisible = true;
                        baseController.Player.Pet.Controller.Effects.UpdateCharacterVisibility();
                    }

                    baseController.Player.Extras.FirstOrDefault(x => x.Value is Cloak).Value.Amount -= 1;
                    Update(Types.CLOAK);
                    baseController.Effects.UpdateCharacterVisibility();
                }
            }
        }

        void DroneRepair()
        {

        }

        public int SelectedMapId;

        public bool JumpSequenceActive;
        public DateTime JumpSequenceEnd;

        void AdvancedJCPU()
        {
            lock (ThreadLock)
            {
                if (JumpSequenceActive && SelectedMapId != 0 && JumpSequenceEnd <= DateTime.Now &&
                    baseController.Player.LastCombatTime.AddSeconds(3) < DateTime.Now)
                {
                    JumpSequenceActive = false;
                    var map = World.StorageManager.Spacemaps[SelectedMapId];
                    Packet.Builder.LegacyModule(baseController.Player.GetGameSession(), "0|A|JCPU|S|-1|-1");
                    baseController.Player.MoveToMap(map, Vector.Random(map), 0);
                }
                else if (baseController.Player.LastCombatTime.AddSeconds(3) > DateTime.Now || SelectedMapId == 0 ||
                         !JumpSequenceActive)
                {
                    Packet.Builder.LegacyModule(baseController.Player.GetGameSession(), "0|A|JCPU|S|-1|-1");
                    JumpSequenceActive = false;
                }
            }
        }

        public void Clear()
        {
            JumpSequenceActive = false;
            baseController.Repairing = false;
            Active.Clear();
        }
    }
}
