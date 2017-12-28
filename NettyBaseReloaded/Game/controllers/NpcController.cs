using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private bool Active { get; set; }

        private DateTime RespawnTimer { get; set; }

        public NpcController(Character character) : base(character)
        {
            Npc = (Npc) character;
        }

        public override void Initiate()
        {
            var ai = (AILevels) Npc.Hangar.Ship.AI;
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
            }
            Active = true;
            Npc.Log.Write($"(ID: {Npc.Id}, {DateTime.Now}) Setted AI to {ai}");
            Checkers.Start();
            Task.Factory.StartNew(ActiveTick);
        }

        private async void ActiveTick()
        {
            while (Active)
            {
                if (Dead || StopController)
                    Active = false;
                else
                {
                    TickClasses();
                    CurrentNpc.Tick();
                }
                await Task.Delay(500);
            }
            Npc.Log.Write($"(ID: {Npc.Id}, {DateTime.Now}) NPC went inactive");
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
            // TODO
            //RespawnTimer = DateTime.Now.AddSeconds(Npc.RespawnTime);
            //if (!StopController) return;
            //StopController = false;
            //Sleep();
        }

        public void Restart()
        {
            if (!StopController) return;
            StopController = false;
            Initiate();
        }
    }
}
