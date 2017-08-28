namespace NettyBaseReloaded.Game.netty.newcommands.settingsModules
{
	class QualitySettingsModule : IServerCommand
	{
        public static int ID = 7571;

        private bool varN2e;
        private int qualityAttack;
        private int qualityBackground;
        private int qualityPresetting;
        private bool qualityCustomized;
        private int vari4j;
        private int qualityShip;
        private int qualityEngine;
        private int qualityExplosion;
        private int varq2F;
        private int qualityEffect;

        public QualitySettingsModule(bool varN2E, int qualityAttack, int qualityBackground, int qualityPresetting, bool qualityCustomized, int vari4J, int qualityShip, int qualityEngine, int qualityExplosion, int varq2F, int qualityEffect)
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

        public override void write()
        {
            writeShort(ID);
            writeShort(qualityAttack);
            writeShort(qualityPresetting);
            writeShort(vari4j);
            writeShort(qualityShip);
            writeShort(qualityExplosion);
            writeBoolean(qualityCustomized);
            writeShort(qualityBackground);
            writeShort(qualityEffect);
            writeBoolean(varN2e);
            writeShort(qualityEngine);
            writeShort(varq2F);
        }
    }
}
