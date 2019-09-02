using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetty.Buffers;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class QualitySettingsRequest
    {
        public const short ID = 26693;

        public short qualityAttack = 0;
        public short qualityBackground = 0;
        public short qualityPresetting = 0;
        public bool qualityCustomized = false;
        public short qualityPOIzone = 0;
        public short qualityShip = 0;
        public short qualityEngine = 0;
        public short qualityExplosion = 0;
        public short qualityCollectables = 0;
        public short qualityEffect = 0;

        public void readCommand(IByteBuffer bytes)
        {
            var cmd = new ByteParser(bytes);
            this.qualityAttack = cmd.readShort();
            this.qualityBackground = cmd.readShort();
            this.qualityPresetting = cmd.readShort();
            this.qualityCustomized = cmd.readBool();
            this.qualityPOIzone = cmd.readShort();
            this.qualityShip = cmd.readShort();
            this.qualityEngine = cmd.readShort();
            this.qualityExplosion = cmd.readShort();
            this.qualityCollectables = cmd.readShort();
            this.qualityEffect = cmd.readShort();
        }
    }
}
