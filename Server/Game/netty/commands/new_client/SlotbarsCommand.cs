using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.new_client
{
    class SlotbarsCommand
    {
        public const short ID = 10437;

        public static Command write(List<SlotbarCategoryModule> categories, string varnz, List<SlotbarQuickslotModule> slotBars)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(categories.Count);
            foreach (var c in categories)
            {
                cmd.AddBytes(c.write());
            }
            cmd.Short(7085);
            cmd.UTF(varnz);
            cmd.Integer(slotBars.Count);
            foreach (var c in slotBars)
            {
                cmd.AddBytes(c.write());
            }
            return new Command(cmd.ToByteArray(), true);
        }
    }
}
