using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class LootModule
    {
        public const short ID = 15835;

        public string lootId;
        public int amount;

        public LootModule(string lootId, int amount)
        {
            this.lootId = lootId;
            this.amount = amount;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(lootId);
            cmd.Integer(amount);
            return cmd.Message.ToArray();
        }
    }
}
