using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.netty.packet;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Networking
{
    class PolicyClient
    {
        private XSocket XSocket;

        public PolicyClient(XSocket gameSocket)
        {
            XSocket = gameSocket;
            XSocket.OnReceive += XSocketOnOnReceive;
            XSocket.Read(true);
        }
        
        private void XSocketOnOnReceive(object sender, EventArgs eventArgs)
        {
            var packet = ((StringArgs)eventArgs).Packet;

            const string policyPacket = "<?xml version=\"1.0\"?>\r\n" +
                                        "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                                        "<cross-domain-policy>\r\n" +
                                        "<allow-access-from domain=\"*\" to-ports=\"*\" />\r\n" +
                                        "</cross-domain-policy>";

            if (packet.StartsWith("<policy-file-request/>"))
                XSocket.Write(policyPacket);
            else
            {
                var ip = XSocket.IpEndPoint.Address.ToString();
                Out.QuickLog("Error with policy request: " + packet + " [" + ip + "]");
                Console.WriteLine("Errorino with policy request: {0}, [{1}]", packet, ip);
            }
        }
    }
}
