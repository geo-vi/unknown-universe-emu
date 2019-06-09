using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.jumpgates;
using NettyBaseReloaded.Main.interfaces;

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

        public ConcurrentDictionary<int, Player> JoinedPlayers = new ConcurrentDictionary<int, Player>();
        public ConcurrentDictionary<int, Player> PendingPlayers = new ConcurrentDictionary<int, Player>();

        public bool Active { get; set; }

        public bool Finished { get; set; }

        public bool Rewarded { get; set; }

        /// <summary>
        /// Overriden for the GG Location (where portal will be created)
        /// </summary>
        public virtual Vector Location { get; set; }

        public DateTime WaitingPhaseEnd = new DateTime();

        protected Player Owner { get; private set; }

        private Task GGTask;

        protected GalaxyGate(Spacemap coreMap, int wave)
        {
            Spacemap = coreMap;
            Wave = wave;
        }

        private async Task RunThread()
        {
            while (!Finished && !Rewarded)
            {
                Tick();
                await Task.Delay(100);
            }
        }

        public virtual void Tick()
        {
            try
            {
                if (VirtualMap.Entities.Count == 0) return;

                if (Finished && Rewarded)
                {
                    foreach (var joined in JoinedPlayers)
                        MoveOut(joined.Value);
                    return;
                }
                if (PendingPlayers.Count > 0)
                {
                    foreach (var pendingPlayer in PendingPlayers.Values)
                    {
                        Player removedCharacter = null;
                        if (!pendingPlayer.Controller.Jumping && pendingPlayer.Spacemap != VirtualMap)
                        {
                            PendingPlayers.TryRemove(pendingPlayer.Id, out removedCharacter);
                            continue;
                        }
                        if (pendingPlayer.Spacemap == VirtualMap)
                        {
                            PendingPlayers.TryRemove(pendingPlayer.Id, out removedCharacter);
                            JoinedPlayers.TryAdd(pendingPlayer.Id, pendingPlayer);
                            PlayerJoinMap(pendingPlayer);
                        }
                    }
                }
                if (JoinedPlayers.Count == 0)
                {
                    //TODO
                    CheckOwnerActivity();
                    return;
                }
                if (JoinedPlayers.Count > 0)
                {
                    CheckPlayersActivity();
                }
                if (CountdownInProcess)
                {
                    Count();
                    return;
                }

                if (Active)
                {
                    if (!VirtualMap.Entities.Any(pair => pair.Value is Npc) && WavesLeftTillEnd <= 0)
                        End();
                    else NpcChecker();
                    CheckOwnerActivity();
                }
                else if (!CountdownInProcess && DateTime.Now > WaitingPhaseEnd && Waves.ContainsKey(Wave))
                {
                    if (JoinedPlayers.Count > 0)
                    {
                        CountdownEnd = DateTime.Now.AddSeconds(25);
                        CountdownInProcess = true;
                    }
                }
                if (!Waves.ContainsKey(Wave) && VirtualMap.Entities.Count(x => x.Value is Npc) == 0)
                    Reward();
            }
            catch (Exception e)
            {
                Console.WriteLine("TICK");
                Console.WriteLine(e);
            }
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
            foreach (var player in JoinedPlayers.Values)
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
        public virtual void Initiate()
        {
            GGTask = Task.Factory.StartNew(RunThread);
        }

        public virtual void CreateAssets() { }

        public virtual void PlayerJoinMap(Player joinedPlayer) { }

        public abstract void Start();

        public abstract void SendWave();

        public abstract void End();

        public virtual void RemoveLife() { }

        #endregion

        /// <summary>
        /// If GG gives any reward you'll override this
        /// </summary>
        public virtual void Reward()
        {
            //Override if needed
            Rewarded = true;
            Finished = true;
            GGTask.Dispose();
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
            CreateAssets();
            Initiate();
        }

        protected event EventHandler AlmostNoNpcsLeft;

        private void NpcChecker()
        {
            if (VirtualMap.Entities.Count(x => x.Value is Npc) < 0.15 * Waves[Wave - 1].Npcs.Count)
                AlmostNoNpcsLeft?.Invoke(this, EventArgs.Empty);

        }

        private void CheckPlayersActivity()
        {
            try
            {
                foreach (var joined in JoinedPlayers.Values)
                {
                    Player player = joined;
                    if (joined.Spacemap != VirtualMap)
                    {
                        JoinedPlayers.TryRemove(player.Id, out player);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error here");
                Console.WriteLine(e);
            }
        }

        private void CheckOwnerActivity()
        {
            try
            {
                if (JoinedPlayers.Count > 0)
                {
                    foreach (var joinedPlayer in JoinedPlayers.Values)
                    {
                        Player player = joinedPlayer;
                        if (player.GetGameSession() == null)
                        {
                            JoinedPlayers.TryRemove(joinedPlayer.Id, out player);
                            continue;
                        }

                        if (player == Owner)
                        {
                            if (player.Spacemap != VirtualMap)
                            {
                                if (JoinedPlayers.Count > 1)
                                {
                                    DefineOwner(JoinedPlayers.FirstOrDefault(x => x.Value != player).Value);
                                }
                                else
                                {
                                    Active = false;
                                    Finished = true;
                                    CheckAndRemove(player);
                                    return;
                                }
                            }
                        }

                        if (Owner == null)
                        {
                            if (JoinedPlayers.Count > 0)
                                DefineOwner(JoinedPlayers.FirstOrDefault().Value);
                        }
                    }
                }
                else
                {
                    if (!PendingPlayers.ContainsKey(Owner.Id))
                        CheckAndRemove(Owner);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Owner activity");
                Console.WriteLine(e);
            }
        }

        public void DefineOwner(Player player)
        {
            if (player.OwnedGates.ContainsKey(Id))
            {
                Owner = player;
                return;
            }
            player.CreateGalaxyGate(this);
            Owner = player;
        }

        public void CheckAndRemove(Player player)
        {
            if (player == null) return;
            if (player.OwnedGates.ContainsKey(Id))
            {
                foreach (var entity in VirtualMap.Entities)
                {
                    if (entity.Key == player.Id) continue;
                    if (entity.Value is Npc npc)
                    {
                        npc.Controller.ExitAI();
                        npc.Controller.Destruction.Kill();
                    }
                    VirtualMap.Entities.TryRemove(entity.Key, out _);
                }
                player.OwnedGates.TryRemove(Id, out _);
                RemoveLife();
                MoveOut(player);
                Finished = true;
                GGTask.Dispose();
            }
        }

        public void MoveOut(Player player)
        {
            if (player?.GetGameSession() == null) return;            
            var tuple = player.GetClosestStation(true);
            MovementController.Move(player, MovementController.ActualPosition(player));
            player.VirtualWorldId = 0;
            player.MoveToMap(tuple.Item2, tuple.Item1, 0);
        }

        public void RemoveAssets()
        {
            foreach (var asset in VirtualMap.Objects)
            {
                VirtualMap.RemoveObject(asset.Value);
            }
        }

        #endregion
    }
}