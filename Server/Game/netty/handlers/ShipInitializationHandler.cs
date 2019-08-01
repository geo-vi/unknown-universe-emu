using System;
using System.Threading.Tasks;
using Server.Configurations;
using Server.Game.controllers.players;
using Server.Game.managers;
using Server.Game.objects;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Networking;
using Server.Networking.clients;
using Server.Utils;

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
                              ", IP: " + client.IpEndPoint.Address + "]");

            if (WhitelistManager.Instance.IsInWhitelist(client.IpEndPoint.Address))
            {
                Out.QuickLog("True, whitelisted. Continuing initialisation");
            }
            else
            {
                Out.QuickLog("Not in whitelist, probably reconnecting...");
            }
            
            Client = client;
            
            UserId = userId;
            client.UserId = userId;
            
            SessionId = sessionId;
            NewClient = newClient;
        }

        public void Execute()
        {
            var sessionBuilt = SessionBuilder();
            if (sessionBuilt == null)
            {
                Task.Run(Client.Disconnect);
                return;
            }
            
            var loginController = new LoginController(sessionBuilt);
            loginController.Execute();
        }

        private GameSession SessionBuilder()
        {
            if (TryHijackSession(out var session))
            {
                Out.WriteLog("Successfully hijacked a session", LogKeys.PLAYER_LOG, UserId);
            }
            else
            {
                var playerAccount = GameDatabaseManager.Instance.CreatePlayer(UserId, NewClient);
                if (playerAccount == null)
                {
                    return null;
                }
                session = new GameSession(Client)
                {
                    Player =  playerAccount
                };
            }

            return session;
        }

        private bool TryHijackSession(out GameSession session)
        {
            session = GameStorageManager.Instance.FindSession(UserId);
            if (session == null)
            {
                return false;
            }
            else
            {
                if (CharacterStateManager.Instance.IsInState(session.Player, CharacterStates.NO_CLIENT_CONNECTED))
                {
                    session.GameClient = Client;
                    CharacterStateManager.Instance.RequestStateChange(session.Player, CharacterStates.LOGIN, out _);
                }
                else
                {
                    //step 1 :: gtfo
                    Task.Run(session.GameClient.Disconnect);
                    
                    CharacterStateManager.Instance.RequestStateChange(session.Player, CharacterStates.NO_CLIENT_CONNECTED, out _);

                    //step 2 :: takeover
                    session.GameClient = Client;
                    
                    CharacterStateManager.Instance.RequestStateChange(session.Player, CharacterStates.LOGIN, out _);
                }
                return true;
            }
        }
    }
}