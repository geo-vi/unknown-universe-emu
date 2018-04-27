using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class BattleStationBuildingUiInitializationCommand
    {
        public const short ID = 14733;

        public static Command write(int mapAssetId, int battleStationId, string battleStationName, AsteroidProgressCommand progress, AvailableModulesCommand availableModules, int buildTimeInMinutesMin, int buildTimeInMinutesMax, int buildTimeInMinutesIncrement)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(mapAssetId);
            cmd.Integer(battleStationId);
            cmd.UTF(battleStationName);
            cmd.AddBytes(progress.write());
            cmd.AddBytes(availableModules.write());
            cmd.Integer(buildTimeInMinutesMin);
            cmd.Integer(buildTimeInMinutesMax);
            cmd.Integer(buildTimeInMinutesIncrement);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
