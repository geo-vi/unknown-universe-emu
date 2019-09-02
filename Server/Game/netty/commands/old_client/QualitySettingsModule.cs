using Server.Utils;
using System;

namespace Server.Game.netty.commands.old_client
{
    class QualitySettingsModule
    {
        public const short ID = 4962;

        public Boolean notSet = false;

        public short qualityAttack = 0;

        public short qualityBackground = 0;

        public short qualityPresetting = 0;

        public Boolean qualityCustomized = false;

        public short qualityPOIzone = 0;

        public short qualityShip = 0;

        public short qualityEngine = 0;

        public short qualityExplosion = 0;

        public short qualityCollectables = 0;

        public short qualityEffect = 0;

        public QualitySettingsModule(bool param1 = false, short param2 = 0, short param3 = 0, short param4 = 0, bool param5 = false, short param6 = 0, short param7 = 0, short param8 = 0, short param9 = 0, short param10 = 0, short param11 = 0)
        {
            this.notSet = param1;
            this.qualityAttack = param2;
            this.qualityBackground = param3;
            this.qualityPresetting = param4;
            this.qualityCustomized = param5;
            this.qualityPOIzone = param6;
            this.qualityShip = param7;
            this.qualityEngine = param8;
            this.qualityExplosion = param9;
            this.qualityCollectables = param10;
            this.qualityEffect = param11;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(this.notSet);
            cmd.Short(this.qualityAttack);
            cmd.Short(this.qualityBackground);
            cmd.Short(this.qualityPresetting);
            cmd.Boolean(this.qualityCustomized);
            cmd.Short(this.qualityPOIzone);
            cmd.Short(this.qualityShip);
            cmd.Short(this.qualityEngine);
            cmd.Short(this.qualityExplosion);
            cmd.Short(this.qualityCollectables);
            cmd.Short(this.qualityEffect);
            return cmd.Message.ToArray();
        }
    }
}
