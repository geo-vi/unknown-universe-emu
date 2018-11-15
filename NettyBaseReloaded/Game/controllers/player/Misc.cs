using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.controllers.player.structs;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players.settings.slotbars;
using NettyBaseReloaded.Networking;

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

            if (baseController.Attack.Attacking || baseController.Attack.GetActiveAttackers()?.Count > 0)
            {
                AbortLogout();
                return;
            }

            if (gameSession.Player.Information.Premium.Active && LogoutStartTime.AddSeconds(5) < DateTime.Now
            || LogoutStartTime.AddSeconds(20) < DateTime.Now)
            {
                Packet.Builder.LogoutCommand(gameSession);
                gameSession.Disconnect(GameSession.DisconnectionType.NORMAL);
                LoggingOut = false;
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
                    baseController.Damage?.Radiation(radiationDamage);
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

        private PlayerBeacon _beacon;

        public void BeaconSync()
        {
            var gameSession = baseController.Player.GetGameSession();
            if (!baseController.Active || gameSession == null) return;
            if (_beacon.InEquipmentArea != baseController.Player.State.InEquipmentArea)
            {
                Packet.Builder.EquipReadyCommand(gameSession, baseController.Player.State.InEquipmentArea);
                _beacon.InEquipmentArea = baseController.Player.State.InEquipmentArea;
            }
            if (_beacon.InDemiZone != baseController.Player.State.InDemiZone|| _beacon.InPortalArea != baseController.Player.State.InPortalArea || _beacon.InRadiationArea != baseController.Player.State.InRadiationArea || _beacon.InTradeArea != baseController.Player.State.InTradeArea)
            {
                Packet.Builder.BeaconCommand(gameSession);
                _beacon.InDemiZone = baseController.Player.State.InDemiZone;
                _beacon.InPortalArea = baseController.Player.State.InPortalArea;
                _beacon.InRadiationArea = baseController.Player.State.InRadiationArea;
                _beacon.InTradeArea = baseController.Player.State.InTradeArea;
            }

            if (_beacon.Repairing != baseController.Repairing)
            {
                Packet.Builder.LegacyModule(gameSession, "0|A" +
                                            "|RS|" +
                                            "S|" + Convert.ToInt32(baseController.Repairing), true);
                _beacon.Repairing = baseController.Repairing;
            }
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
            var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);

            if (baseController.Character.Cooldowns.Any(x => x is ConfigCooldown))
            {
                Packet.Builder.LegacyModule(gameSession
                , "0|A|STM|config_change_failed_time");
                return;
            }

            baseController.Character.Cooldowns.Add(new ConfigCooldown());

            targetConfigId = baseController.Player.CurrentConfig == 2 ? 1 : 2;

            baseController.Player.CurrentConfig = targetConfigId;
            baseController.Player.Updaters.Update();
            Packet.Builder.LegacyModule(gameSession
                , "0|A|CC|" + baseController.Player.CurrentConfig);

            baseController.Player.UpdateExtras();

            foreach (var playerEntity in baseController.Player.Spacemap.Entities.Values.Where(x => x is Player))
            {
                var player = playerEntity as Player;
                var entitySession = player.GetGameSession();
                if (gameSession != null)
                    Packet.Builder.DronesCommand(entitySession, baseController.Player);
            }

            if (baseController.Player.Moving)
                MovementController.Move(baseController.Player, baseController.Player.Destination);
        }

        private class jClass
        {
            private DateTime _jumpEndTime;

            private readonly PlayerController _baseController;

            private int TargetVirtualWorldId { get; set; }
            private Spacemap TargetMap { get; set; }
            private Vector TargetPosition { get; set; }

            public jClass(PlayerController baseController)
            {
                this._baseController = baseController;
            }

            /// <summary>
            /// Starting the Jump sequence
            /// </summary>
            /// <param name="targetVW"></param>
            /// <param name="targetMapId"></param>
            /// <param name="targetPos"></param>
            /// <param name="portalId"></param>
            public void Initiate(int targetVW, int targetMapId, Vector targetPos, int portalId = -1)
            {
                if (_baseController.Character.EntityState == EntityStates.DEAD || _baseController.StopController) return;

                TargetVirtualWorldId = targetVW;
                TargetMap = World.StorageManager.Spacemaps[targetMapId];
                TargetPosition = targetPos;

                var gameSession = World.StorageManager.GetGameSession(_baseController.Player.Id);
                if (TargetMap.Level > _baseController.Player.Information.Level.Id)
                {
                    Packet.Builder.LegacyModule(gameSession, $"0|k|{TargetMap.Level}");
                    Cancel();
                    return;
                }
                if (portalId != -1)
                {
                    Packet.Builder.ActivatePortalCommand(gameSession, _baseController.Player.Spacemap.Objects[portalId] as Jumpgate);
                }
                _jumpEndTime = DateTime.Now.AddSeconds(3);
                _baseController.Jumping = true;
            }

            public void Checker()
            {
                if (_baseController.Jumping) Refresh();
            }

            void Refresh()
            {
                if (_baseController.Character.EntityState == EntityStates.DEAD || _baseController.StopController)
                {
                    Cancel();
                    return;
                }

                if (_baseController.Attack.GetActiveAttackers().Any(x => x.InRange(_baseController.Character, x.AttackRange)) && _baseController.Player.Spacemap.Pvp)
                {
                    Cancel();
                    return;
                }

                if (DateTime.Now > _jumpEndTime)
                {
                    _baseController.Miscs.ForceChangeMap(TargetMap, TargetPosition, TargetVirtualWorldId);
                    Reset();
                }
            }

            void Cancel()
            {
                Reset();
            }

            void Reset()
            {
                _baseController.Jumping = false;
                _jumpEndTime = DateTime.Now;
                TargetMap = null;
                TargetPosition = null;
            }
        }

        public void Jump(int targetMapId, Vector targetPos, int portalId = -1, int targetVW = 0)
        {
            JClass.Initiate(targetVW, targetMapId, targetPos, portalId);
        }

        private void ForceChangeMap(Spacemap targetMap, Vector targetPosition, int vw = 0)
        {
            baseController.Player.Pet?.Invalidate();
            if (baseController.Player.Spacemap == targetMap) return;
            var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
            Packet.Builder.MapChangeCommand(gameSession);
            gameSession.Relog(targetMap, targetPosition);
            gameSession.Player.VirtualWorldId = vw;
        }
    }
}
