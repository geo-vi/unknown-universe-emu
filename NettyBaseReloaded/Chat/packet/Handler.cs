using System;
using System.Collections.Generic;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.packet.handlers;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Chat.packet
{
    class Handler
    {
        public Handler() { AddHandlers(); }

        private Dictionary<string, IHandler> HandledPackets = new Dictionary<string, IHandler>();
        public void AddHandlers()
        {
            HandledPackets.Add(Constants.CMD_USER_MSG, new UserMessageHandler());
        }

        public void Handle(ChatClient client, string content)
        {
            ChatSession chatSession = null;

            chatSession = Chat.StorageManager.GetChatSession(client.UserId);

            // Getting rid of all the '@'
            var packet = content.Replace("@", "%");
            //Console.WriteLine("CHAT> " + packet);

            // Converting it to char and then splitting the packet
            var msgSeperator = Convert.ToChar(Constants.MSG_SEPERATOR);
            var paramSplitter = packet.Split(msgSeperator);

            if (paramSplitter[0] == Constants.CMD_USER_LOGIN)
            {
                new UserLoginHandler().execute(client, paramSplitter);
            }
            else
            {
                if (HandledPackets.ContainsKey(paramSplitter[0]))
                    HandledPackets[paramSplitter[0]].execute(chatSession, paramSplitter);
            }
        }
    }
}