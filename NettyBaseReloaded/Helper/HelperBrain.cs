using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Helper.packets;
using NettyBaseReloaded.Helper.packets.commands;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Helper
{
    class HelperBrain
    {
        /// <summary>
        /// Instance -> Generated after connection is established
        /// </summary>
        public static HelperBrain _instance;

        /// <summary>
        /// Handler -> auto generated
        /// </summary>
        public static Handler Handler = new Handler();

        /// <summary>
        /// Client which is going to be for sending packets
        /// </summary>
        private DiscordClient Client;

        /// <summary>
        /// Discord ID: Used for identifying
        /// </summary>
        public string DiscordId { get; set; }

        /// <summary>
        /// Used in chat or other places
        /// </summary>
        public string DiscordName { get; set; }

        public HelperBrain(DiscordClient client)
        {
            Client = client;
        }

        public static void SendCommand(ICommand command)
        {
            var helper = _instance;
            if(helper != null)
                helper.Client.Write(command.Packet);
        }
    }
}
