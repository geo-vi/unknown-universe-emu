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

namespace NettyBaseReloaded.Game.netty.handlers
{
    class ShipInitalizationHandler
    {
        private GameSession GameSession { get; set; }
        private Player Player { get; set; }
        
        public ShipInitalizationHandler(GameClient client, int userId, string sessionId, bool newClient = false)
        {
            if (Properties.Game.PRINTING_CONNECTIONS)
                Console.WriteLine("Connection Received, [USERID: " + userId + ", SESSIONID: " + sessionId +
                              "]");

            client.UserId = userId;

            Player = GetAccount(userId);
            if (Player != null) GameSession = CreateSession(client, Player);
            else
            {
                Console.WriteLine("Failed loading user ship / ShipInitializationHandler ERROR");
                return;
            }

            if (Player.SessionId != sessionId)
            {
                ExecuteWrongSession();
                return; // Wrong session ID
            }
            Player.UsingNewClient = newClient;

            execute();
        }

        private Player GetAccount(int userId)
        {
            if (Player != null) return Player;
            return World.DatabaseManager.GetAccount(userId);
        }

        private void ExecuteWrongSession()
        {
            Console.WriteLine($"{GameSession.Client.IPAddress} tried breaching into {GameSession.Client.UserId}'s account");
        }

        private GameSession CreateSession(GameClient client, Player player)
        {
            return new GameSession(player)
            {
                Client = client,
            };
        }

        private void execute()
        {
            if (GameSession == null) return;

            if (!World.StorageManager.GameSessions.ContainsKey(Player.Id))
                World.StorageManager.GameSessions.Add(Player.Id, GameSession);
            else
            {
                World.StorageManager.GameSessions[Player.Id].Disconnect();
                World.StorageManager.GameSessions[Player.Id] = GameSession;
            }
            
            LoginController.Initiate(GameSession);
        }
    }
}