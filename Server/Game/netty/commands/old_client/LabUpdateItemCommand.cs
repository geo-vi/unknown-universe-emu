using Server.Utils;
using System.Collections.Generic;

namespace Server.Game.netty.commands.old_client
{
    class LabUpdateItemCommand
    {
        public const short ID = 19873;

        public static Command write(List<UpdateItemModule> itemsUpdatedInfo)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(itemsUpdatedInfo.Count);
            foreach (var item in itemsUpdatedInfo)
            {
                cmd.AddBytes(item.write());
            }
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
