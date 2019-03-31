using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.npcs;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Daughter : INpc
    {
        private NpcController Controller { get; set; }

        private Vector Center;

        public Daughter(NpcController controller)
        {
            Controller = controller;
            Center = controller.Npc.MotherShip.Position;
        }

        public void Tick()
        {
            PositionChecker();
            if (Controller.Npc.SelectedCharacter == null)
                Inactive();
            else Active();
        }

        public void Active()
        {
            if (!Controller.Npc.SelectedCharacter.InRange(Controller.Npc))
            {
                Controller.Npc.Selected = null;
                return;
            }
            Controller.Attack.Attacking = true;
        }

        public void Inactive()
        {
            if (Controller.Npc.LastCombatTime.AddSeconds(30) < DateTime.Now || (Controller.Npc.MotherShip != null && Controller.Npc.MotherShip.Controller.Active && Controller.Npc.MotherShip.LastCombatTime.AddSeconds(30) < DateTime.Now))
            {
                Exit();
                return;
            }

            Character attacker = null;
            if (Controller.Npc.MotherShip == null || !Controller.Npc.MotherShip.Controller.Attack.GetActiveAttackers().Any())
            {
                var rangePlayers = Controller.Npc.Range.Entities.Where(x => x.Value is Player);
                attacker = rangePlayers.FirstOrDefault().Value;
            }
            else if (Controller.Npc.MotherShip.Controller.Attack.GetActiveAttackers().Any(x => x is Player))
            {
                attacker = Controller.Npc.MotherShip.Controller.Attack.GetActiveAttackers().Where(x => x is Player).OrderBy(x => x.Damage).FirstOrDefault();
            }

            if (!Controller.Attack.TrySelect(attacker))
            {
                Controller.Attack.TrySelect(Controller.Npc.Range.Entities.FirstOrDefault(x =>
                    x.Value is Player && x.Value.Position.DistanceTo(Controller.Npc.Position) < 700).Value);
            }
        }

        public void PositionChecker()
        {
            if (Controller.Npc.MovementStartTime.AddSeconds(1) <= DateTime.Now)
                MovementController.Move(Controller.Npc, Vector.GetPosOnCircle(Center, 550));
        }

        public void Paused()
         {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            Controller.Npc.Invalidate();
            if (Controller.Npc.Spacemap.RemoveEntity(Controller.Npc))
            {
                var cube = Controller.Npc.MotherShip as Cubikon;
                Npc removed;
                cube?.Children.TryRemove(Controller.Npc.Id, out removed);
            }
        }

    }
}
