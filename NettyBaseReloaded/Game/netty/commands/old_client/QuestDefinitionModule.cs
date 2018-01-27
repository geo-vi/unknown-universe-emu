using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class QuestDefinitionModule
    {
        public const short ID = 14040;

        public int id = 0;
        public List<QuestTypeModule> types;
        public QuestCaseModule rootCase;
        public List<LootModule> rewards;
        public List<QuestIconModule> icons;

        public QuestDefinitionModule(int id, List<QuestTypeModule> types, QuestCaseModule rootCase, List<LootModule> rewards, List<QuestIconModule> icons)
        {
            this.id = id;
            this.types = types;
            this.rootCase = rootCase;
            this.rewards = rewards;
            this.icons = icons;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(id);
            cmd.Integer(types.Count);
            foreach (var loc0 in types)
            {
                cmd.AddBytes(loc0.write());
            }
            cmd.AddBytes(rootCase.write());
            cmd.Integer(rewards.Count);
            foreach (var loc1 in rewards)
            {
                cmd.AddBytes(loc1.write());
            }
            cmd.Integer(icons.Count);
            foreach (var loc2 in icons)
            {
                cmd.AddBytes(loc2.write());
            }
            return cmd.Message.ToArray();
        }
    }
}
