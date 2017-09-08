using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.players.settings;

namespace NettyBaseReloaded.Game.objects.world.players.ammo
{
    static class Converter
    {
        public static string AmmoTypeToString(short ammoType)
        {
            switch (ammoType)
            {
                case AmmunitionTypeModule.X1:
                    return Slotbar.Items.LaserIds[0];
                case AmmunitionTypeModule.X2:
                    return Slotbar.Items.LaserIds[1];
                case AmmunitionTypeModule.X3:
                    return Slotbar.Items.LaserIds[2];
                case AmmunitionTypeModule.X4:
                    return Slotbar.Items.LaserIds[3];
                case AmmunitionTypeModule.SAB:
                    return Slotbar.Items.LaserIds[4];
                case AmmunitionTypeModule.CBO:
                    return Slotbar.Items.LaserIds[5];
                case AmmunitionTypeModule.RSB:
                    return Slotbar.Items.LaserIds[6];
                case AmmunitionTypeModule.JOB100:
                    return Slotbar.Items.LaserIds[7];
                case AmmunitionTypeModule.R310:
                    return Slotbar.Items.RocketIds[0];
                case AmmunitionTypeModule.PLT2026:
                    return Slotbar.Items.RocketIds[1];
                case AmmunitionTypeModule.PLT2021:
                    return Slotbar.Items.RocketIds[2];
                case AmmunitionTypeModule.PLT3030:
                    return Slotbar.Items.RocketIds[3];
                case AmmunitionTypeModule.PLASMA:
                    return Slotbar.Items.RocketIds[4];
                case AmmunitionTypeModule.DECELERATION:
                    return Slotbar.Items.RocketIds[5];
                case AmmunitionTypeModule.WIZARD:
                    return Slotbar.Items.RocketIds[6];
                case AmmunitionTypeModule.BDR1211:
                    return Slotbar.Items.RocketIds[7];
                case AmmunitionTypeModule.BDR1212:
                    return Slotbar.Items.RocketIds[8];
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
