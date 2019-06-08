using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects
{
    class GameSession : ITick
    {
        private int TickId { get; set; }

        public Player Player { get; set; }

        public GameClient Client { get; set; }

        public DateTime LastActivityTime { get; set; }

        public DateTime SessionStartTime;

        public bool Active
        {
            get
            {
                if (Client != null && (Player.LastCombatTime.AddMinutes(10) > DateTime.Now ||
                                       Player.MovementStartTime.AddMinutes(10) > DateTime.Now))
                {
                    LastActivityTime = DateTime.Now;
                    return true;
                }

                return false;
            }
        }

        public GameSession(Player player)
        {
            Player = player;
            LastActivityTime = DateTime.Now;
            var tickId = -1;
            Global.TickManager.Add(this, out tickId);
            TickId = tickId;
            World.StorageManager.GameSessions.TryAdd(Player.Id, this);
            SessionStartTime = DateTime.Now;
        }

        public int GetId()
        {
            return TickId;
        }

        public void Tick()
        {
            if (LastActivityTime.AddSeconds(25) < DateTime.Now &&
                !Active /*Player.LastCombatTime.AddMinutes(3) > LastActivityTime && Player.MovementStartTime.AddMinutes(3) > LastActivityTime*/
            )
            {
                Kick();
            }
        }

        public void Start()
        {
            LoginController.Initiate(this);
            LastActivityTime = DateTime.Now;
        }

        public static Dictionary<int, GameSession> GetRangeSessions(IAttackable attackable)
        {
            Dictionary<int, GameSession> playerSessions = new Dictionary<int, GameSession>();
            foreach (var player in attackable.Spacemap.Entities.Where(x => x.Value is Player)
                .ToDictionary(x => x.Key, y => y.Value as Player))
            {
                var session = player.Value.GetGameSession();
                playerSessions.Add(player.Key, session);
            }

            return playerSessions;
        }

        //TODO:
        public void Relog(Spacemap spacemap = null, Vector pos = null)
        {
            throw new NotImplementedException();
            Disconnect();
            if (spacemap != null)
                Player.Spacemap = spacemap;
            if (pos != null)
                Player.ChangePosition(pos);

            Player.Save();
            //Store();
        }

        public void Reset(GameClient client)
        {
            Client.Disconnect();

            Player.Invalidate();
            Client = client;
        }

        public void Inactivity()
        {
            //todo
        }

        //TODO:
        public void Store()
        {
        }

        public void Kick()
        {
            Player.Invalidate();
            Disconnect();
        }

        /// <summary>
        /// No preparations, just close the socket
        /// </summary>
        public void Disconnect()
        {
            if (Player == null) return;
            EndSession();
            Client.Disconnect();
        }

        public void EndSession()
        {
            GameSession removedSession;
            Global.TickManager.Remove(this);

            World.StorageManager.GameSessions.TryRemove(Player.Id, out removedSession);

            if (Player.Controller != null)
            {
                Global.TickManager.Remove(Player.Controller);
                Player.Controller = null;
            }

            Global.TickManager.Remove(Player);
            Player = null;
        }

        public void KillSession()
        {
            Disconnect();
        }
    }
}
