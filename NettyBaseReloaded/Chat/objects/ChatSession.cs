using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.objects
{
    class ChatSession
    {
        public ChatClient Client { get; set; }

        public Player Player { get; set; }

        public ChatSession(Player player)
        {
            Player = player;
        }

        /// <summary>
        /// Gets the GameSession of the player
        /// </summary>
        /// <returns></returns>
        public GameSession GetEquivilentGameSession()
        {
            var id = Player.Id;
            var sessionId = Player.SessionId;
            var worldSession = World.StorageManager.GetGameSession(id);
            if (worldSession != null && worldSession.Player.Id == id && worldSession.Player.SessionId == sessionId)
            {
                return worldSession;
            }

            return null;
        }

        public void Kick(string reason)
        {
            MessageController.System(Player, reason);
            Close();
        }

        public void Close()
        {
            Player.DisconnectRooms();
            Client.Disconnect();
        }
    }
}
