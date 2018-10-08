using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.npcs;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class SpaceballAI : INpc
    {
        private NpcController Controller;

        private Spaceball Npc;

        public SpaceballAI(Spaceball npc, NpcController controller)
        {
            Controller = controller;
            Npc = npc;
        }

        public void Tick()
        {
            if (Npc.Position == new Vector(0, 0) || Npc.Position == new Vector(0,0) || Npc.Position == new Vector(0,0)) Restart();
            else Active();
        }

        public void Active()
        {
            if (Npc.MMOHitDamage > Npc.EICHitDamage && Npc.MMOHitDamage > Npc.VRUHitDamage)
            {
                //speed = 1
                Npc.LeadingFaction = Faction.MMO;
                Npc.MovingSpeed = Npc.MMOHitDamage >= 500000 ? 2 : 1;
            }
            else if (Npc.EICHitDamage > Npc.MMOHitDamage && Npc.EICHitDamage > Npc.VRUHitDamage)
            {
                //speed = 1
                Npc.LeadingFaction = Faction.EIC;
                Npc.MovingSpeed = Npc.EICHitDamage >= 500000 ? 2 : 1;
            }
            else if (Npc.VRUHitDamage > Npc.MMOHitDamage && Npc.VRUHitDamage > Npc.EICHitDamage)
            {
                Npc.LeadingFaction = Faction.VRU;
                Npc.MovingSpeed = Npc.VRUHitDamage >= 500000 ? 2 : 1;
            }
            else
            {
                Npc.MovingSpeed = 0;
            }

            switch (Npc.LeadingFaction)
            {
                case Faction.MMO:
                    // move to left portal
                    break;
                case Faction.EIC:
                    // move to upper portal
                    break;
                case Faction.VRU:
                    // move to lower portal
                    break;
            }
        }

        public void Inactive()
        {
            throw new NotImplementedException();
        }

        public void Paused()
        {
            throw new NotImplementedException();
        }

        public void Restart()
        {
            var position = new Vector(0, 0);
            Npc.SetPosition(position);
            Npc.LeadingFaction = Faction.NONE;
            Npc.EICHitDamage = 0;
            Npc.MMOHitDamage = 0;
            Npc.VRUHitDamage = 0;
            Npc.EICAttackers = new List<int>();
            Npc.MMOAttackers = new List<int>();
            Npc.VRUAttackers = new List<int>();
        }

        public void Exit()
        {
            Controller.StopController = true;
            Controller.Destruction.SystemDestroy();
        }
    }
}
