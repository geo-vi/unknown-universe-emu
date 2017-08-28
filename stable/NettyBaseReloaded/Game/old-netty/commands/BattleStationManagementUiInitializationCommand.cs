using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class BattleStationManagementUiInitializationCommand
    {
        public const short ID = 16007;
        public static byte[] write(int mapAssetId, int battleStationId, string battleStationName, string clanName, FactionModule faction, BattleStationStatusCommand state, AvailableModulesCommand availableModules,
    int deflectorShieldMinutesMin, int deflectorShieldMinutesMax, int deflectorShieldMinutesIncrement, bool deflectorDeactivationPossible)
        {
            ByteArray enc = new ByteArray(ID);
            enc.Integer(mapAssetId);
            enc.Integer(battleStationId);
            enc.UTF(battleStationName);
            enc.UTF(clanName);
            enc.AddBytes(faction.write());
            enc.AddBytes(state.write());
            enc.AddBytes(availableModules.write());
            enc.Integer(deflectorShieldMinutesMin);
            enc.Integer(deflectorShieldMinutesMax);
            enc.Integer(deflectorShieldMinutesIncrement);
            enc.Boolean(deflectorDeactivationPossible);
            return enc.ToByteArray();
        }
    }
}
