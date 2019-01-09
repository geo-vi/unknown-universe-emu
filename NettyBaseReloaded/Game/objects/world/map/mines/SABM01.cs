using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.map.mines
{
    class SABM01 : Mine
    {
        public override int MineType => 3;

        public SABM01(int id, string hash, Vector pos, Spacemap map) : base(id, hash, pos, map)
        {
        }

        public override void Effect()
        {
            var area = Spacemap.Entities.Where(x => x.Value.Position.DistanceTo(Position) <= 1000 && x.Value is Player);
            foreach (var entry in area)
            {
                var value = (int)(entry.Value.CurrentShield * 0.5);
                entry.Value.CurrentShield = value;
                Packet.Builder.AttackHitCommand(World.StorageManager.GetGameSession(entry.Key), 0, entry.Value, value, (short)Damage.Types.SL);
            }
        }
    }
}
