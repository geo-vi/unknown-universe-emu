using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class Ore : Object
    {
        public string Hash { get; }

        public OreTypes Type { get; set; }

        public int[] Limits { get; set; }

        public bool Disposed { get; set; }

        public Ore(int id, string hash, OreTypes type, Vector pos, Spacemap map, int[] limits) : base(id, pos, map)
        {
            Hash = hash;
            Type = type;
            Limits = limits;
        }

        public override void execute(Character character)
        {
        }

        public virtual void Collect(Character character)
        {
            if (Disposed) return;
            Dispose();
        }

        public void Dispose()
        {
            GameClient.SendToSpacemap(Spacemap, netty.commands.old_client.LegacyModule.write("0|2|" + Hash));
            Spacemap.RemoveObject(this);
            Disposed = true;
        }
    }
}
