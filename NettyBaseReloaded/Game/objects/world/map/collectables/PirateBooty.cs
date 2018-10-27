using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.objects.world.map.collectables
{
    class PirateBooty : Collectable
    {
        private Reward BoxReward;

        private bool Respawning { get; }
        public PirateBooty(int id, string hash, Types type, Vector pos, Spacemap map, Vector[] limits, bool respawning) : base(id, hash, type, pos, map, limits)
        {
            Respawning = respawning;
        }

        public void RandomiseReward()
        {
            //todo:
        }

        public override void Dispose()
        {
            base.Dispose();
            if (Respawning)
                Respawn();
        }

        public override void Collect(Character character)
        {
            if (character is Player player)
            {
                Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|STD|fuck a nigga dickason");
            }
        }

        protected override void Reward(Player player)
        {
        }
    }
}
