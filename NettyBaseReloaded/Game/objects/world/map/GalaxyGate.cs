using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class GalaxyGate
    {
        public Spacemap VirtualSpacemap { get; set; }

        public int Wave { get; set; }

        public List<Player> JoinedPlayers = new List<Player>();

        public bool Active { get; set; }

        protected GalaxyGate(Spacemap virtualWorld, int wave)
        {
            VirtualSpacemap = virtualWorld;
            Wave = wave;
        }

        public void Tick()
        {
            if (JoinedPlayers.Count == 0)
            {
                //TODO
                return;
            }
            if (CountdownInProcess)
            {
                Count();
                return;
            }

            if (Active)
            {
                if (VirtualSpacemap.Entities.Count(pair => pair.Value is Npc) == 0)
                    End();
            }
        }

        public void CreatePortalForPlayer(Player player)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public void MovePlayer(Player player)
        {
            Start();
        }

        public bool CountdownInProcess { get; set; }
        public DateTime CountdownEnd { get; set; }

        public void Count()
        {
            if (CountdownEnd < DateTime.Now)
            {
                CountdownInProcess = false;
                Start();
                return;
            }

            foreach (var player in JoinedPlayers)
            {
                var playerSession = player.GetGameSession();
                if (playerSession != null)
                {
                    //TODO
                    var secs = (CountdownEnd - DateTime.Now).TotalSeconds;
                }
            }
        }

        public abstract void Start();

        public abstract void SendWave();

        public abstract void End();

        public virtual void Reward()
        {
            //Override if needed
        }
    }
}
