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

        public Types Type { get; set; }

        public bool Disposed { get; set; }
        protected DateTime EstTimeOfDisposal { get; set; }

        public bool Temporary { get; set; }

        public Vector[] Limits;

        public virtual bool PetCanCollect => false;

        public Collectable(int id, string hash, Types type, Vector pos, Spacemap map, Vector[] limits) : base(id, pos, map)
        {
            Hash = hash;
            Type = type;
            Limits = limits;
        }

        public override void Tick()
        {
            if (Temporary && EstTimeOfDisposal < DateTime.Now) Dispose();
        }

        public virtual void Collect(Character character)
        {
            if (Disposed) return;
            Player player = null;
            if (character is Player _player)
            {
                player = _player;
                if (player.Position.DistanceTo(Position) > 200) return;
            }
            else if (character is Pet pet)
            {
                player = pet.GetOwner();
            }

            if (player == null) return;
            if (this is CargoLoot && player.Information.Cargo.Full) return;
            player.QuestData.AddCollection(this);
            Dispose();
            Reward(player);
        }

        protected abstract void Reward(Player player);

        public virtual void Dispose()
        {
            try
            {
                GameClient.SendToSpacemap(Spacemap, netty.commands.new_client.DisposeBoxCommand.write(Hash, true));
                GameClient.SendToSpacemap(Spacemap, netty.commands.old_client.LegacyModule.write("0|2|" + Hash));
                Spacemap.RemoveObject(this);
                Disposed = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        protected void Respawn()
        {
            var newPos = Vector.Random(Spacemap);
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
