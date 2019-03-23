using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Helper.packets.commands
{
    class ChatLogCommand : ICommand
    {
        public override string Prefix => "cl";

        public ChatLogCommand(Character sender, int roomId, string log)
        {
            AddParam(roomId.ToString());
            AddParam(sender.Id.ToString());
            AddParam(sender.Name);
            if (sender is Moderator senderAsMod)
            {
                AddParam(senderAsMod.AdminLevel.ToString());
            }
            else AddParam("USER");

            if (log.Contains('|'))
            {
                log = log.Replace('|', '/');
            }

            if (log.Contains("LINE_SEPERATOR"))
            {
                log = log.Replace("LINE_SEPERATOR", "#");
            }
            AddParam(log);
        }
    }
}
