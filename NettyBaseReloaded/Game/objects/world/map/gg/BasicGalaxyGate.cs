using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.map.gg
{
    abstract class BasicGalaxyGate : GalaxyGate
    {
        public int GGID;

        public bool AtPause;

        protected BasicGalaxyGate(int ggId, Spacemap coreMap, int wave) : base(coreMap, wave)
        {
            GGID = ggId;
        }

        public override void Initiate()
        {
            AtPause = false;
            base.Initiate();
        }

        public override void Start()
        {
            WavesLeftTillEnd = 4;
            Active = true;
            SendWave();
        }

        public override void Tick()
        {
            base.Tick();
            if (LastSentWaveTime.AddSeconds(10) < DateTime.Now && Active) SendWave();
        }

        public override void CreateAssets()
        {
            if (!AtPause) return;
            VirtualMap.CreateExitGate(Owner, this, new Vector(11200, 6400));
            VirtualMap.CreateGalaxyGate(Owner, GGID, new Vector(9300, 6400));
        }

        private DateTime LastSentWaveTime;
        public override void SendWave()
        {
            if (!Waves.ContainsKey(Wave) || WavesLeftTillEnd <= 0)
            {
                return;
            }

            Waves[Wave].Create(VirtualMap, VWID, Owner.Position, 2000);
            Wave++;

            WavesLeftTillEnd--;

            foreach (var joined in JoinedPlayers.Values)
            {
                if (joined.GetGameSession() == null) continue;
                Packet.Builder.LegacyModule(joined.GetGameSession(), "0|A|STD|Wave " + Wave);
            }

            LastSentWaveTime = DateTime.Now;
        }
        
        public override void End()
        {
            foreach (var joined in JoinedPlayers.Values)
            {
                joined.Gates.PushWave(GGID, 4);
                joined.Gates.Save();
            }

            Active = false;
            WaitingPhaseEnd = DateTime.MaxValue;
            AtPause = true;
            CreateAssets();
        }

        public override void RemoveLife()
        {
            Owner.Gates.RemoveLife(GGID);
        }

        public override void PlayerJoinMap(Player joinedPlayer)
        {
            RemoveAssets();
        }
    }
}
