using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Slave : INpc
    {
        private NpcController Controller { get; set; }

        private Character MasterCharacter { get; set; }

        private bool FleeToMurder = false;

        public Slave(NpcController controller, Character master)
        {
            Controller = controller;
            MasterCharacter = master;
            Controller.Npc.FactionId = master.FactionId;
            Controller.Npc.RefreshPlayersView();
        }

        private bool MasterAttacking => MasterCharacter.Controller.Attack.Attacking;
        private bool MasterAttacked => MasterCharacter.Controller.Attack.GetActiveAttackers().Count > 0;

        public void Tick()
        {
            if (MasterCharacter.EntityState == EntityStates.DEAD || MasterCharacter.Spacemap != Controller.Npc.Spacemap)
            {
                Exit();
                return;
            }
            Active();
            
        }

        public void Active()
        {
            if (CustomPlacement) return;
            if (!FleeToMurder && (!Vector.IsPositionInCircle(Controller.Npc.Position, MasterCharacter.Position, 300) || !Vector.IsPositionInCircle(Controller.Npc.Destination, MasterCharacter.Position, 300)))
            {
                MovementController.Move(Controller.Npc, Vector.GetPosOnCircle(MasterCharacter.Position, 300));
            }

            if (MasterAttacking)
            {
                if (!SelectedIsEnemy())
                    Controller.Npc.Selected = MasterCharacter.Selected;
                if (Controller.Npc.Selected == null) return;
                Controller.Attack.Attacking = true;
                FleeToMurder = true;
                MovementController.Move(Controller.Npc, Vector.GetPosOnCircle(Controller.Npc.Selected.Position, 150));
            }
            else if (MasterAttacked)
            {
                if (!SelectedIsEnemy())
                    Controller.Npc.Selected = MasterCharacter.Controller.Attack.GetActiveAttackers().FirstOrDefault();
                if (Controller.Npc.Selected == null) return;
                FleeToMurder = true;
                MovementController.Move(Controller.Npc, Vector.GetPosOnCircle(Controller.Npc.Selected.Position, 150));
                Controller.Attack.Attacking = true;
            }
            else FleeToMurder = false;
        }

        private bool SelectedIsEnemy()
        {
            var selected = Controller.Npc.SelectedCharacter;
            if (selected != null && selected.FactionId != MasterCharacter.FactionId &&
                MasterCharacter.Controller.Attack.Attackers.ContainsKey(selected.Id) || MasterCharacter.Selected == selected)
                return true;
            return false;
        } 

        public void Inactive()
        {
            throw new NotImplementedException();
        }

        public void Paused()
        {
            throw new NotImplementedException();
        }

        public void Attack(Character character)
        {
            if (character == null) return;
            Controller.Npc.Selected = character;
            Controller.Attack.Attacking = true;
        }

        public bool CustomPlacement = false;
        public void Move(Vector pos)
        {
            MovementController.Move(Controller.Npc, pos);
            CustomPlacement = true;
        }

        public void MoveToMaster()
        {
            CustomPlacement = false;
        }

        public void Exit()
        {
            Controller.Npc.Name = Controller.Npc.Hangar.Ship.Name;
            Controller.Npc.FactionId = Faction.NONE;
            Controller.Destruction.SystemDestroy();
        }

        public void SetName(string s)
        {
            Controller.Npc.Name = s;
            Controller.Npc.RefreshPlayersView();
        }
    }
}
