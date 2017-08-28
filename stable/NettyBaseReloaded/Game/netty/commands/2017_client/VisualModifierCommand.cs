using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class VisualModifierCommand
    {
        public const short i30 = 32;
        public const short MARKED_PURPLE = 46;
        public const short DRAW_FIRE = 6;
        public const short s33 = 30;
        public const short V3S = 41;
        public const short SINGULARITY = 13;
        public const short INVERTED_CONTROL = 20;
        public const short m3z = 48;
        public const short w2i = 31;
        public const short WRAP_EFFECT = 15;
        public const short INSTANT_SHIELD = 9;
        public const short p22 = 7;
        public const short INSTANT_SHIELD_2 = 27;
        public const short RED_SWORD = 16;
        public const short p2l = 49;
        public const short GREEN_NEON = 22;
        public const short RAYOS_AMARILLOS = 55;
        public const short VENOM_SKILL = 14;
        public const short TIMER = 28;
        public const short s1l = 29;
        public const short o3B = 25;
        public const short CHANGE_SHIP = 18;
        public const short V2P = 44;
        public const short SHIELD_ICON = 37;
        public const short CITADEL_AGGRO = 3;
        public const short BLUE_WAVE = 34;
        public const short BLUE_NEON = 17;
        public const short C1M = 45;
        public const short LANTERN = 54;
        public const short EARTHQUAKE = 56;
        public const short DAMAGE_BOOSTER_ICON = 43;
        public const short sJ = 0;
        public const short INACTIVE = 8;
        public const short DIMINISHER_SKILL_2 = 11;
        public const short MARKED_BLUE = 47;
        public const short i3A = 53;
        public const short YELLOW_NEON = 23;
        public const short INVINCIBILITY = 26;
        public const short q3x = 5;
        public const short ir = 35;
        public const short RED_HEAL_ICON = 36;
        public const short x2I = 1;
        public const short UBA_WINNER = 51;
        public const short Y1F = 38;
        public const short SPECTRUM_SKILL = 10;
        public const short g2 = 42;
        public const short T3L = 21;
        public const short p3V = 39;
        public const short r12 = 2;
        public const short GHOST = 19;
        public const short MS = 40;
        public const short DIMINISHER_SKILL = 12;
        public const short ENERGY_LEECH_ARRAY = 52;
        public const short PURPLE_SHIELD = 4;
        public const short mQ = 24;
        public const short SURGEON_PLAGE = 50;
        public const short yi = 33;

        public const short ID = 18881;

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

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(this.attribute >> 2 | this.attribute << 30);
            cmd.UTF(this.varl11);
            cmd.Integer(this.userId >> 5 | this.userId << 27);
            cmd.Integer(this.count << 7 | this.count >> 25);
            cmd.Boolean(this.activated);
            cmd.Short(this.modifier);
            return cmd.Message.ToArray();
        }
    }
}
