using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Networking;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipInitalizationHandler
    {
        public ShipInitalizationHandler(GameClient client, int userId, string sessionId, bool newClient = false)
        {
            if (Properties.Game.PRINTING_CONNECTIONS)
                Console.WriteLine("Connection Received, [USERID: " + userId + ", SESSIONID: " + sessionId +
                              "]");

            client.UserId = userId;
            execute(SessionBuilder(client, userId, sessionId, newClient));
        }

        private void execute(GameSession session)
        {
            if (session == null) return;
            session.Start();
        }

        public GameSession SessionBuilder(GameClient client, int userId, string sessionId, bool usingNewClient)
        {
            var account = World.DatabaseManager.GetAccount(userId);
            account.UsingNewClient = usingNewClient;

            if (sessionId != account.SessionId)
            {
                Console.WriteLine("Breach attempt by " + client.IpEndPoint);
                return null; // Fucked up session
            }
            if (World.StorageManager.GameSessions.ContainsKey(userId))
            {
                var gameSession = World.StorageManager.GameSessions[userId];
                gameSession.Kick();
//                account = gameSession.Player;
//                account.SessionId = sessionId;
//                account.UsingNewClient = usingNewClient;
            }
            return new GameSession(account) { Client = client };
        }
    }
}