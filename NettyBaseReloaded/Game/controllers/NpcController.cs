using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.npc;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.npcs;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers
{
    class NpcController : AbstractCharacterController
    {
        public Npc Npc { get; set; }

        private INpc CurrentNpc { get; set; }

        private DateTime RespawnTimer;

        public NpcController(Character character) : base(character)
        {
            Npc = (Npc) character;
        }

        public AILevels CustomSetAI = AILevels.NULL;

        private object ThreadLock = new object();

        public override void Initiate()
        {

            AILevels ai;
            if (CustomSetAI == AILevels.NULL)
                ai = (AILevels) Npc.Hangar.Ship.AI;
            else ai = CustomSetAI;
            switch (ai)
            {
                case AILevels.PASSIVE:
                case AILevels.AGGRESSIVE:
                    //if (Npc is Barracuda)
                    //{
                    //    CurrentNpc = new Kamikaze(this);
                    //}
                    //else CurrentNpc = new Basic(this);
                    CurrentNpc = new Basic(this);
                    break;
                case AILevels.GALAXY_GATES:
                case AILevels.INVASION:
                    CurrentNpc = new Aggressive(this);
                    Npc.IsRegenerating = false;
                    break;
                case AILevels.MOTHERSHIP:
                    CurrentNpc = new Mothership(this, World.StorageManager.Ships[81]);
                    break;
                case AILevels.DAUGHTER:
                    CurrentNpc = new Daughter(this);
                    break;
                case AILevels.SPACEBALL:
                    CurrentNpc = new SpaceballAI(this);
                    break;
            }

            base.Initiate();
            StopController = false;
            Active = true;

            MovementController.Move(Npc, MovementController.ActualPosition(Npc));
            Checkers.Start();
            if (Npc is EventNpc eventNpc) eventNpc.Announce();
        }


        public new void Tick()
        {
            CurrentNpc.Tick();            
        }

        private bool Restarting = false;
        public void DelayedRestart()
        {
            var tickId = 0;
            TickId = tickId;
            Restarting = true;
            Task.Delay(Npc.RespawnTime * 1000).ContinueWith(x =>
            {
                if (!Restarting) return;
                Restarting = false;
                Restart();
            });
        }

        public void Restart()
        {
            lock (ThreadLock)
            {
                Restarting = false;
                StopController = false;
                if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                    Character.Spacemap.AddEntity(Character);
                var id = 0;
                Global.TickManager.Add(Character, out id);
                Character.SetTickId(id);
                Initiate();
            }
        }

        public Type GetAI() => CurrentNpc.GetType();

        public void Enslave(Character ownedByCharacter, out Slave slave)
        {
            slave = new Slave(this, ownedByCharacter);
            CurrentNpc = slave;
        }

        public void ExitAI()
        {
            lock (ThreadLock)
            {
                CurrentNpc.Exit();
            }
        }
    }
}
