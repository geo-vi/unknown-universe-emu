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

        public GameSession(Player player)
        {
            Player = player;
            var tickId = -1;
            Global.TickManager.Add(this, out tickId);
            TickId = tickId;
            World.StorageManager.GameSessions.Add(Player.Id, this);
        }

        public int GetId()
        {
            return TickId;
        }

        public void Tick()
        {
        }

        public static Dictionary<int, GameSession> GetRangeSessions(IAttackable attackable)
        {
            Dictionary<int, GameSession> playerSessions = new Dictionary<int, GameSession>();
            foreach (var player in attackable.Spacemap.Entities.Where(x => x.Value is Player).ToDictionary(x => x.Key, y => y.Value as Player))
            {
                var session = player.Value.GetGameSession();
                playerSessions.Add(player.Key, session);
            }
            return playerSessions;
        }

        public void Relog(Spacemap spacemap = null, Vector pos = null)
        {
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
            if (Client.Connected)
            {
                Client.Disconnect();
            }
            Player.Invalidate();
            Client = client;
        }

        public void Inactivity()
        {
            //todo
        }

        public void Store()
        {
            World.StorageManager.PlayerStorage.Add(Player.Id, Player);
            if (Player.Storage.RemoveTask.Status == TaskStatus.Running) Player.Storage.RemoveTask.Dispose();
            Player.Storage.RemoveTask = Task.Delay(90000).ContinueWith((t) => World.StorageManager.PlayerStorage.Remove(Player.Id));
        }

        public void Kick()
        {
            Packet.Builder.LegacyModule(this, "ERR|2");
            Disconnect();
        }

        /// <summary>
        /// No preparations, just close the socket
        /// </summary>
        public void Disconnect()
        {
            Player.Invalidate();
            Client.Disconnect();
            EndSession();
        }

        public void EndSession()
        {
            Global.TickManager.Remove(this);
            if (World.StorageManager.GameSessions.ContainsKey(Player.Id))
                World.StorageManager.GameSessions.Remove(Player.Id);
        }
    }
}
