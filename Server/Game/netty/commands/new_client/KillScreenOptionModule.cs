using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class KillScreenOptionModule
    {
        public const short ID = 16458;

        public int cooldownTime;
        public KillScreenOptionTypeModule varxo;
        public MessageLocalizedWildcardCommand toolTipKey;
        public MessageLocalizedWildcardCommand varg2i;
        public MessageLocalizedWildcardCommand vart15;
        public PriceModule price;
        public MessageLocalizedWildcardCommand varq36;
        public bool affordableForPlayer;

        public KillScreenOptionModule(int cooldownTime, KillScreenOptionTypeModule varxo, MessageLocalizedWildcardCommand toolTipKey, MessageLocalizedWildcardCommand varg2I, MessageLocalizedWildcardCommand vart15, PriceModule price, MessageLocalizedWildcardCommand varq36, bool affordableForPlayer)
        {
            this.cooldownTime = cooldownTime;
            this.varxo = varxo;
            this.toolTipKey = toolTipKey;
            varg2i = varg2I;
            this.vart15 = vart15;
            this.price = price;
            this.varq36 = varq36;
            this.affordableForPlayer = affordableForPlayer;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(cooldownTime >> 7 | cooldownTime << 25);
            cmd.AddBytes(varxo.write());
            cmd.AddBytes(toolTipKey.write());
            cmd.AddBytes(varg2i.write());
            cmd.AddBytes(vart15.write());
            cmd.AddBytes(price.write());
            cmd.AddBytes(varq36.write());
            cmd.Boolean(affordableForPlayer);
            return cmd.Message.ToArray();
        }
    }
}