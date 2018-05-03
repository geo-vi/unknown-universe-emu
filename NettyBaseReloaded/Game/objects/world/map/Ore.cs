using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
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
            var player = character as Player;
            if (Disposed)
            {
                if (player != null)
                    Packet.Builder.MapEventOreCommand(player.GetGameSession(), this, OreCollection.FAILED_ALREADY_COLLECTED);
                return;
            }
            if (player != null)
            {
                Packet.Builder.MapEventOreCommand(player.GetGameSession(), this, OreCollection.FINISHED);
            }
            Dispose();
        }

        public void Dispose()
        {
            GameClient.SendToSpacemap(Spacemap, netty.commands.old_client.LegacyModule.write("0|q|" + Hash));
            Spacemap.RemoveObject(this);
            Disposed = true;
        }
    }
}
