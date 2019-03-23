using System;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.managers;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.packet.handlers
{
    class UserLoginHandler
    {
        public void execute(ChatClient client, string[] packet)
        {
            string username = packet[2];
            int id = int.Parse(packet[3]);
            string sessionId = packet[4];
            int projectBuild = int.Parse(packet[5]);
            string lang = packet[6];
            string clanTag = packet[7];
            string version = packet[8];

            client.UserId = id;
            var player = Chat.DatabaseManager.LoadPlayer(id);
            if (Chat.StorageManager.ChatModerators.ContainsKey(id))
            {
                var mod = Chat.StorageManager.GetChatModerator(id);
                mod.SessionId = player.SessionId;
                player = mod;
            }
            if (player == null) return;

            if (!ValidateSession(player, sessionId))
            {
                //send msg
                client.Send("dq%Invalid Session#");
                return;
            }

            var chatSession = new ChatSession(player) {Client = client};

            if (Chat.StorageManager.ChatSessions.ContainsKey(id))
                Chat.StorageManager.ChatSessions[id] = chatSession;
            else Chat.StorageManager.ChatSessions.Add(id, chatSession);

            new LoginController(chatSession);
        }

        private bool ValidateSession(Character character, string sessionId)
        {
            return character.SessionId == sessionId;
        }
    }
}