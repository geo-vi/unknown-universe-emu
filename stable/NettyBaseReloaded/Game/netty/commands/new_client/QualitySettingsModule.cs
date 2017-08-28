using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class QualitySettingsModule
    {
        public const short ID = 7571;

        private bool varN2e;
        private short qualityAttack;
        private short qualityBackground;
        private short qualityPresetting;
        private bool qualityCustomized;
        private short vari4j;
        private short qualityShip;
        private short qualityEngine;
        private short qualityExplosion;
        private short varq2F;
        private short qualityEffect;

        public QualitySettingsModule(bool varN2E, short qualityAttack, short qualityBackground, short qualityPresetting, bool qualityCustomized, short vari4J, short qualityShip, short qualityEngine, short qualityExplosion, short varq2F, short qualityEffect)
        {
            this.varN2e = varN2E;
            this.qualityAttack = qualityAttack;
            this.qualityBackground = qualityBackground;
            this.qualityPresetting = qualityPresetting;
            this.qualityCustomized = qualityCustomized;
            this.vari4j = vari4J;
            this.qualityShip = qualityShip;
            this.qualityEngine = qualityEngine;
            this.qualityExplosion = qualityExplosion;
            this.varq2F = varq2F;
            this.qualityEffect = qualityEffect;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Short(qualityAttack);
            cmd.Short(qualityPresetting);
            cmd.Short(vari4j);
            cmd.Short(qualityShip);
            cmd.Short(qualityExplosion);
            cmd.Boolean(qualityCustomized);
            cmd.Short(qualityBackground);
            cmd.Short(qualityEffect);
            cmd.Boolean(varN2e);
            cmd.Short(qualityEngine);
            cmd.Short(varq2F);
            return cmd.Message.ToArray();
        }
    }
}
