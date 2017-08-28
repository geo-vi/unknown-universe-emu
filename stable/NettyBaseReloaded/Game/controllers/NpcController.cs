using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers
{
    class NpcController : AbstractCharacterController
    {
        /// <summary>
        /// This is the NPC AI based on state machine method
        /// 
        ///  // AI
        /// - There is Npc Set for setting the NPC inside the AI class that it belongs to
        /// - There is refresh so it will refresh upon every single tick 
        /// The rest of the classes are Active(), Inactive(), Paused(), Exit() which belongs
        /// to the state machine method.
        ///  // Base class
        /// - There is the main function which triggers the GetAI function & after it Initiate function.
        /// The Initiate function is loading the AI (sub classes) and is setting CurrentAI according to what's set in the Database.
        /// The Exit function is there & set to public in case of external exit if something goes wrong or simply just close it upon
        /// emulator close.
        /// </summary>
        
        interface AI
        {
            void Set(Npc npc, AILevels AILevel);
            void Refresh();
            void Active();
            void Inactive();
            void Paused();
            void Exit();
        }

        private AI CurrentAI;

        public NpcController(Character character) : base(character)
        {
        }

        public void Start(AILevels selectedAi)
        {
            GetAI(selectedAi);
            Initiate(selectedAi);
        }

        public void Tick()
        {
            CurrentAI.Refresh();
            Checkers();
        }

        void GetAI(AILevels ai)
        {
            switch (ai)
            {
                case AILevels.PASSIVE:
                case AILevels.AGGRESSIVE:
                    CurrentAI = new BasicAI();
                    break;
                case AILevels.GALAXY_GATES:
                    CurrentAI = new GalaxyGatesAI();
                    break;
                case AILevels.MOTHERSHIP:
                case AILevels.DAUGHTER:
                    CurrentAI = new CubikonAI();
                    break;
                case AILevels.INVASION:
                    CurrentAI = new InvasionAI();
                    break;
            }
        }

        void Initiate(AILevels aiLevel)
        {
            CurrentAI.Set((Npc) Character, aiLevel);
            if (!Global.TickManager.Exists(this))
                Global.TickManager.Tickables.Add(this);

        }

        public void Restart()
        {
            if (!Global.TickManager.Exists(this))
                Global.TickManager.Tickables.Add(this);
        }

        public void Exit(bool full = true)
        {
            if (full)
            {
                if (Global.TickManager.Exists(this))
                    Global.TickManager.Tickables.Remove(this);
            }
            CurrentAI.Exit();
        }

        class BasicAI : AI
        {
            private Npc Npc;
            private AILevels AILevel;

            public void Set(Npc npc, AILevels aiLevel)
            {
                Npc = npc;
                AILevel = aiLevel;
            }

            DateTime LastRefreshTime = new DateTime(2017, 1, 1, 0, 0, 0);
            public void Refresh()
            {
                if (LastRefreshTime.AddMilliseconds(500) > DateTime.Now) return;

                if (Npc.Selected == null) Inactive();
                else Paused();

                LastRefreshTime = DateTime.Now;
            }

            private bool MovingToEntity = false;
            public void Active()
            {
                var target = Npc.Selected;
                if (!Npc.RangeEntities.ContainsKey(target.Id))
                {
                    DeselectPlayer(target as Player);
                    return;
                }

                if (Npc.LastCombatTime.AddSeconds(3) > DateTime.Now)
                {
                    var attacker = Npc.Controller.GetAttacker();
                    if (attacker != null)
                    {
                        target = attacker;
                        Npc.Selected = attacker;
                        Npc.Controller.Attacking = true;
                    }
                }

                if (target.InRange(Npc, 550))
                {
                    if (AILevel == AILevels.AGGRESSIVE && !target.Controller.Dead)
                        Npc.Controller.Attacking = true;
                }
                else
                {
                    MovementController.Move(Npc, Vector.GetPosInCircle(target.Position, 400));
                }
             }

            private DateTime InactiveTimer;
            public void Inactive()
            {
                if (DateTime.Now > InactiveTimer.AddMilliseconds(1000) && Npc.Spacemap.Entities.Count > 0)
                {
                    MovementController.Move(Npc, Vector.Random(0, 20800, 0, 12800));
                    InactiveTimer = DateTime.Now;
                }

                if (Npc.RangeEntities.Count > 0)
                {
                    var players = Npc.RangeEntities.Values.Where(x => x is Player);
                    Player closestPlayer = null;
                    foreach (var character in players)
                    {
                        var player = (Player) character;
                        if (closestPlayer == null) closestPlayer = player;
                        else
                        {
                            closestPlayer = Npc.Position.GetCloserCharacter(player, closestPlayer) as Player;
                        }
                    }
                    SelectPlayer(closestPlayer);
                }
            }

            private void DeselectPlayer(Player player)
            {
                if(player == null) return;

                if (player.AttachedNpcs.Contains(Npc))
                {
                    player.AttachedNpcs.Remove(Npc);
                }
                Npc.Selected = null;
            }

            private void SelectPlayer(Player player)
            {
                if (player == null) return;

                if (player.AttachedNpcs.Contains(Npc)) Npc.Selected = player;
                if (player.AttachedNpcs.Count == 5 || !player.Controller.Targetable) return;
                player.AttachedNpcs.Add(Npc);
                Npc.Selected = player;
            }

            public void Paused()
            {
                var target = Npc.Selected as Player;
                if (target != null && target.AttachedNpcs.Count < 7)
                {
                    Active();
                }
                else
                {
                    Npc.Selected = null;
                }
            }

            public void Exit()
            {
            }
        }

        class GalaxyGatesAI : AI
        {
            public Npc Npc;
            private AILevels AILevel;

            public void Set(Npc npc, AILevels aiLevel)
            {

            }

            public void Refresh()
            {
                
            }

            public void Active()
            {

            }

            public void Inactive()
            {

            }

            public void Paused()
            {

            }

            public void Exit()
            {

            }
        }

        class CubikonAI : AI
        {
            public Npc Npc;
            private AILevels AILevel;

            public void Set(Npc npc, AILevels aiLevel)
            {

            }
            
            public void Refresh()
            {
                
            }

            public void Active()
            {

            }

            public void Inactive()
            {

            }

            public void Paused()
            {

            }

            public void Exit()
            {

            }
        }

        class InvasionAI : AI
        {
            private Npc Npc;
            private AILevels AILevel;

            public void Set(Npc npc, AILevels aiLevel)
            {
                Npc = npc;
                AILevel = aiLevel;
            }

            DateTime LastRefreshTime = new DateTime(2017, 1, 1, 0, 0, 0);
            public void Refresh()
            {
                if (LastRefreshTime.AddMilliseconds(500) > DateTime.Now) return;

                if (Npc.Selected == null) Inactive();
                else Paused();

                LastRefreshTime = DateTime.Now;
            }

            private bool MovingToEntity = false;
            public void Active()
            {
                var target = Npc.Selected;
                if (!World.StorageManager.GameSessions.ContainsKey(target.Id))
                {
                    Npc.Selected = null;
                    return;
                }

                if (Npc.LastCombatTime.AddSeconds(3) > DateTime.Now)
                {
                    var attacker = Npc.Controller.GetAttacker();
                    if (attacker != null)
                    {
                        target = attacker;
                        Npc.Selected = attacker;
                        Npc.Controller.Attacking = true;
                    }
                }

                if (target.InRange(Npc, 550))
                {
                    if (AILevel == AILevels.AGGRESSIVE && !target.Controller.Dead)
                        Npc.Controller.Attacking = true;
                }
                else
                {
                    MovementController.Move(Npc, Vector.GetPosInCircle(target.Position, 500));
                }
            }

            DateTime InactiveTimer = new DateTime();
            public void Inactive()
            {
                if (InactiveTimer.AddMinutes(1) < DateTime.Now)
                {
                    MovementController.Move(Npc, Vector.Random(0, 20800, 0, 12800));
                    InactiveTimer = DateTime.Now;
                }

                if (Npc.RangeEntities.Count > 0)
                {
                    var players = Npc.RangeEntities.Values.Where(x => x is Player);
                    Player closestPlayer = null;
                    foreach (var character in players)
                    {
                        var player = (Player) character;
                        if (closestPlayer == null) closestPlayer = player;
                        else
                        {
                            closestPlayer = Npc.Position.GetCloserCharacter(player, closestPlayer) as Player;
                        }
                    }
                    Npc.Selected = closestPlayer;
                }
            }

            public void Paused()
            {
                var target = Npc.Selected as Player;
                if (target != null && target.AttachedNpcs.Count < 7)
                {
                    Active();
                }
                else
                {
                    Npc.Selected = null;
                }
            }

            public void Exit()
            {
            }
        }

    }
}
