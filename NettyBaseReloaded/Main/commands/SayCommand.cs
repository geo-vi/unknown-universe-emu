using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;

namespace NettyBaseReloaded.Main.commands
{
    class SayCommand : Command
    {
        public SayCommand() : base("say", "")
        {
        }

        public override void Execute(string[] args = null)
        {
            if (args[1] == "chat")
            {
                Chat.controllers.MessageController.SystemMessageToAll(string.Join(" ",args).Replace(args[0] + " " + args[1], ""));
            }
            else
            {
                World.StreamMessageToWorld(string.Join(" ", args).Replace(args[0], ""));
            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            if (session.Player.RCON) Execute(args);
        }
    }
}
