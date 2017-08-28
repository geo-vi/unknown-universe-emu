using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class StationModuleModule
    {
        public static short NONE = 0;
        public static short DESTROYED = 1;
        public static short HULL = 2;
        public static short DEFLECTOR = 3;
        public static short REPAIR = 4;
        public static short LASER_HIGH_RANGE = 5;
        public static short LASER_MID_RANGE = 6;
        public static short LASER_LOW_RANGE = 7;
        public static short ROCKET_MID_ACCURACY = 8;
        public static short ROCKET_LOW_ACCURACY = 9;
        public static short HONOR_BOOSTER = 10;
        public static short DAMAGE_BOOSTER = 11;
        public static short EXPERIENCE_BOOSTER = 12;

        public const short ID = 30874;

        public StationModuleModule(int _asteroidId, int _itemId, int _slotId, short _type, int _currentHitpoints, int _maxHitpoints, int _currentShield, int _maxShield,
            int _upgradeLevel, string _ownerName, int _installationSeconds, int _installationSecondsLeft, int _emergencyRepairSecondsLeft, int _emergencyRepairSecondsTotal, int _emergencyRepairCost)
        {
            asteroidId = _asteroidId;
            itemId = _itemId;
            slotId = _slotId;
            type = _type;
            currentHitpoints = _currentHitpoints;
            maxHitpoints = _maxHitpoints;
            currentShield = _currentShield;
            maxShield = _maxShield;
            upgradeLevel = _upgradeLevel;
            ownerName = _ownerName;
            installationSeconds = _installationSeconds;
            installationSecondsLeft = _installationSecondsLeft;
            emergencyRepairSecondsLeft = _emergencyRepairSecondsLeft;
            emergencyRepairSecondsTotal = _emergencyRepairSecondsTotal;
            emergencyRepairCost = _emergencyRepairCost;

        }

        private int asteroidId;
        private int itemId;
        private int slotId;
        private short type;
        private int currentHitpoints;
        private int maxHitpoints;
        private int currentShield;
        private int maxShield;
        private int upgradeLevel;
        private string ownerName;
        private int installationSeconds;
        private int installationSecondsLeft;
        private int emergencyRepairSecondsLeft;
        private int emergencyRepairSecondsTotal;
        private int emergencyRepairCost;

        public byte[] write()
        {
            ByteArray enc = new ByteArray(ID);
            enc.Integer(asteroidId);
            enc.Integer(itemId);
            enc.Integer(slotId);
            enc.Short(type);
            enc.Integer(currentHitpoints);
            enc.Integer(maxHitpoints);
            enc.Integer(currentShield);
            enc.Integer(maxShield);
            enc.Integer(upgradeLevel);
            enc.UTF(ownerName);
            enc.Integer(installationSeconds);
            enc.Integer(installationSecondsLeft);
            enc.Integer(emergencyRepairSecondsLeft);
            enc.Integer(emergencyRepairSecondsTotal);
            enc.Integer(emergencyRepairCost);
            return enc.Message.ToArray();
        }

    }
}
