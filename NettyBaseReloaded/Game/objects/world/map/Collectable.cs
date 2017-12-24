using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class Collectable : Object
    {
        public string Hash { get; set; }

        public Spacemap Spacemap { get; set; }

        public Types Type { get; set; }

        protected bool Disposed { get; set; }
        protected DateTime EstTimeOfDisposal { get; set; }

        public bool Temporary { get; set; }

        public Collectable(int id, string hash, Types type, Vector pos, Spacemap map) : base(id, pos)
        {
            Hash = hash;
            Type = type;
            Spacemap = map;
        }

        public override void Tick()
        {
            if (Temporary && EstTimeOfDisposal < DateTime.Now) Dispose();
        }

        public virtual void Collect(Player player)
        {
            if (Disposed || player.Position.DistanceTo(Position) > 200) return;
            Dispose();
            Reward(player);
        }

        protected abstract void Reward(Player player);

        public virtual void Dispose()
        {
            GameClient.SendToSpacemap(Spacemap, netty.commands.new_client.DisposeBoxCommand.write(Hash, true));
            GameClient.SendToSpacemap(Spacemap, netty.commands.old_client.LegacyModule.write("0|2|" + Hash));
            Spacemap.RemoveObject(this);
            Disposed = true;
        }

        protected void Respawn()
        {
            var newPos = Vector.Random(1000, 19800, 1000, 11800);
            Position = newPos;
            Disposed = false;
            Spacemap.AddObject(this);
        }

        public override void execute(Character character)
        {
        }

        public virtual int GetTypeId(Character character)
        {
            return (int)Type;
        }

        public void DelayedDispose(int ms)
        {
            EstTimeOfDisposal = DateTime.Now.AddMilliseconds(ms);
        }
    }
}
