using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class SlotbarCategoryModule
    {
        public const short ID = 21275;

        public string name;
        public List<SlotbarCategoryItemModule> items;

        public SlotbarCategoryModule(string name, List<SlotbarCategoryItemModule> items)
        {
            this.name = name;
            this.items = items;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(items.Count);
            foreach (var c in items)
            {
                cmd.AddBytes(c.write());
            }
            cmd.Short(-25751);
            cmd.UTF(name);
            return cmd.Message.ToArray();
        }

    }
}
