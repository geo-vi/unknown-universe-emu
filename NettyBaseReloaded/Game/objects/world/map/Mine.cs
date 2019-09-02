using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class Mine : Object
    {
        public string Hash { get; set; }

        public virtual bool PulseActive => false;

        public virtual int MineType => -1;

        public DateTime PlacedTime = DateTime.Now;

        protected Mine(int id, string hash, Vector pos, Spacemap map) : base(id, pos, map)
        {
            Hash = hash;
        }

        public override void execute(Character character)
        {
        }

        public override void Tick()
        {
            if (Position == null) return;
            var potential = Spacemap.Entities.OrderBy(x => x.Value.Position.DistanceTo(Position))
                .FirstOrDefault(x => x.Value.Position.DistanceTo(Position) < 200 && x.Value is Player);
            if (potential.Value is Player)
            {
                Explode();
            }
        }

        public void Explode()
        {
            if (PlacedTime.AddSeconds(1) > DateTime.Now) return;
            GameClient.SendToSpacemap(Spacemap, netty.commands.old_client.LegacyModule.write("0|n|MIN|" + Hash));
            GameClient.SendToSpacemap(Spacemap, netty.commands.new_client.LegacyModule.write("0|n|MIN|" + Hash));
            Effect();
            Spacemap.RemoveObject(this);
        }

        public abstract void Effect();

        public void Respawn()
        {
            Spacemap.AddObject(this);
        }
    }
}
