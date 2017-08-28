using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipInitalizationHandler
    {
        private GameSession GameSession { get; set; }
        private Player Player { get; set; }
        
        public ShipInitalizationHandler(GameClient client, int userId, string sessionId, bool newClient = false)
        {
            Logger.Logger.WritingManager.Write("Connection Received, [USERID: " + userId + ", SESSIONID: " + sessionId + "]");

            client.UserId = userId;
            if (!ValidateSession(userId, sessionId)) return;

            if (!World.StorageManager.Accounts.ContainsKey(userId))
                World.DatabaseManager.SearchForAccount(client.UserId);

            Player = World.StorageManager.Accounts[userId];
            Player.UsingNewClient = newClient;

            if (Player != null) GameSession = CreateSession(client, Player);

            execute();
        }

        private bool ValidateSession(int userId, string sessionId)
        {               
            return true;
        }

        private GameSession CreateSession(GameClient client, Player player)
        {
            return new GameSession(player)
            {
                Client = client,
                LastActiveTime = DateTime.Now
            };
        }

        public void execute()
        {
            if (GameSession == null) return;

            if (!World.StorageManager.GameSessions.ContainsKey(Player.Id))
                World.StorageManager.GameSessions.Add(Player.Id, GameSession);
            else World.StorageManager.GameSessions[Player.Id] = GameSession;

            LoginController.Login(GameSession);
        }
    }
}
