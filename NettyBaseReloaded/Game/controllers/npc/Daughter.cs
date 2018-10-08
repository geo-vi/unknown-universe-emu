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
            if (Controller.Npc.MotherShip == null)
            {
                return;
            }

            var attacker = Controller.Npc.MotherShip.Controller.Attack.GetActiveAttackers().OrderBy(x => x.Damage).FirstOrDefault();
            if (attacker == null)
            {
                var rangePlayers = Controller.Npc.Range.Entities.Where(x => x.Value is Player);
                attacker = rangePlayers.FirstOrDefault().Value;
            }

            Controller.Npc.Selected = attacker;
        }

        public void PositionChecker()
        {
            if (Controller.Npc.MovementStartTime.AddSeconds(1) <= DateTime.Now)
                MovementController.Move(Controller.Npc, Vector.GetPosOnCircle(Center, 400));
        }

        public void Paused()
         {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            if (Controller.Npc.Spacemap.RemoveEntity(Controller.Npc))
            {
                Controller.StopAll();
                var cube = Controller.Npc.MotherShip as Cubikon;
                Npc removed;
                cube?.Children.TryRemove(Controller.Npc.Id, out removed);
            }
        }

    }
}
