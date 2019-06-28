using Server.Utils;

namespace Server.Game.netty.commands.old_client
{
    class POITypeModule
    {
        public const short ID = 17741;

        public short typeValue = 0;

        public POITypeModule(short typeValue)
        {
            this.typeValue = typeValue;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(typeValue);
            return cmd.Message.ToArray();
        }
    }
}