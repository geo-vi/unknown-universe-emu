using Server.Utils;

namespace Server.Game.netty.commands.new_client
{
    class BeaconCommand
    {
        public const short ID = 311;

        /// <summary>
        ///
        /// </summary>
        /// <param name="var1">useless</param>
        /// <param name="var2">useless</param>
        /// <param name="var3">useless</param>
        /// <param name="var4">useless</param>
        /// <param name="inDemiZone"></param>
        /// <param name="var6">repairing?</param>
        /// <param name="var7">active</param>
        /// <param name="var8">equipment_extra_repbot_rep-4</param>
        /// <param name="inRadiationZone"></param>
        /// <returns></returns>
        public static Command write(int var1, int var2, int var3, int var4, bool inDemiZone, bool var6, bool var7, string var8, bool inRadiationZone)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(var4 << 3 | var4 >> 29);
            cmd.Boolean(inRadiationZone);
            cmd.Short(-1851);
            cmd.Integer(var2 << 3 | var2 >> 29);
            cmd.Boolean(var7);
            cmd.Integer(var1 >> 1 | var1 << 31);
            cmd.Integer(var3 << 14 | var3 >> 18);
            cmd.Boolean(var6);
            cmd.Boolean(inDemiZone);
            cmd.UTF(var8);
            return new Command(cmd.ToByteArray(), true);
        }
    }
}