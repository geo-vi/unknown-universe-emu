using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects.world.players.settings.new_client_slotbars;

namespace NettyBaseReloaded.Game.objects.world.storages.playerStorages
{
    class CooldownStorage
    {
        #region Constants
        public const short LASER_COOLDOWN = 0;
        public const short ROCKET_COOLDOWN = 1;
        public const short HELLSTORM_COOLDOWN = 2;
        public const short HELLSTORM_ROCKET_COOLDOWN = 3;
        public const short RSB_COOLDOWN = 4;
        public const short MINE_COOLDOWN = 5;
        public const short ISH_COOLDOWN = 6;
        public const short SMB_COOLDOWN = 7;
        public const short EMP_COOLDOWN = 8;
        public const short FIREWORK_COOLDOWN = 9;
        public const short JUMPGATE_COOLDOWN = 10;
        public const short CONFIG_COOLDOWN = 11;
        public const short DRONE_FORMATION_COOLDOWN = 12;
        public const short DCR_ROCKET_COOLDOWN = 13;
        #endregion

        public DateTime LaserCooldownEnd = new DateTime(2016,12,15,0,0,0);
        public DateTime RocketCooldownEnd = new DateTime(2016, 12, 15, 0, 0, 0);
        public DateTime HellstormCooldownEnd = new DateTime(2016, 12, 15, 0,0,0);
        public DateTime HellstormRocketLoading = new DateTime(2016, 12, 15, 0, 0, 0);

        #region Various Cooldowns
        public DateTime RSBCooldownEnd = new DateTime(2016, 12, 15, 0, 0, 0);
        public DateTime MineCooldownEnd = new DateTime(2016, 12, 15, 0, 0, 0);
        public DateTime InstaShieldCooldownEnd = new DateTime(2016, 12, 15,0,0,0);
        public DateTime SmartBombCooldownEnd = new DateTime(2016,12, 15, 0,0,0);
        public DateTime EMPCooldownEnd = new DateTime(2016, 12, 15, 0, 0, 0);
        public DateTime FireworkCooldownEnd = new DateTime(2016, 12,15,0,0,0);
        public DateTime JumpGateEnd = new DateTime(2016, 12, 15, 0,0,0);
        public DateTime ConfigCooldownEnd = new DateTime(2016, 12, 15, 0, 0, 0);
        public DateTime DroneFormationCooldownEnd = new DateTime(2017, 1, 21, 0, 0,0);
        public DateTime DCREffectEnd = new DateTime(2017, 1, 24, 0, 0, 0);
        public DateTime DCRCooldownEnd = new DateTime(2017, 1, 24, 0,0,0);
        #endregion        

        public void Edit(short type, DateTime EndTime)
        {
            switch (type)
            {
                case LASER_COOLDOWN:
                    LaserCooldownEnd = EndTime;
                    break;
                case ROCKET_COOLDOWN:
                    RocketCooldownEnd = EndTime;
                    break;
                case HELLSTORM_COOLDOWN:
                    HellstormCooldownEnd = EndTime;
                    break;
                case HELLSTORM_ROCKET_COOLDOWN:
                    HellstormRocketLoading = EndTime;
                    break;
                case RSB_COOLDOWN:
                    RSBCooldownEnd = EndTime;
                   break;
                case MINE_COOLDOWN:
                    MineCooldownEnd = EndTime;
                    break;
                case ISH_COOLDOWN:
                    InstaShieldCooldownEnd = EndTime;
                    break;
                case SMB_COOLDOWN:
                    SmartBombCooldownEnd = EndTime;
                    break;
                case EMP_COOLDOWN:
                    EMPCooldownEnd = EndTime;
                    break;
                case FIREWORK_COOLDOWN:
                    FireworkCooldownEnd = EndTime;
                    break;
                case JUMPGATE_COOLDOWN:
                    JumpGateEnd = EndTime;
                    break;
                case CONFIG_COOLDOWN:
                    ConfigCooldownEnd = EndTime;
                    break;
                case DCR_ROCKET_COOLDOWN:
                    DCRCooldownEnd = EndTime;
                    break;
            }
        }

        public bool Finished(short type)
        {
            switch (type)
            {
                case LASER_COOLDOWN:
                    if (LaserCooldownEnd < DateTime.Now) return true;
                    break;
                case ROCKET_COOLDOWN:
                    if (RocketCooldownEnd < DateTime.Now) return true;
                    break;
                case HELLSTORM_COOLDOWN:
                    if (HellstormCooldownEnd < DateTime.Now) return true;
                    break;
                case HELLSTORM_ROCKET_COOLDOWN:
                    if (HellstormRocketLoading < DateTime.Now) return true;
                    break;
                case RSB_COOLDOWN:
                    if (RSBCooldownEnd < DateTime.Now) return true;
                    break;
                case MINE_COOLDOWN:
                    if (MineCooldownEnd < DateTime.Now) return true;
                    break;
                case ISH_COOLDOWN:
                    if (InstaShieldCooldownEnd < DateTime.Now) return true;
                    break;
                case SMB_COOLDOWN:
                    if (SmartBombCooldownEnd < DateTime.Now) return true;
                    break;
                case EMP_COOLDOWN:
                    if (EMPCooldownEnd < DateTime.Now) return true;
                    break;
                case FIREWORK_COOLDOWN:
                    if (FireworkCooldownEnd < DateTime.Now) return true;
                    break;
                case JUMPGATE_COOLDOWN:
                    if (JumpGateEnd < DateTime.Now) return true;
                    break;
                case CONFIG_COOLDOWN:
                    if (ConfigCooldownEnd < DateTime.Now) return true;
                    break;
                case DRONE_FORMATION_COOLDOWN:
                    if (DroneFormationCooldownEnd < DateTime.Now) return true;
                    break;
                case DCR_ROCKET_COOLDOWN:
                    if (DCRCooldownEnd < DateTime.Now) return true;
                    break;
            }
            return false;
        }

        public void Start(GameSession gameSession, short type)
        {
            switch (type)
            {
                case ROCKET_COOLDOWN:
                    RocketCooldownEnd = DateTime.Now.AddSeconds(2);
                    //gameSession?.Client.Send(LegacyModule.write("0|A|CLD|ROK|2"));
                    break;
                case RSB_COOLDOWN:
                    RSBCooldownEnd = DateTime.Now.AddSeconds(3);
                    break;
                case MINE_COOLDOWN:
                    break;
                case ISH_COOLDOWN:
                    break;
                case SMB_COOLDOWN:
                    break;
                case EMP_COOLDOWN:
                    break;
                case FIREWORK_COOLDOWN:
                    break;
                case DRONE_FORMATION_COOLDOWN:
                    DroneFormationCooldownEnd = DateTime.Now.AddSeconds(3);
                    //gameSession?.Client.Send(LegacyModule.write("0|A|CLD|DRF|3"));
                    break;
                case DCR_ROCKET_COOLDOWN:
                    DCRCooldownEnd = DateTime.Now.AddSeconds(45);
                    //gameSession?.Client.Send(LegacyModule.write("0|A|CLD|DCR|45"));
                    break;
            }
        }
    }
}
