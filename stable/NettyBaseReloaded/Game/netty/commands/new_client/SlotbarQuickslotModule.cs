using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class SlotbarQuickslotModule
    {
        public const short ID = 2545;

        public string slotBarId;
        public List<SlotbarQuickslotItem> var2Z;
        public string position;
        public string varW2e;
        public bool visible;

        public SlotbarQuickslotModule(string slotBarId, List<SlotbarQuickslotItem> var2Z, string position, string varW2E, bool visible)
        {
            this.slotBarId = slotBarId;
            this.var2Z = var2Z;
            this.position = position;
            varW2e = varW2E;
            this.visible = visible;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(position);
            cmd.Short(-31484);
            cmd.Integer(var2Z.Count);
            foreach (var c in var2Z)
            {
                cmd.AddBytes(c.write());
            }
            cmd.UTF(slotBarId);
            cmd.Boolean(visible);
            cmd.Short(-8177);
            cmd.UTF(varW2e);
            return cmd.Message.ToArray();
        }
    }
}
