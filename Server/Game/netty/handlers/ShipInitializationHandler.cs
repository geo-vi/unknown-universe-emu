using System;
using Server.Configurations;
using Server.Game.controllers.player;
using Server.Game.objects;
using Server.Networking;

namespace Server.Game.netty.handlers
{
    class ShipInitializationHandler
    {
        private GameClient Client { get; }
        private int UserId { get; }

        private string SessionId { get; }
        
        private bool NewClient { get; }
        
        public ShipInitializationHandler(GameClient client, int userId, string sessionId, bool newClient = false)
        {
            if (ServerConfiguration.PRINTING_CONNECTIONS)
                Console.WriteLine("Connection Received, [USERID: " + userId + ", SESSIONID: " + sessionId +
                              "]");

            Client = client;
            
            UserId = userId;
            client.UserId = userId;
            
            SessionId = sessionId;
            NewClient = newClient;
        }

        public void Execute()
        {
            var sessionBuilt = SessionBuilder();
            var loginController = new LoginController(sessionBuilt);
        }

        private GameSession SessionBuilder()
        {
            return null;
//            var account = GameDatabaseManager.GetAccount(UserId);
//            account.UsingNewClient = NewClient;
//
//            if (SessionId != account.SessionId)
//            {
//                Console.WriteLine("Breach attempt by " + Client.IpEndPoint);
//                return null; // Fucked up session
//            }
//            if (GameStorageManager.GameSessions.ContainsKey(UserId))
//            {
//                var gameSession = GameStorageManager.GameSessions[UserId];
//                gameSession.Kick();
////                account = gameSession.Player;
////                account.SessionId = sessionId;
////                account.UsingNewClient = usingNewClient;
//            }
//            return new GameSession(account) { GameClient = Client };
       }
    }
}