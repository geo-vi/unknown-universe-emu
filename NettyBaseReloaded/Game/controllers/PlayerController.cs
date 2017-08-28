using System;
using System.Linq;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Game.objects.world.players.settings.new_client_slotbars;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers
{
    class PlayerController : AbstractCharacterController
    {
        #region Classes
        public CPU CPUs { get; set; }

        public Range Ranges { get; set; }

        public Misc Miscs { get; set; }
        #endregion

        public Player Player { get; }

        public bool Repairing { get; set; }

        public bool Jumping { get; set; }

        public PlayerController(Character character) : base(character)
        {
            Player = Character as Player;
            Repairing = false;
            Jumping = false;
        }

        private void AddClasses()
        {
            CPUs = new CPU(this);
            CheckedClasses.Add(CPUs);
            Ranges = new Range(this);
            CheckedClasses.Add(Ranges);
            Miscs = new Misc(this);
            CheckedClasses.Add(Miscs);
        }

        public void Start()
        {
            AddClasses();    
        }

        public new void Tick()
        {
            Checkers();

            foreach (var _class in CheckedClasses)
            {
                _class.Check();
            }
        }

        public class CPU : IChecker
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
                foreach (var type in Enum.GetValues(typeof(Types))) Initiate((Types) type);
            }

            DateTime LastTimeCheckedCpus = new DateTime(2017, 1, 18, 0, 0, 0);
            
            public void Check()
            {
                if (LastTimeCheckedCpus.AddSeconds(1) > DateTime.Now) return;
                LastTimeCheckedCpus = DateTime.Now;

                if (baseController.Repairing) Robot();
            }

            public void Initiate(Types type)
            {
                var client = World.StorageManager.GetGameSession(baseController.Character.Id).Client;
                switch (type)
                {
                    case Types.CLOAK:
                        //client.Send(Builder.LegacyModule("0|A|CPU|C|" + baseController.Player.GetCpuUsesLeft(type)).Bytes);
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
                if (baseController.Character.Moving || !baseController.Repairing || baseController.Character.LastCombatTime.AddSeconds(3) > DateTime.Now) return;

                var player = (Player)baseController.Character;
                if (player.CurrentHealth == player.MaxHealth)
                {
                    baseController.Repairing = false;
                    return;
                }

                var repAmount = (player.MaxHealth / 100) * player.GetRobotLevel();

                baseController.Heal(repAmount);
            }

            void Cloak()
            {
                
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

        public class Range : IChecker
        {
            private PlayerController baseController;

            public Range(PlayerController controller)
            {
                baseController = controller;
            }

            public void Check()
            {
                LookupRangeZones();
            }

            private DateTime LastTimeCheckedZones = new DateTime();
            public void LookupRangeZones()
            {
                if (LastTimeCheckedZones.AddMilliseconds(250) > DateTime.Now) return;

                if (baseController.Player.RangeZones.Values.Count(x => x is DemiZone) > 0)
                {
                    if (!baseController.Player.InDemiZone && !baseController.Attacking)
                    {
                        baseController.Player.InDemiZone = true;
                        UpdatePlayer();
                    }
                }
                else
                {
                    if (baseController.Player.InDemiZone)
                    {
                        baseController.Player.InDemiZone = false;
                        UpdatePlayer();
                    }
                }

                LastTimeCheckedZones = DateTime.Now;
            }

            private DateTime LastTimeCheckedObjects = new DateTime();
            public void LookupRangeObjects()
            {
                //TODO
                LastTimeCheckedObjects = DateTime.Now;
            }

            public void UpdatePlayer()
            {
                var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
                Packet.Builder.BeaconCommand(gameSession);
            }
        }

        public class Misc : IChecker
        {
            // TODO: Make every function return 0 / 1 & stuff to be handled by the response.

            private PlayerController baseController;

            private jClass JClass { get; set; }

            public Misc(PlayerController controller)
            {
                baseController = controller;
                JClass = new jClass(controller);
            }

            public void Check()
            {
                JClass.Checker();
            }

            /// <summary>
            /// Executes the item function depending of the selected one
            /// </summary>
            public void UseItem(string itemId)
            {
                var player = (Player) baseController.Player;

                //Console.WriteLine(itemId);
                if (player.Settings.Slotbar._items.ContainsKey(itemId))
                {
                    var item = player.Settings.Slotbar._items[itemId];

                    if (item.Visible && (item.Activable || item is RocketItem))
                    {
                        //This is the magic function :D
                        item.Execute(player);
                    }
                }
            }

            public void ChangeConfig(int targetConfigId = 0)
            {
//                if (!baseController.CooldownStorage.Finished(objects.world.storages.playerStorages.CooldownStorage.CONFIG_COOLDOWN)) return;
//                baseController.CooldownStorage.ConfigCooldownEnd = DateTime.Now.AddSeconds(3);
//
//                targetConfigId = baseController.Player.CurrentConfig == 2 ? 1 : 2;
//
//                baseController.Player.CurrentConfig = targetConfigId;
//
//                baseController.Player.Update();

                //World.StorageManager.GetGameSession(baseController.Player.Id).Client?.Send(Builder.LegacyModule("0|A|CC|" + baseController.Player.CurrentConfig).Bytes);
            }

            private class jClass
            {
                private DateTime JumpEndTime = new DateTime(2017, 1, 24, 0, 0, 0);

                private PlayerController baseController;

                private Spacemap TargetMap { get; set; }
                private Vector TargetPosition { get; set; }

                public jClass(PlayerController baseController)
                {
                    this.baseController = baseController;
                }

                public void Initiate(int targetMapId, Vector targetPos, int portalId = -1)
                {
                    if (baseController.Dead || baseController.StopController) return;

                    TargetMap = World.StorageManager.Spacemaps[targetMapId];
                    TargetPosition = targetPos;

                    var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
                    if (TargetMap.Level > baseController.Player.Level.Id)
                    {
                        Packet.Builder.LegacyModule(gameSession, $"0|k|{TargetMap.Level}");
                        Cancel();
                        return;
                    }
                    if (portalId != -1)
                    {
                        Packet.Builder.ActivatePortalCommand(gameSession, baseController.Player.Spacemap.Objects[portalId] as Jumpgate);
                    }
                    JumpEndTime = DateTime.Now.AddSeconds(3);
                    baseController.Jumping = true;
                }

                public void Checker()
                {
                    if (baseController.Jumping) Refresh();
                }

                void Refresh()
                {
                    if (baseController.Dead || baseController.StopController)
                    {
                        Cancel();
                        return;
                    }

                    if ((baseController.Attacked || baseController.Attacking) && baseController.Player.Spacemap.Pvp)
                    {
                        Cancel();
                        return;
                    }

                    if (DateTime.Now > JumpEndTime)
                    {
                        baseController.Player.Spacemap = TargetMap;
                        baseController.Player.Position = TargetPosition;
                        baseController.Remove(baseController.Player);
                        Reset();
                        Console.WriteLine("Faking test");
                        World.StorageManager.GetGameSession(baseController.Player.Id).Disconnect();
                    }
                }

                void Cancel()
                {
                    Reset();
                }

                void Reset()
                {
                    baseController.Jumping = false;
                    JumpEndTime = DateTime.Now;
                    TargetMap = null;
                    TargetPosition = null;
                }
            }

            public void Jump(int targetMapId, Vector targetPos, int portalId = -1)
            {
                JClass.Initiate(targetMapId, targetPos, portalId);
            }

            public void ChangeDroneFormation(DroneFormation targetFormation)
            {
                //if (
                //    !baseController.CooldownStorage.Finished(
                //        objects.world.storages.playerStorages.CooldownStorage.DRONE_FORMATION_COOLDOWN)) return;

                //var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
                //baseController.Player.Formation = targetFormation;
                //gameSession.Client.Send(DroneFormationChangeCommand.write(baseController.Player.Id, (int)targetFormation));
                //baseController.CooldownStorage.Start(gameSession, objects.world.storages.playerStorages.CooldownStorage.DRONE_FORMATION_COOLDOWN);
                //baseController.Player.Update();
            }

            private DateTime LastReloadedTime = new DateTime(2016, 1, 1, 0, 0, 0);
            public void ReloadConfigs()
            {
                try
                {
                    var dateTime = DateTime.Now;
                    if (LastReloadedTime <= dateTime.AddSeconds(3))
                    {
                        var configs = World.DatabaseManager.LoadConfig(baseController.Player);
                        var hangar = World.DatabaseManager.LoadHangar(baseController.Player);
                        var drones = World.DatabaseManager.LoadDrones(baseController.Player);

                        baseController.Player.Hangar = hangar;
                        baseController.Player.Hangar.Configurations = configs;
                        baseController.Player.Hangar.Drones = drones;

                        LastReloadedTime = DateTime.Now;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: Can't reload the confis.");
                }
            }

        }
    }
}
