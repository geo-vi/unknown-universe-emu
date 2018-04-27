using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class BattleStationManagementUiInitializationCommand
    {
        public const short ID = 16007;

        public static Command write(int mapAssetId, int battleStationId, string battleStationName, string clanName, FactionModule faction, BattleStationStatusCommand state, AvailableModulesCommand availableModules, 
            int deflectorShieldMinutesMin, int deflectorShieldMinutesMax, int deflectorShieldMinutesIncrement, bool deflectorDeactivationPossible)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(mapAssetId);
            cmd.Integer(battleStationId);
            cmd.UTF(battleStationName);
            cmd.UTF(clanName);
            cmd.AddBytes(faction.write());
            cmd.AddBytes(state.write());
            cmd.AddBytes(availableModules.write());
            cmd.Integer(deflectorShieldMinutesMin);
            cmd.Integer(deflectorShieldMinutesMax);
            cmd.Integer(deflectorShieldMinutesIncrement);
            cmd.Boolean(deflectorDeactivationPossible);
            return new Command(cmd.ToByteArray(), false);
        }
        
    }
}
