namespace NettyBaseReloaded.Game.netty.newcommands
{
    class MovementCommand : SimpleCommand
    {
        public static int ID = 1771;
        public MovementCommand(int varGX, int x, int y, int varF2K)
        {
            writeShort(ID);
            writeShort(22713);
            writeShort(4581);
            writeInt(y >> 1 | y << 31);
            writeInt(varGX >> 10 | varGX << 22);
            writeInt(x << 6 | x >> 26);
            writeInt(varF2K << 11 | varF2K >> 21);
        }
    }
}