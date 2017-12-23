using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class KillScreenOptionModule
    {
        public const short ID = 31597;

        public KillScreenOptionTypeModule repairType;

        public PriceModule price;

        public bool affordableForPlayer;

        public int cooldownTime;

        public MessageLocalizedWildcardCommand descriptionKey;

        public MessageLocalizedWildcardCommand descriptionKeyAddon;

        public MessageLocalizedWildcardCommand toolTipKey;

        public MessageLocalizedWildcardCommand buttonKey;

        public KillScreenOptionModule(KillScreenOptionTypeModule repairType, PriceModule price, bool affordableForPlayer, int cooldownTime,
            MessageLocalizedWildcardCommand descriptionKey, MessageLocalizedWildcardCommand descriptionKeyAddon, MessageLocalizedWildcardCommand toolTipKey, MessageLocalizedWildcardCommand buttonKey)
        {
            this.repairType = repairType;
            this.price = price;
            this.affordableForPlayer = affordableForPlayer;
            this.cooldownTime = cooldownTime;
            this.descriptionKey = descriptionKey;
            this.descriptionKeyAddon = descriptionKeyAddon;
            this.toolTipKey = toolTipKey;
            this.buttonKey = buttonKey;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(repairType.write());
            cmd.AddBytes(price.write());
            cmd.Boolean(affordableForPlayer);
            cmd.Integer(cooldownTime);
            cmd.AddBytes(descriptionKey.write());
            cmd.AddBytes(descriptionKeyAddon.write());
            cmd.AddBytes(toolTipKey.write());
            cmd.AddBytes(buttonKey.write());
            return cmd.Message.ToArray();
        }
    }
}
