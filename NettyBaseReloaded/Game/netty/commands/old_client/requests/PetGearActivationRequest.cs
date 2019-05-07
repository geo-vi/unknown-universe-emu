using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class PetGearActivationRequest
    {
        public const short ID = 6873;

        public PetGearTypeModule gearTypeToActivate;
        public int optParam;

        public void readCommand(IByteBuffer bytes)
        {
            var parser = new ByteParser(bytes);
            parser.readShort(); // command id
            //parser.readShort(); // lenght
            gearTypeToActivate = new PetGearTypeModule(parser.readShort());
            optParam = parser.readShort();
        }
    }
}
