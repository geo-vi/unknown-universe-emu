using System;
using NettyBaseReloaded.Chat.controllers;
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

            if (!ValidateSession(sessionId)) return;

            client.UserId = id;

            Character character;

            if (Chat.StorageManager.Moderators.ContainsKey(id))
                character = Chat.StorageManager.Moderators[id];
            else character = new Player(id, username, Global.StorageManager.GetClan(clanTag));
            //character = new Moderator(id, username, Global.StorageManager.GetClan(clanTag), Moderator.Level.MOD);

            var chatSession = new ChatSession(character) {Client = client};

            if (Chat.StorageManager.ChatSessions.ContainsKey(id))
                Chat.StorageManager.ChatSessions[id] = chatSession;
            else Chat.StorageManager.ChatSessions.Add(id, chatSession);

            new LoginController(chatSession);
        }

        private bool ValidateSession(string sessionId)
        {
            return true;
        }
    }
}