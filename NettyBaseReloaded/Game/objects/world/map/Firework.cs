using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.map
{
    class Firework : Object
    {
        public Player Owner;

        public string Hash;

        public int FireworkType;

        public DateTime TimePlaced;

        public Firework(int id, string hash, int fireworkType, Vector pos, Spacemap map, Player owner) : base(id, pos, map)
        {
            Hash = hash;
            FireworkType = fireworkType;
            Owner = owner;
            TimePlaced = DateTime.Now;
        }

        public override void Tick()
        {
            if (TimePlaced.AddSeconds(10) < DateTime.Now) Detonate();
        }

        public override void execute(Character character)
        {
            
        }

        public void Detonate()
        {
            GameClient.SendToSpacemap(Spacemap, netty.commands.old_client.LegacyModule.write("0|n|FWI|" + Hash));
            GameClient.SendToSpacemap(Spacemap, netty.commands.new_client.LegacyModule.write("0|n|FWI|" + Hash));
            Spacemap.RemoveObject(this);
        }
    }
}
