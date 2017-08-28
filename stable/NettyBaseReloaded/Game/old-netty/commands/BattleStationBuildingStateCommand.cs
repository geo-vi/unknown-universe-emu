using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class BattleStationBuildingStateCommand
    {
        public const short ID = 23487;
        public static byte[] CMD(int mapAssetId, int battleStationId, string battleStationName, int secondsLeft, int totalSeconds, string ownerClan, FactionModule affiliatedFaction)
        {
            ByteArray enc = new ByteArray(ID);
            enc.Integer(mapAssetId);
            enc.Integer(battleStationId);
            enc.UTF(battleStationName);
            enc.Integer(secondsLeft);
            enc.Integer(totalSeconds);
            enc.UTF(ownerClan);
            enc.AddBytes(affiliatedFaction.write());
            return enc.ToByteArray();
        }
    }
}
