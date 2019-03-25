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

        public Vector[] Limits { get; set; }

        public bool Disposed { get; set; }

        public Ore(int id, string hash, OreTypes type, Vector pos, Spacemap map, Vector[] limits) : base(id, pos, map)
        {
            Hash = hash;
            Type = type;
            Limits = limits;
            if (Limits == null)
                Limits = Spacemap.Limits;
        }

        public override void execute(Character character)
        {
        }

        public virtual void Collect(Character character)
        {
            Player player = null;
            if (character is Pet pet)
            {
                player = pet.GetOwner();
            }
            else if (character is Player _player)
            {
                player = _player;
            }
            if (player == null) return;
            if (player.Information.Cargo.Full)
            {
                Packet.Builder.MapEventOreCommand(player.GetGameSession(), this, OreCollection.FAILED_CARGO_FULL);
                return;
            }
            if (Disposed)
            {
                Packet.Builder.MapEventOreCommand(player.GetGameSession(), this, OreCollection.FAILED_ALREADY_COLLECTED);
                return;
            }
            Packet.Builder.MapEventOreCommand(player.GetGameSession(), this, OreCollection.FINISHED);
            player.QuestData.AddResourceCollection(this);
            Dispose();
        }

        public void Dispose()
        {
            GameClient.SendToSpacemap(Spacemap, netty.commands.old_client.LegacyModule.write("0|q|" + Hash));
            Spacemap.RemoveObject(this);
            Disposed = true;
            Respawn();
        }

        private void Respawn()
        {
            var newPos = Vector.Random(Spacemap, Limits[0], Limits[1]);
            Position = newPos;
            Disposed = false;
            Spacemap.AddObject(this);
        }
    }
}
