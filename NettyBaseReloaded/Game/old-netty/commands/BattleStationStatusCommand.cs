using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class BattleStationStatusCommand
    {
        public const short ID = 16527;

        public BattleStationStatusCommand(int _mapAssetId, int _battleStationId, string _battleStationName, bool _deflectorShieldActive, int _deflectorShieldSeconds, int _deflectorShieldSecondsMax, int _attackRating,
    int _defenceRating, int _repairRating, int _honorBoosterRating, int _experienceBoosterRating, int _damageBoosterRating, int _deflectorShieldRate, int _repairPrice, EquippedModulesModule _equipment)
        {
            mapAssetId = _mapAssetId;
            battleStationId = _battleStationId;
            battleStationName = _battleStationName;
            deflectorShieldActive = _deflectorShieldActive;
            deflectorShieldSeconds = _deflectorShieldSeconds;
            deflectorShieldSecondsMax = _deflectorShieldSecondsMax;
            attackRating = _attackRating;
            defenceRating = _defenceRating;
            repairRating = _repairRating;
            honorBoosterRating = _honorBoosterRating;
            experienceBoosterRating = _experienceBoosterRating;
            damageBoosterRating = _damageBoosterRating;
            deflectorShieldRate = _deflectorShieldRate;
            repairPrice = _repairPrice;
            equipment = _equipment;
        }

        private int mapAssetId;
        private int battleStationId;
        private string battleStationName;
        private bool deflectorShieldActive;
        private int deflectorShieldSeconds;
        private int deflectorShieldSecondsMax;
        private int attackRating;
        private int defenceRating;
        private int repairRating;
        private int honorBoosterRating;
        private int experienceBoosterRating;
        private int damageBoosterRating;
        private int deflectorShieldRate;
        private int repairPrice;
        private EquippedModulesModule equipment;

        public byte[] write()
        {
            ByteArray enc = new ByteArray(ID);
            enc.Integer(mapAssetId);
            enc.Integer(battleStationId);
            enc.UTF(battleStationName);
            enc.Boolean(deflectorShieldActive);
            enc.Integer(deflectorShieldSeconds);
            enc.Integer(deflectorShieldSecondsMax);
            enc.Integer(attackRating);
            enc.Integer(defenceRating);
            enc.Integer(repairRating);
            enc.Integer(honorBoosterRating);
            enc.Integer(experienceBoosterRating);
            enc.Integer(damageBoosterRating);
            enc.Integer(deflectorShieldRate);
            enc.Integer(repairPrice);
            enc.AddBytes(equipment.write());
            return enc.Message.ToArray();
        }

    }
}
