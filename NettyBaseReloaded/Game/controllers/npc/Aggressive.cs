using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Aggressive : INpc
    {
        private NpcController Controller { get; set; }

        public Aggressive(NpcController controller)
        {
            Controller = controller;
            Controller.Checkers.VisibilityRange = -1;
        }

        public void Tick()
        {
            Controller.Attack.Attacking = true;
            if (Controller.Character.CurrentHealth < 0.05 * Controller.Character.MaxHealth)
                Inactive();
            else Active();
        }

        private Vector SelectedDestination;
        public void Active()
        {
            var npc = Controller.Npc;
            if (npc.Selected == null || Controller.Attack.MainAttacker != null && Controller.Attack.MainAttacker != npc.Selected || npc.Selected.EntityState == EntityStates.DEAD)
            {
                if (Controller.Attack.GetActiveAttackers().Count == 0)
                {
                    var firstOrDefalt = npc.Spacemap.Entities.Values.FirstOrDefault(x => x is Player);
                    npc.Selected = firstOrDefalt;
                }
                else npc.Selected = Controller.Attack.MainAttacker;
            }
            else
            {
                if (SelectedDestination != null && Vector.IsPositionInCircle(SelectedDestination, MovementController.ActualPosition(npc.SelectedCharacter), 350))
                    MovementController.Move(npc, SelectedDestination);
                else
                {
                    if (SelectedDestination == null || !Vector.IsPositionInCircle(SelectedDestination, npc.Selected.Position, 350))
                        SelectedDestination = Vector.GetPosOnCircle(npc.Selected.Position, 350);
                }
            }

            // Circle & attack the player
        }

        public void Inactive()
        {
            var getCloserPos = Controller.Npc.Position.GetCloserVector(new Vector(0, 0), new Vector(20800, 12800));
            MovementController.Move(Controller.Npc, getCloserPos);
        }

        public void Paused()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            // No more players on the map -> Self destroy
        }
    }
}
