namespace NettyBaseReloaded.Game.netty.newcommands
{
    class VisualModifierCommand : IServerCommand
    {
        public static short ID = 18881;

        public static short i30 = 32;
        public static short MARKED_PURPLE = 46;
        public static short DRAW_FIRE = 6;
        public static short s33 = 30;
        public static short V3S = 41;
        public static short SINGULARITY = 13;
        public static short INVERTED_CONTROL = 20;
        public static short m3z = 48;
        public static short w2i = 31;
        public static short WRAP_EFFECT = 15;
        public static short INSTANT_SHIELD = 9;
        public static short p22 = 7;
        public static short INSTANT_SHIELD_2 = 27;
        public static short RED_SWORD = 16;
        public static short p2l = 49;
        public static short GREEN_NEON = 22;
        public static short RAYOS_AMARILLOS = 55;
        public static short VENOM_SKILL = 14;
        public static short TIMER = 28;
        public static short s1l = 29;
        public static short o3B = 25;
        public static short CHANGE_SHIP = 18;
        public static short V2P = 44;
        public static short SHIELD_ICON = 37;
        public static short CITADEL_AGGRO = 3;
        public static short BLUE_WAVE = 34;
        public static short BLUE_NEON = 17;
        public static short C1M = 45;
        public static short LANTERN = 54;
        public static short EARTHQUAKE = 56;
        public static short DAMAGE_BOOSTER_ICON = 43;
        public static short sJ = 0;
        public static short INACTIVE = 8;
        public static short DIMINISHER_SKILL_2 = 11;
        public static short MARKED_BLUE = 47;
        public static short i3A = 53;
        public static short YELLOW_NEON = 23;
        public static short INVINCIBILITY = 26;
        public static short q3x = 5;
        public static short ir = 35;
        public static short RED_HEAL_ICON = 36;
        public static short x2I = 1;
        public static short UBA_WINNER = 51;
        public static short Y1F = 38;
        public static short SPECTRUM_SKILL = 10;
        public static short g2 = 42;
        public static short T3L = 21;
        public static short p3V = 39;
        public static short r12 = 2;
        public static short GHOST = 19;
        public static short MS = 40;
        public static short DIMINISHER_SKILL = 12;
        public static short ENERGY_LEECH_ARRAY = 52;
        public static short PURPLE_SHIELD = 4;
        public static short mQ = 24;
        public static short SURGEON_PLAGE = 50;
        public static short yi = 33;

        public int attribute = 0;
        public string varl11 = "";
        public int userId = 0;
        public int count = 0;
        public bool activated = false;
        public short modifier = 0;

        public VisualModifierCommand(bool activated, short modifier, string varl11, int attribute, int userId, int count)
        {
            this.activated = activated;
            this.modifier = modifier;
            this.varl11 = varl11;
            this.attribute = attribute;
            this.userId = userId;
            this.count = count;
        }


        public override void write()
        {
            writeShort((short) ID);
            writeInternal();
        }

        private void writeInternal()
        {
            writeInt(this.attribute >> 2 | this.attribute << 30);
            writeUTF(this.varl11);
            writeInt(this.userId >> 5 | this.userId << 27);
            writeInt(this.count << 7 | this.count >> 25);
            writeBoolean(this.activated);
            writeShort(this.modifier);
        }
    }
}
