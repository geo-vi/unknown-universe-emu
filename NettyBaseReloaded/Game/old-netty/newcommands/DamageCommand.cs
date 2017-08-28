namespace NettyBaseReloaded.Game.netty.newcommands
{
	class DamageCommand : SimpleCommand
	{
		public static int ID = 2540;

		public DamageCommand(DamageEffectModule effect, int damage, int varJ2N, int varY1, int vark2Z, int varO2M, int vard3f, bool var4j)
		{
			writeShort(ID);
			writeInt(damage << 8 | damage >> 24);
			writeInt(varJ2N << 1 | varJ2N >> 31);
			writeInt(varY1 >> 6 | varY1 << 26);
			writeInt(vark2Z << 7 | vark2Z >> 25);
            effect.write();
            writeBytes(effect.command.ToArray());
			writeInt(varO2M >> 6 | varO2M << 26);
			writeInt(vard3f >> 2 | vard3f << 30);
			writeBoolean(var4j);
			writeShort(-26986);
		}
	}
}
