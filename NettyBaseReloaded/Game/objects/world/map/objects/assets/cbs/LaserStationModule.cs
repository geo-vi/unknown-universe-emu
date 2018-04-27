using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Main.objects;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets.cbs
{
    class LaserStationModule : BattleStationModule
    {
        public LaserStationModule(Player player, Module module, Asteroid asteroid, Module.Types type) : base(player,
            module, asteroid, type)
        {
        }

        public override void Tick()
        {
            CheckRange();
        }

        public Character Target;

        private void CheckRange()
        {
            foreach (var entity in Spacemap.Entities.Values.Where(x => x.Position.DistanceTo(Position) < Range))
            {
                if (entity.Clan != null && entity.Clan.GetRelation(Clan) == (short) Diplomacy.AT_WAR)
                {
                    Shoot(entity);
                }
            }
        }

        private DateTime LastShot = new DateTime();

        private void Shoot(Character entity)
        {
            if (LastShot.AddSeconds(1) > DateTime.Now || Core.EntityState == EntityStates.DEAD) return;

            foreach (var spacemapValue in Spacemap.Entities.Values.Where(x => x is Player))
            {
                var player = (Player) spacemapValue;
                var session = player.GetGameSession();
                if (session != null)
                    Packet.Builder.AttackLaserRunCommand(session, Id, entity.Id, 0, true, true);
            }

            Damage.Entity(entity, DamageLevelCalculator(), Damage.Types.LASER, Id);

            LastShot = DateTime.Now;
        }

        private int DamageLevelCalculator()
        {
            int baseDmg = 1000;
            switch (ModuleType)
            {
                case Module.Types.LASER_MID_RANGE:
                    baseDmg *= 2;
                    break;
                case Module.Types.LASER_LOW_RANGE:
                    baseDmg *= 4;
                    break;
            }

            return baseDmg * UpgradeLevel;
        }
    }
}
