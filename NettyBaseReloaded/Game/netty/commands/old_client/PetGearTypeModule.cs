using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class PetGearTypeModule
    {
        public const short BEHAVIOR = 0;
      
        public const short PASSIVE = 1;
      
        public const short GUARD = 2;
      
        public const short GEAR = 3;
      
        public const short AUTO_LOOT = 4;
      
        public const short AUTO_RESOURCE_COLLECTION = 5;
      
        public const short ENEMY_LOCATOR = 6;
      
        public const short RESOURCE_LOCATOR = 7;
      
        public const short TRADE_POD = 8;
      
        public const short REPAIR_PET = 9;
      
        public const short KAMIKAZE = 10;
      
        public const short COMBO_SHIP_REPAIR = 11;
      
        public const short COMBO_GUARD = 12;
      
        public const short ADMIN = 13;

        public const short ID = 3314;

        public short typeValue;

        public PetGearTypeModule(short typeValue)
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
