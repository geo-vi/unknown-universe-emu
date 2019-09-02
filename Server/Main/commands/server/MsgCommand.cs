using System;
using System.Linq;
using DotNetty.Common.Utilities;
using Server.Game.managers;
using Server.Game.netty;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Utils;

namespace Server.Main.commands.server
{
    class MsgCommand : GlobalCommand
    {
        public MsgCommand() : base("msg", "Message command", true, new []
        {
            new SubHelp("server", "Sending message to server"), 
            new SubHelp("client", "Sending message to client (*ID REQUIRED)"), 
        })
        {
        }

        public override void Execute(string[] args = null)
        {
            try
            {
                string join = "";
                switch (args[1])
                {
                    case "server":
                        join = string.Join(" ",args.Slice(0, 1));
                        PrebuiltLegacyCommands.Instance.GlobalServerMessage(join);
                        break;
                    case "client":
                        join = string.Join(" ", args.Skip(3));
                        
                        if (int.TryParse(args[2], out var playerId))
                        {
                            var session = GameStorageManager.Instance.FindSession(playerId);
                            PrebuiltLegacyCommands.Instance.ServerMessage(session.Player, join);
                            Out.WriteLog("Message sent to " + session.Player.Name + "\n[" + join + "]");
                        }
                        break;
                }
            }
            catch (Exception)
            {
                Out.WriteLog("Failed sending a message");
            }
        }
    }
}