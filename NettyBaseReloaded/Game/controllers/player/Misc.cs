using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players.settings.slotbars;

namespace NettyBaseReloaded.Game.controllers.player
{
    class Misc : IChecker
    {
        // TODO: Make every function return 0 / 1 & stuff to be handled by the response.

        private PlayerController baseController;

        private jClass JClass { get; set; }

        public Misc(PlayerController controller)
        {
            baseController = controller;
            JClass = new jClass(controller);
        }

        public bool LoggingOut = false;
        private DateTime LogoutStartTime = new DateTime();

        public void Logout(bool start = false)
        {
            if (start)
            {
                LoggingOut = true;
                LogoutStartTime = DateTime.Now;
                return;
            }

            if (!LoggingOut) return;

            var gameSession = baseController.Player.GetGameSession();

            if (gameSession.Player.Information.Premium && LogoutStartTime.AddSeconds(5) < DateTime.Now
            || LogoutStartTime.AddSeconds(20) < DateTime.Now)
            {
                gameSession.Disconnect(GameSession.DisconnectionType.NORMAL);
                Packet.Builder.LegacyModule(gameSession, "0|l");
            }

        }

        public void AbortLogout()
        {
            var gameSession = baseController.Player.GetGameSession();
            LoggingOut = false;
            Packet.Builder.LegacyModule(gameSession, "0|t");
        }

        private DateTime lastDamagedTime = new DateTime();

        public void RadiationZone()
        {
            if (baseController.Player.State.InRadiationArea)
            {
                if(lastDamagedTime.AddSeconds(1) < DateTime.Now)
                {
                    var radiationDamage = (DateTime.Now - baseController.Player.State.RadiationEntryTime).Seconds * (baseController.Player.MaxHealth / 25);
                    baseController.Damage?.Radiation(baseController.Character, radiationDamage);
                    lastDamagedTime = DateTime.Now;
                }
            }
        }

        public void Check()
        {
            JClass.Checker();
            Logout();
            RadiationZone();
            BeaconSync();
        }

        private DateTime LastBeaconSent = new DateTime();
        public void BeaconSync()
        {
            if (LastBeaconSent.AddSeconds(1) > DateTime.Now || !baseController.Active) return;
            var gameSession = baseController.Player?.GetGameSession();
            if (gameSession != null)
                Packet.Builder.BeaconCommand(gameSession);
            LastBeaconSent = DateTime.Now;
        }

        /// <summary>
        /// Executes the item function depending of the selected one
        /// </summary>
        public void UseItem(string itemId)
        {
            var player = (Player)baseController.Player;

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
            if (baseController.Character.Cooldowns.Exists(x => x is ConfigCooldown)) return;

            baseController.Character.Cooldowns.Add(new ConfigCooldown());

            targetConfigId = baseController.Player.CurrentConfig == 2 ? 1 : 2;

            baseController.Player.CurrentConfig = targetConfigId;

            baseController.Player.Update();

            var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
            Packet.Builder.LegacyModule(gameSession
                , "0|A|CC|" + baseController.Player.CurrentConfig);

            baseController.Player.UpdateExtras();
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
                if (TargetMap.Level > baseController.Player.Information.Level.Id)
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

                if (baseController.Attack.Attacking && baseController.Player.Spacemap.Pvp)
                {
                    Cancel();
                    return;
                }

                if (DateTime.Now > JumpEndTime)
                {
                    baseController.Miscs.ForceChangeMap(TargetMap, TargetPosition);
                    Reset();
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

        public void ForceChangeMap(Spacemap targetMap, Vector targetPosition)
        {
            if (baseController.Player.Spacemap == targetMap) return;
            var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
            Packet.Builder.MapChangeCommand(gameSession);
            baseController.Destruction.Deselect(baseController.Player);
            gameSession.Relog(targetMap, targetPosition);
            //baseController.Player.Position = targetPosition;
            //baseController.Player.Spacemap = targetMap;
            //baseController.Player.Save();
        }

        public void ChangeDroneFormation(DroneFormation targetFormation)
        {
            //if (
            //    !baseController.CooldownStorage.Finished(
            //        objects.world.storages.playerStorages.CooldownStorage.DRONE_FORMATION_COOLDOWN)) return;

            //var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
            //baseController.Player.Formation = targetFormation;
            //gameSession.Client.Send(DroneFormationChangeCommand.write(baseController.Player.Id, (int)targetFormation));
            //baseController.CooldownStorage.Setup(gameSession, objects.world.storages.playerStorages.CooldownStorage.DRONE_FORMATION_COOLDOWN);
            //baseController.Player.Update();
        }

        private DateTime LastReloadedTime = new DateTime(2016, 1, 1, 0, 0, 0);
        public void ReloadConfigs()
        {
            
        }
    }
}
