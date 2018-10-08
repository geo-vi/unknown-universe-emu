using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.npc;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers
{
    class NpcController : AbstractCharacterController
    {
        public Npc Npc { get; set; }

        private INpc CurrentNpc { get; set; }

        public NpcController(Character character) : base(character)
        {
            Npc = (Npc) character;
        }

        public AILevels CustomSetAI = AILevels.NULL;

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
                    CurrentNpc = new Basic(this);
                    break;
                case AILevels.GALAXY_GATES:
                case AILevels.INVASION:
                    CurrentNpc = new Aggressive(this);
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
            Active = true;
            //Npc.Log.Write($"(ID: {Npc.Id}, {DateTime.Now}) Setted AI to {ai}");
            Checkers.Start();
            Global.TickManager.Add(this);
            //Task.Factory.StartNew(ActiveTick);
        }

        public new void Tick()
        {
            CurrentNpc.Tick();            
        }

        private async void ActiveTick()
        {
            while (Active)
            {
                if (Character.EntityState == EntityStates.DEAD || StopController)
                    Active = false;
                else
                {
                    await Task.Factory.StartNew(TickClasses);
                    await Task.Factory.StartNew(CurrentNpc.Tick);
                }
                await Task.Delay(500);
            }
            //Npc.Log.Write($"(ID: {Npc.Id}, {DateTime.Now}) NPC went inactive");
            Sleep();
        }

        private async void Sleep()
        {
            while (!Active)
            {
                if (StopController) return;
                await Task.Delay(5000);
            }
            ActiveTick();
        }

        public void DelayedRestart()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(Npc.RespawnTime * 1000);
                if (StopController) return;
                Restart();
            });
            // TODO
            //RespawnTimer = DateTime.Now.AddSeconds(Npc.RespawnTime);
            //if (!StopController) return;
            //StopController = false;
            //Sleep();
        }

        public void Restart()
        {
            if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.AddEntity(Character);

            if (!StopController) return;
            StopController = false;
            Initiate();
        }

        public Type GetAI() => CurrentNpc.GetType();

        public void Enslave(Character ownedByCharacter, out Slave slave)
        {
            slave = new Slave(this, ownedByCharacter);
            CurrentNpc = slave;
        }

        public void ExitAI()
        {
            CurrentNpc.Exit();
        }
    }
}
