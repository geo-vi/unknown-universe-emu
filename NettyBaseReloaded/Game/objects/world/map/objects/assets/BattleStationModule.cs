using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map.objects.assets.cbs;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class BattleStationModule : AttackableAsset
    {
        public static Vector SLOT_01 = new Vector(-413, -98);
        public static Vector SLOT_02 = new Vector(-171, -236);
        public static Vector SLOT_03 = new Vector(+170, +236);
        public static Vector SLOT_04 = new Vector(+412, -98);
        public static Vector SLOT_05 = new Vector(412, +97);
        public static Vector SLOT_06 = new Vector(170, -235);
        public static Vector SLOT_07 = new Vector(-171, +235);
        public static Vector SLOT_08 = new Vector(-413, +97);

        /// <summary>
        /// Equipped into asteroid Id
        /// </summary>
        public ClanBattleStation BattleStation { get; set; }

        public int SlotId { get; set; }

        public Item Item { get; set; }

        public int UpgradeLevel { get; set; } = 16;

        public Player Owner;

        public Module.Types ModuleType { get; set; }

        /// <summary>
        /// Installation
        /// </summary>

        #region Installation
        public bool InstallationActive { get; set; } = false;
        public DateTime InstallationStart;
        public DateTime InstallationEnd;
        #endregion

        /// <summary>
        /// Emergency repair
        /// </summary>
        #region Emergency Repair
        public bool EmergencyRepairActive { get; set; } = false;
        public DateTime EmergencyRepairStart;
        public DateTime EmergencyRepairEnd;
        #endregion

        public int EmergencyRepairCost => 0; // 0 U.

        public BattleStationModule(int id, string name, AssetTypes type, Faction faction, Clan clan, int designId, int expansionStage, Vector position, Spacemap map, bool invisible, bool visibleOnWarnRadar, bool detectedByWarnRadar, int hp, int maxHp, int shd, int maxShd, int nano, int maxNano, double abs, double pen) : base(id, name, type, faction, clan, designId, expansionStage, position, map, invisible, visibleOnWarnRadar, detectedByWarnRadar, hp, maxHp, shd, maxShd, nano, maxNano, abs, pen)
        {
        }

        protected BattleStationModule(Player player, Module module, Asteroid asteroid, Module.Types type) : this(asteroid.Spacemap.GetNextObjectId(), "",
            AssetTypes.SATELLITE, asteroid.Faction, player.Clan, 0, 0, asteroid.Position,
            asteroid.Spacemap, false, false, false, 1000, 1000, 1000, 1000, 0, 0, 0, 0)
        {
            Owner = player;
            Item = module.Item;
            ModuleType = type;
            Spacemap.Objects.TryAdd(Id, null); //assign the ID
            DesignId = ToSatelliteType();
        }

        private int ToSatelliteType()
        {
            switch (ModuleType)
            {
                case Module.Types.HULL:
                    return 1;
                case Module.Types.DEFLECTOR:
                    return 2;
                case Module.Types.REPAIR:
                    return 3;
                case Module.Types.LASER_LOW_RANGE:
                    return 4;
                case Module.Types.LASER_MID_RANGE:
                    return 5;
                case Module.Types.LASER_HIGH_RANGE:
                    return 6;
                case Module.Types.ROCKET_LOW_ACCURACY:
                    return 7;
                case Module.Types.ROCKET_MID_ACCURACY:
                    return 8;
                case Module.Types.HONOR_BOOSTER:
                    return 9;
                case Module.Types.DAMAGE_BOOSTER:
                    return 10;
                case Module.Types.EXPERIENCE_BOOSTER:
                    return 11;
                default: return 0;
            }
        }

        public override void Tick()
        {
            if (InstallationActive && GetInstallationSeconds() == 0)
                FinishInstallation();
        }

        public static BattleStationModule Equip(Player equipper, Module module, Asteroid asteroid, int slotId, int installationTime = 45)
        {
            if (module.Equipped || module.Destroyed || slotId == 0 && module.ModuleType != Module.Types.HULL || 
                slotId == 1 && module.ModuleType != Module.Types.DEFLECTOR || slotId != 0 && module.ModuleType == Module.Types.HULL ||
                slotId != 1 && module.ModuleType == Module.Types.DEFLECTOR || asteroid.EquippedModules.Any(x => x.Value.SlotId == slotId) || (module.ModuleType == Module.Types.DAMAGE_BOOSTER || module.ModuleType == Module.Types.HONOR_BOOSTER || module.ModuleType == Module.Types.EXPERIENCE_BOOSTER) && asteroid.EquippedModules.Values.Any(x =>
                    x.Clan == equipper.Clan && (x.ModuleType == Module.Types.DAMAGE_BOOSTER ||
                                       x.ModuleType == Module.Types.EXPERIENCE_BOOSTER ||
                                       x.ModuleType == Module.Types.HONOR_BOOSTER))) return null;
            module.Equipped = true;
            module.EquippedBattleStationId = asteroid.AssignedBattleStationId;
            BattleStationModule battleStationModule;
            switch (module.ModuleType)
            {
                case Module.Types.LASER_LOW_RANGE:
                    battleStationModule = new LaserStationModule(equipper, module, asteroid, module.ModuleType)
                    {
                        InstallationActive = true,
                        InstallationStart = DateTime.Now,
                        InstallationEnd = DateTime.Now.AddSeconds(installationTime),
                        SlotId = slotId,
                        Name = "LTM-LR",
                        Range = 500,
                        Clan = equipper.Clan
                    };
                    break;
                case Module.Types.LASER_MID_RANGE:
                    battleStationModule = new LaserStationModule(equipper, module, asteroid, module.ModuleType)
                    {
                        InstallationActive = true,
                        InstallationStart = DateTime.Now,
                        InstallationEnd = DateTime.Now.AddSeconds(installationTime),
                        SlotId = slotId,
                        Name = "LTM-MR",
                        Range = 700,
                        Clan = equipper.Clan
                    };
                    break;
                case Module.Types.LASER_HIGH_RANGE:
                    battleStationModule = new LaserStationModule(equipper, module, asteroid, module.ModuleType)
                    {
                        InstallationActive = true,
                        InstallationStart = DateTime.Now,
                        InstallationEnd = DateTime.Now.AddSeconds(installationTime),
                        SlotId = slotId,
                        Name = "LTM-HR",
                        Range = 1000,
                        Clan = equipper.Clan
                    };
                    break;
                default:
                    battleStationModule = new BattleStationModule(equipper, module, asteroid, module.ModuleType)
                    {
                        InstallationActive = true,
                        InstallationStart = DateTime.Now,
                        InstallationEnd = DateTime.Now.AddSeconds(installationTime),
                        SlotId = slotId,
                        Clan = equipper.Clan
                    };
                    break;
            }
            module.BattleStationModule = battleStationModule;
            return battleStationModule;
        }

        public static void AddModule(Module module, int battleStationId, int slotId, Clan clan)
        {//todo ; recode this whole mess up...
        }

        public static Vector GetPos(Vector center,int slotId)
        {
            switch (slotId)
            {
                case 0: return new Vector(center.X, center.Y);
                case 1: return new Vector(center.X, center.Y);
                case 2: return new Vector(center.X + SLOT_01.X, center.Y + SLOT_01.Y);
                case 3: return new Vector(center.X + SLOT_02.X, center.Y + SLOT_02.Y);
                case 4: return new Vector(center.X + SLOT_03.X, center.Y + SLOT_03.Y);
                case 5: return new Vector(center.X + SLOT_04.X, center.Y + SLOT_04.Y);
                case 6: return new Vector(center.X + SLOT_05.X, center.Y + SLOT_05.Y);
                case 7: return new Vector(center.X + SLOT_06.X, center.Y + SLOT_06.Y);
                case 8: return new Vector(center.X + SLOT_07.X, center.Y + SLOT_07.Y);
                case 9: return new Vector(center.X + SLOT_08.X, center.Y + SLOT_08.Y);
                default:
                    return null;
            }
        }

        private void FinishInstallation()
        {
            InstallationActive = false;
            //Owner.Equipment.ModuleEquipping = false;
        }

        public int GetEmergencyRepairSeconds()
        {
            var totalSeconds = (int)(EmergencyRepairEnd - EmergencyRepairStart).TotalSeconds;
            if (EmergencyRepairActive && totalSeconds > 0)
                return totalSeconds;
            return 0;
        }

        public int GetEmergencyRepairSecondsLeft()
        {
            var secs = (int)(InstallationEnd - DateTime.Now).TotalSeconds;

            if (EmergencyRepairActive && secs > 0)
                return secs;
            return 0;
        }

        public int GetInstallationSeconds()
        {
            var totalSeconds = (int)(InstallationEnd - InstallationStart).TotalSeconds;
            if (InstallationActive && totalSeconds > 0)
                return totalSeconds;
            return 0;
        }

        public int GetInstallationSecondsLeft()
        {
            var secs = (int)(InstallationEnd - DateTime.Now).TotalSeconds;

            if (InstallationActive && secs > 0)
                return secs;
            return 0;
        }

        public void ReturnToOwner()
        {
            //var module = Owner?.Equipment.Modules.Values.FirstOrDefault(x => x.BattleStationModule == this);
            //if (module != null)
            //{
            //    module.EquippedBattleStationId = -1;
            //    module.Equipped = false;
            //}
        }

        public override void OnDestroyed()
        {
            DesignId = 0;
            foreach (var spacemapValue in Spacemap.Entities.Values.Where(x => x is Player))
            {
                var player = (Player)spacemapValue;
                var session = player.GetGameSession();
                if (session != null)
                    Packet.Builder.AssetInfoCommand(session, this);
            }
        }
    }
}
