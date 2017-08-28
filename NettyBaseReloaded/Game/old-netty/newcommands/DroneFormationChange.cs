namespace NettyBaseReloaded.Game.netty.newcommands
{
	class DroneFormationChange : SimpleCommand
	{
		public static int ID = 32691;

	    public DroneFormationChange(int playerId, int formationId)
	    {
			writeShort(ID);
            writeShort(-10056);
            writeInt(formationId << 12 | formationId >> 20);
            writeInt(playerId << 13 | playerId >> 19);
        }
    }
}
