using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class BoostedAttributeTypeModule
    {
        public const short EP = 0;

        public const short HONOUR = 1;

        public const short DAMAGE = 2;

        public const short SHIELD = 3;

        public const short REPAIR = 4;

        public const short SHIELDRECHARGE = 5;

        public const short RESOURCE = 6;

        public const short MAXHP = 7;

        public const short ABILITY_COOLDOWN = 8;

        public const short BONUSBOXES = 9;

        public const short QUESTREWARD = 10;

        public const short ID = 16887;

        public short typeValue = 0;

        public BoostedAttributeTypeModule(short typeValue)
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
