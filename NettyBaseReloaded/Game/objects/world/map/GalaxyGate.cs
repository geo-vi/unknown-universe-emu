using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.jumpgates;

namespace NettyBaseReloaded.Game.objects.world.map
{
    abstract class GalaxyGate
    {
        public int Id { get; set; }

        public Spacemap Spacemap { get; set; }

        public Spacemap VirtualMap => Spacemap.VirtualWorlds[VWID];

        /// <summary>
        /// Virtual World ID
        /// </summary>
        public int VWID { get; set; }

        public int Wave { get; set; }

        public abstract Dictionary<int, Wave> Waves { get; }

        public int WavesLeftTillEnd = 0;

        public List<Player> JoinedPlayers = new List<Player>();
        public List<Player> PendingPlayers = new List<Player>();

        public bool Active { get; set; }

        /// <summary>
        /// Overriden for the GG Location (where portal will be created)
        /// </summary>
        public virtual Vector Location { get; set; }

        public DateTime WaitingPhaseEnd = new DateTime();
        
        protected Player Owner { get; private set; }

        protected GalaxyGate(Spacemap coreMap, int wave)
        {
            Spacemap = coreMap;
            Wave = wave;
        }

        public virtual void Tick()
        {
            if (PendingPlayers.Count != 0)
            {
                for (int i = 0; i < PendingPlayers.Count; i++)
                {
                    var p = PendingPlayers[i];
                    if (!p.Controller.Jumping && p.Spacemap != VirtualMap)
                    {
                        PendingPlayers.RemoveAt(i);
                        continue;
                    }
                    if (p.Spacemap == VirtualMap)
                    {
                        PendingPlayers.RemoveAt(i);
                        JoinedPlayers.Add(p);
                    }
                }
            }
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
                if (!VirtualMap.Entities.Any(pair => pair.Value is Npc))
                    End();
                else NpcChecker();
                CheckOwnerActivity();
            }
            else if (!CountdownInProcess && DateTime.Now > WaitingPhaseEnd && Waves.ContainsKey(Wave))
            {
                if (JoinedPlayers.Count > 0)
                {
                    CountdownEnd = DateTime.Now.AddSeconds(30);
                    CountdownInProcess = true;
                }
            }
            else if (!Waves.ContainsKey(Wave))
                Reward();
        }

        /// <summary>
        /// Simple countdown to get the gate started
        /// </summary>
        #region Countdown
        public bool CountdownInProcess { get; set; }
        public DateTime CountdownEnd { get; set; }

        private int LastCldSent = 0; 
        public void Count()
        {
            if (CountdownEnd < DateTime.Now)
            {
                CountdownInProcess = false;
                Start();
                return;
            }

            var secs = (CountdownEnd - DateTime.Now).Seconds;
            foreach (var player in JoinedPlayers)
            {
                var playerSession = player.GetGameSession();
                if (playerSession != null)
                {
                    //TODO
                    if (LastCldSent != secs)
                    {
                        Packet.Builder.LegacyModule(playerSession, $"0|A|STD|-= [ {secs} ] =-");
                    }
                }
            }
            LastCldSent = secs;
        }
        #endregion

        /// <summary>
        /// All functions here will be implemented in any GalaxyGate
        /// </summary>

        #region Abstracts
        public abstract void Initiate();

        public abstract void Start();

        public abstract void SendWave();

        public abstract void End();
        #endregion

        /// <summary>
        /// If GG gives any reward you'll override this
        /// </summary>
        public virtual void Reward()
        {
            //Override if needed
        }

        #region Core

        public void InitiateVirtualWorld()
        {
            var getVWID = Spacemap.VirtualWorlds.FirstOrDefault(x => x.Key != 0 && x.Value == null);
            VWID = getVWID.Key;
            if (VWID == 0)
            {
                VWID = Spacemap.VirtualWorlds.Count;
                Spacemap.VirtualWorlds.Add(VWID, null);
            }

            Spacemap vwMap;
            Spacemap.Duplicate(Spacemap, out vwMap);
            Spacemap.VirtualWorlds[VWID] = vwMap;
            Initiate();
        }

        protected event EventHandler AlmostNoNpcsLeft;
        private void NpcChecker()
        {
            if (VirtualMap.Entities.Count(x => x.Value is Npc) < 0.15 * Waves[Wave].Npcs.Count)
                AlmostNoNpcsLeft?.Invoke(this, EventArgs.Empty);
        }

        private void CheckOwnerActivity()
        {
            if (Owner == null && JoinedPlayers.Count > 0)
            {
                var player = JoinedPlayers.FirstOrDefault(x => x?.GetGameSession() != null);
                if (player != null)
                    DefineOwner(player);
                return;
            }
            if (Owner == null || JoinedPlayers.Count == 0) return;
            if (Owner.Spacemap != VirtualMap)
            {
                Player player = JoinedPlayers.FirstOrDefault(x => x?.Spacemap == VirtualMap);
                if (player != null)
                    DefineOwner(player);
                else
                {
                    Owner.OwnedGates.Remove(this);
                }
            }
        }

        public void DefineOwner(Player player)
        {
            if (Owner != null)
            {
                if (Owner.OwnedGates.Contains(this))
                    Owner.OwnedGates.Remove(this);
            }
            Owner = player;
            player.OwnedGates.Add(this);
        }

        public void MoveOut()
        {
            foreach (var joinedPlayer in JoinedPlayers)
            {
                Vector targetPos;
                Spacemap targetMap;
                switch (joinedPlayer.FactionId)
                {
                    case Faction.MMO:
                        targetMap = World.StorageManager.Spacemaps[1];
                        targetPos = new Vector(1000, 1000);
                        break;
                    case Faction.EIC:
                        targetMap = World.StorageManager.Spacemaps[5];
                        targetPos = new Vector(19800, 1000);
                        break;
                    case Faction.VRU:
                        targetMap = World.StorageManager.Spacemaps[9];
                        targetPos = new Vector(19800, 11800);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                joinedPlayer.MoveToMap(targetMap, targetPos);
            }
            JoinedPlayers.Clear();
            Owner.OwnedGates.Remove(this);
        }
        #endregion
    }
}
