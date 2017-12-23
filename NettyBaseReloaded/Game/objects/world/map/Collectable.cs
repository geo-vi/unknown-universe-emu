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

        public Vector Position { get; set; }

        public Types Type { get; set; }

        protected bool Disposed { get; set; }

        public Collectable(int id, string hash, Types type, Vector pos) : base(id, pos)
        {
            Hash = hash;
            Type = type;
            Position = pos;
        }

        public virtual void Collect(Player player)
        {
            if (Disposed) return;
            Dispose(player.Spacemap);
            Reward(player);
        }

        protected abstract void Reward(Player player);

        public virtual void Dispose(Spacemap map)
        {
            GameClient.SendToSpacemap(map, netty.commands.new_client.DisposeBoxCommand.write(Hash, true));
            GameClient.SendToSpacemap(map, netty.commands.old_client.LegacyModule.write("0|2|" + Hash));
            map.RemoveObject(this);
            Disposed = true;
        }

        protected void Respawn(Spacemap map)
        {
            var newPos = Vector.Random(1000, 19800, 1000, 11800);
            Position = newPos;
            if (!map.Objects.ContainsKey(Id))
                map.AddObject(this);
            Disposed = false;
        }

        public override void execute(Character character)
        {
        }

        public virtual int GetTypeId(Character character)
        {
            return (int)Type;
        }
    }
}
