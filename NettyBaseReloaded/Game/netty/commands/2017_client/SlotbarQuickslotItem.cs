using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class SlotbarQuickslotItem
    {
        public const short ID = 9992;

        public int slotId;
        public string lootId;

        public SlotbarQuickslotItem(int slotId, string lootId)
        {
            this.slotId = slotId;
            this.lootId = lootId;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(slotId >> 3 | slotId << 29);
            cmd.UTF(lootId);
            return cmd.Message.ToArray();
        }

    }
}
