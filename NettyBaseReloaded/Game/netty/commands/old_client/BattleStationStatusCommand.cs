using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class BattleStationStatusCommand
    {
        public const short ID = 16527;

        public int mapAssetId;
        public int battleStationId;
        public string battleStationName;
        public bool deflectorShieldActive;
        public int deflectorShieldSeconds;
        public int deflectorShieldSecondsMax;
        public int attackRating;
        public int defenceRating;
        public int repairRating;
        public int honorBoosterRating;
        public int experienceBoosterRating;
        public int damageBoosterRating;
        public int deflectorShieldRate;
        public int repairPrice;
        public EquippedModulesModule equipment;

        public BattleStationStatusCommand(int mapAssetId, int battleStationId, string battleStationName,
            bool deflectorShieldActive, int deflectorShieldSeconds, int deflectorShieldSecondsMax,
            int attackRating, int defenceRating, int repairRating, int honorBoosterRating, int experienceBoosterRating,
            int damageBoosterRating, int deflectorShieldRate, int repairPrice, EquippedModulesModule equipment)
        {
            this.mapAssetId = mapAssetId;
            this.battleStationId = battleStationId;
            this.battleStationName = battleStationName;
            this.deflectorShieldActive = deflectorShieldActive;
            this.deflectorShieldSeconds = deflectorShieldSeconds;
            this.deflectorShieldSecondsMax = deflectorShieldSecondsMax;
            this.attackRating = attackRating;
            this.defenceRating = defenceRating;
            this.repairRating = repairRating;
            this.honorBoosterRating = honorBoosterRating;
            this.experienceBoosterRating = experienceBoosterRating;
            this.damageBoosterRating = damageBoosterRating;
            this.deflectorShieldRate = deflectorShieldRate;
            this.repairPrice = repairPrice;
            this.equipment = equipment;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(mapAssetId);
            cmd.Integer(battleStationId);
            cmd.UTF(battleStationName);
            cmd.Boolean(deflectorShieldActive);
            cmd.Integer(deflectorShieldSeconds);
            cmd.Integer(deflectorShieldSecondsMax);
            cmd.Integer(attackRating);
            cmd.Integer(defenceRating);
            cmd.Integer(repairRating);
            cmd.Integer(honorBoosterRating);
            cmd.Integer(experienceBoosterRating);
            cmd.Integer(damageBoosterRating);
            cmd.Integer(deflectorShieldRate);
            cmd.Integer(repairPrice);
            cmd.AddBytes(equipment.write());
            return cmd.Message.ToArray();
        }
    }
}
