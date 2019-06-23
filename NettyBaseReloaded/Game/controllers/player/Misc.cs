using System;
using System.Linq;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.controllers.player.structs;
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

        private object ThreadLock = new object();

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
            lock (ThreadLock)
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

                if ((gameSession.Player.Information.Premium.Active ||
                     gameSession.Player.RankId == Rank.ADMINISTRATOR) && LogoutStartTime.AddSeconds(5) < DateTime.Now
                    || LogoutStartTime.AddSeconds(20) < DateTime.Now)
                {
                    Packet.Builder.LogoutCommand(gameSession);
                    gameSession.Disconnect();
                    LoggingOut = false;
                }
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
            lock (ThreadLock)
            {
                if (baseController.Player.State.InRadiationArea)
                {
                    if (lastDamagedTime.AddSeconds(1) < DateTime.Now)
                    {
                        var radiationDamage = (DateTime.Now - baseController.Player.State.RadiationEntryTime).Seconds *
                                              (baseController.Player.MaxHealth / 25);
                        baseController.Damage?.Radiation(radiationDamage);
                        lastDamagedTime = DateTime.Now;
                    }
                }
            }
        }

        public void Check()
        {
            JClass.Checker();
            Logout();
            RadiationZone();
        }

        /// <summary>
        /// Executes the item function depending of the selected one
        /// </summary>
        public void UseItem(string itemId)
        {
            lock (ThreadLock)
            {
                var player = (Player) baseController.Player;

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
        }

        public void ChangeConfig(int targetConfigId = 0)
        {
            lock (ThreadLock)
            {
                var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);

                if (baseController.Character.Cooldowns.CooldownDictionary.Any(c => c.Value is ConfigCooldown))
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

                baseController.Player.UpdateConfig();

                foreach (var rangeSession in GameSession.GetRangeSessions(baseController.Player))
                {
                    if (rangeSession.Value != null)
                        Packet.Builder.DronesCommand(rangeSession.Value, baseController.Player);
                }

                if (baseController.Player.Moving)
                    MovementController.Move(baseController.Player, baseController.Player.Destination);

                baseController.Player.Pet?.RefreshConfig();
                baseController.Player.Pet?.Controller.SendResetGear();
            }
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
                if (_baseController.Character.EntityState == EntityStates.DEAD || _baseController.StopController || _baseController.Jumping) return;

                TargetVirtualWorldId = targetVW;
                TargetMap = World.StorageManager.Spacemaps[targetMapId];
                TargetPosition = targetPos;

                var gameSession = World.StorageManager.GetGameSession(_baseController.Player.Id);
                if (TargetMap.Level > _baseController.Player.Information.Level.Id)
                {
                    //World.DatabaseManager.AddPlayerLog(_baseController.Player, PlayerLogTypes.SYSTEM, "Canceling Jump due to not enough level.");
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
                    //World.DatabaseManager.AddPlayerLog(_baseController.Player, PlayerLogTypes.SYSTEM, "Canceling Jump due to Entity death or stopped controller.");
                    Cancel();
                    return;
                }

                if (_baseController.Attack.GetActiveAttackers().Any(x => x.InRange(_baseController.Character, x.AttackRange)) && _baseController.Player.Spacemap.Pvp)
                {
                    //World.DatabaseManager.AddPlayerLog(_baseController.Player, PlayerLogTypes.SYSTEM, "Canceling Jump due to an active attack in PVP map.");
                    Cancel();
                    return;
                }

                if (DateTime.Now > _jumpEndTime && TargetMap != null && TargetPosition != null)
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
            lock (ThreadLock)
            {
                JClass.Initiate(targetVW, targetMapId, targetPos, portalId);
                //World.DatabaseManager.AddPlayerLog(baseController.Player, PlayerLogTypes.SYSTEM,
                //    "Started jump process from portal ID " + portalId + " to target map: " + targetMapId + " [pos: " +
                //    targetPos + ",vw: " + targetVW + "]");
            }
        }

        private readonly object JumpLock = new object();
        private void ForceChangeMap(Spacemap targetMap, Vector targetPosition, int vw = 0)
        {
            lock (JumpLock)
            {
                baseController.Player.State.Jumping = true;
                if (baseController.Player.Spacemap == targetMap) return;
                baseController.Player.MoveToMap(targetMap, targetPosition, vw);
                baseController.Player.State.Jumping = false;
                //World.DatabaseManager.AddPlayerLog(baseController.Player, PlayerLogTypes.SYSTEM, "Jumped to " + targetMap.Id + " AT " + targetPosition + " IN VW " + vw);
            }
        }
    }
}
