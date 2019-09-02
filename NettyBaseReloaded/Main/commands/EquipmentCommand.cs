using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Main.commands
{
    class EquipmentCommand : Command
    {
        public EquipmentCommand() : base("equipment", "", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
        }

        public override void Execute(ChatSession session, string[] args = null)
        {

        }
    }
}
