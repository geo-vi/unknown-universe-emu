using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Helper.packets.requests;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Helper.packets.handlers
{
    class DiscordChannelMessageHandler : IHandler
    {
        public void Execute(HelperBrain brain, string[] packet)
        {
            var request = new DiscordChannelMessageRequest();
            request.Read(packet);

            var globalRoom = Chat.Chat.StorageManager.Rooms[0];

            ChatClient.SendToRoom("j%" + globalRoom.Id + "@" + request.Username.Replace("#", "|") + "@" + request.Content + "@" +
                                  1 + "#", globalRoom);
        }
    }
}
