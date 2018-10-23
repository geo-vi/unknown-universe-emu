using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.events
{
    class Spaceball : PlayerEvent
    {
        public npcs.Spaceball BallNpc;

        public Spaceball(Player player, int id) : base(player, id, 500)
        {
            BallNpc = World.StorageManager.Spacemaps[16].Entities.FirstOrDefault(x => x.Value is npcs.Spaceball).Value as npcs.Spaceball;
        }

        public int ScoreMmo => BallNpc.MMOScore;
        public int ScoreEic => BallNpc.EICScore;
        public int ScoreVru => BallNpc.VRUScore;

        public int Owner => (int)BallNpc.LeadingFaction;

        public int Speed => BallNpc.MovingSpeed;

        public override void Start()
        {
            //todo initiate scoreboard
        }

        public override void Tick()
        {
            
        }

        public override void End()
        {
            // todo: hide window spaceball
        }
    }
}
