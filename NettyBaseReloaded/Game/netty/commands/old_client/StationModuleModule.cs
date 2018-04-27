using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class StationModuleModule
    {
        public const short NONE = 0;
      
        public const short DESTROYED = 1;
      
        public const short HULL = 2;
      
        public const short DEFLECTOR = 3;
      
        public const short REPAIR = 4;
      
        public const short LASER_HIGH_RANGE = 5;
      
        public const short LASER_MID_RANGE = 6;
      
        public const short LASER_LOW_RANGE = 7;
      
        public const short ROCKET_MID_ACCURACY = 8;
      
        public const short ROCKET_LOW_ACCURACY = 9;
      
        public const short HONOR_BOOSTER = 10;
      
        public const short DAMAGE_BOOSTER = 11;
      
        public const short EXPERIENCE_BOOSTER = 12;


        public const short ID = 30874;

        public int asteroidId;
        public int itemId;
        public int slotId;
        public short type;
        public int currentHitpoints;
        public int maxHitpoints;
        public int currentShield;
        public int maxShield;
        public int upgradeLevel;
        public string ownerName;
        public int installationSeconds;
        public int installationSecondsLeft;
        public int emergencyRepairSecondsLeft;
        public int emergencyRepairSecondsTotal;
        public int emergencyRepairCost;

        public StationModuleModule(int asteroidId, int itemId, int slotId, short type, int currentHitpoints, int maxHitpoints, int currentShield, int maxShield, int upgradeLevel, string ownerName, int installationSeconds,
            int installationSecondsLeft, int emergencyRepairSecondsLeft, int emergencyRepairSecondsTotal, int emergencyRepairCost)
        {
            this.asteroidId = asteroidId;
            this.itemId = itemId;
            this.slotId = slotId;
            this.type = type;
            this.currentHitpoints = currentHitpoints;
            this.maxHitpoints = maxHitpoints;
            this.currentShield = currentShield;
            this.maxShield = maxShield;
            this.upgradeLevel = upgradeLevel;
            this.ownerName = ownerName;
            this.installationSeconds = installationSeconds;
            this.installationSecondsLeft = installationSecondsLeft;
            this.emergencyRepairSecondsLeft = emergencyRepairSecondsLeft;
            this.emergencyRepairSecondsTotal = emergencyRepairSecondsTotal;
            this.emergencyRepairCost = emergencyRepairCost;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(asteroidId);
            cmd.Integer(itemId);
            cmd.Integer(slotId);
            cmd.Short(type);
            cmd.Integer(currentHitpoints);
            cmd.Integer(maxHitpoints);
            cmd.Integer(currentShield);
            cmd.Integer(maxShield);
            cmd.Integer(upgradeLevel);
            cmd.UTF(ownerName);
            cmd.Integer(installationSeconds);
            cmd.Integer(installationSecondsLeft);
            cmd.Integer(emergencyRepairSecondsLeft);
            cmd.Integer(emergencyRepairSecondsTotal);
            cmd.Integer(emergencyRepairCost);
            return cmd.Message.ToArray();
        }
    }
}
