using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using Packet = NettyBaseReloaded.Chat.packet.Packet;

namespace NettyBaseReloaded.Main.commands
{
    class UsersCommand : Command
    {
        public UsersCommand() : base("users", "")
        {
        }

        public override void Execute(string[] args = null)
        {
            
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            var names = World.StorageManager.GameSessions.Values.Select(x => x.Player.Name);
            Packet.Builder.SystemMessage(session, string.Join(", ", names));
        }
    }
}
