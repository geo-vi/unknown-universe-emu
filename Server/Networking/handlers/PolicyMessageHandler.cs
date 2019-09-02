using System;
using System.Net;
using DotNetty.Transport.Channels;
using Server.Game.managers;
using Server.Utils;

namespace Server.Networking.handlers
{
    class PolicyMessageHandler : ChannelHandlerAdapter
    {
        private const string POLICY_SERVER_RESPONSE = "<?xml version=\"1.0\"?>\r\n" +
                                    "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                                    "<cross-domain-policy>\r\n" +
                                    "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n" +
                                    "</cross-domain-policy>";

        private const string POLICY_REQUEST = @"<policy-file-request/>";
        
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var ip = context.Channel.RemoteAddress as IPEndPoint;
            var msgIn = (string) message;
            
            Out.QuickLog("Received Policy Message from [" + ip.Address + "]: \n![" + msgIn + "] vs [" + POLICY_REQUEST + "]");
            
            if (msgIn.Contains(POLICY_REQUEST) && msgIn.StartsWith(POLICY_REQUEST))
            {
                Out.QuickLog("Received policy request, proceeding...");
                context.WriteAndFlushAsync(POLICY_SERVER_RESPONSE);
                WhitelistManager.Instance.AddToWhitelist(ip.Address);
            }
            else
            {
                context.WriteAndFlushAsync("When was the last time you heard about the BAN ZONE? " +
                                           "ps: your ip is recorded\nLet's play a little game :jokerface: )))))");
                WhitelistManager.Instance.AddToBlacklist(ip.Address, "Wrong Policy, policy readed: " + message);
            }
            context.CloseAsync();
            Out.QuickLog("Closing policy server");
        }
    }
}
