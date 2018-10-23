using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Main.commands
{
    class NpcCommand : Command
    {
        public NpcCommand() : base("npc", "npc controlling command", true, null)
        {
        }

        private static Npc Selected;
        public override void Execute(string[] args = null)
        {
            try
            {
                switch (args[1])
                {
                    case "create":
                        break;
                    case "select":
                        break;
                    case "setai":
                        break;
                    case "setpos":
                        break;
                    case "rename":
                        break;
                    case "destroy":
                        break;
                    case "delete":
                        break;
                    case "heal":
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            if (session.Player.RCON) Execute(args);
        }
    }
}
