using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.npcs;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class SpaceballAI : INpc
    {
        private NpcController Controller;

        private Spaceball Npc;


        private Jumpgate MmoPortal;

        private Jumpgate EicPortal;

        private Jumpgate VruPortal;


        public SpaceballAI(NpcController controller)
        {
            Controller = controller;
            Npc = Controller.Npc as Spaceball;
            LoadPortals();
        }

        public void LoadPortals()
        {
            var map = World.StorageManager.Spacemaps[16];
            MmoPortal = map.Objects.FirstOrDefault(x => x.Value is Jumpgate gate && gate.DestinationMapId == 17).Value as Jumpgate;
            EicPortal = map.Objects.FirstOrDefault(x => x.Value is Jumpgate gate && gate.DestinationMapId == 21).Value as Jumpgate;
            VruPortal = map.Objects.FirstOrDefault(x => x.Value is Jumpgate gate && gate.DestinationMapId == 25).Value as Jumpgate;
        }

        public void Tick()
        {
            if (Npc.Position == MmoPortal.Position)
            {
                ScoreGoal(Faction.MMO, MmoPortal);
            }
            else if (Npc.Position == EicPortal.Position)
            {
                ScoreGoal(Faction.EIC, EicPortal);
            }
            else if (Npc.Position == VruPortal.Position)
            {
                ScoreGoal(Faction.VRU, VruPortal);
            }
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
                Npc.LeadingFaction = Faction.NONE;
            }

            switch (Npc.LeadingFaction)
            {
                case Faction.MMO:
                    if (!Npc.Moving)
                        MovementController.Move(Npc, MmoPortal.Position);
                    break;
                case Faction.EIC:
                    if (!Npc.Moving)
                        MovementController.Move(Npc, EicPortal.Position);
                    break;
                case Faction.VRU:
                    if (!Npc.Moving)
                        MovementController.Move(Npc, VruPortal.Position);
                    break;
                default:
                    if (Npc.Moving)
                        MovementController.Move(Npc, MovementController.ActualPosition(Npc));
                    break;
            }

            if (Npc.LastCombatTime.AddSeconds(3) < DateTime.Now)
            {
                Npc.MMOHitDamage = 0;
                Npc.EICHitDamage = 0;
                Npc.VRUHitDamage = 0;
                Npc.LeadingFaction = Faction.NONE;
                Npc.MovingSpeed = 0;
            }
        }

        public void ScoreGoal(Faction faction, Jumpgate portal)
        {
            Npc.Score(faction, portal);
            Restart();
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
            var position = Vector.GetMiddle(Npc.Spacemap);
            Npc.SetPosition(position);
            Npc.RefreshPlayersView();
            Npc.LeadingFaction = Faction.NONE;
            Npc.EICHitDamage = 0;
            Npc.MMOHitDamage = 0;
            Npc.VRUHitDamage = 0;
        }

        public void Exit()
        {
            Controller.StopController = true;
            Controller.Destruction.SystemDestroy();
        }
    }
}
