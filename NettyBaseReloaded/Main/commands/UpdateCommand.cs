using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;

namespace NettyBaseReloaded.Main.commands
{
    class UpdateCommand : Command
    {
        public UpdateCommand() : base("update", "", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            World.StorageManager.Quests.Clear();
            World.DatabaseManager.LoadQuests();
            foreach (var session in World.StorageManager.GameSessions)
            {
                Packet.Builder.QuestListCommand(session.Value);
            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            throw new NotImplementedException();
        }
    }
}
