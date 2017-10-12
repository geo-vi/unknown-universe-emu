﻿using System;
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
        public NpcController(Character character) : base(character)
        {
            Npc = (Npc) character;
        }

        public void Initiate()
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
                    CurrentNpc = new Mothership(this);
                    break;
            }
            Active = true;
            Tick();
        }

        private async void Tick()
        {
            while (Active)
            {
                if (Attacking)
                    LaserAttack();
                if (Dead || StopController)
                    Active = false; 
                else CurrentNpc.Tick();
                await Task.Delay(500);
            }
            Sleep();
        }

        private async void Sleep()
        {
            while (!Active)
            {
                if (StopController) return;
                await Task.Delay(5000);
            }
            Tick();
        }

        public void Restart()
        {
            Active = true;
            if (!StopController) return;
            StopController = false;
            Tick();
        }
    }
}