using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.zones;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Basic : INpc
    {
        // TODO: FIX BUG
        /// <summary>
        /// Unknown position (null)
        /// Unknown spacemap (null)
        /// Suspected error in AttachedNpcs
        /// Error in Vector (position = null)
        /// Suspected problem in Range Entities (npc)
        /// </summary>
        private NpcController Controller { get; set; }

        public Basic(NpcController controller)
        {
            Controller = controller;
        }

        public void Tick()
        {
            if (Controller.Npc.Selected == null)
                Inactive();
            else Paused();
        }

        public void Active()
        {
            // TODO: If npc is throwing rockets etc define here
            Controller.Attack.Attacking = true;
        }

        private DateTime LastMovedTime = new DateTime();

        public void Inactive()
        {
            try
            {
                Controller.Attack.Attacking = false;
                if (LastMovedTime.AddSeconds(45) <= DateTime.Now || Controller.Npc.Range.Objects.Count > 0)
                {
                    newDest:
                    var dest = Vector.Random(0, 20800, 0, 12800);
                    if (Controller.Npc.Spacemap.Objects.Count(x => x.Value?.Position.DistanceTo(dest) < 1000) > 0)
                    {
                        goto newDest;
                    }
                    MovementController.Move(Controller.Npc, dest);
                    LastMovedTime = DateTime.Now;
                }
                if (Controller.Npc.Range.Entities.Count(x => x.Value is Player) > 0)
                {
                    var players = Controller.Npc.Range.Entities.Where(x => x.Value is Player);
                    Player candidatePlayer = null;
                    foreach (var player in players)
                    {
                        if (player.Value == null || player.Value.Controller.Dead) continue;
                        if (candidatePlayer == null)
                        {
                            var _player = (Player) player.Value;
                            if (_player.Spacemap.Id == Controller.Npc.Spacemap.Id && !_player.State.InDemiZone)
                                candidatePlayer = _player;
                        }
                        else
                            candidatePlayer =
                                Controller.Npc.Position.GetCloserCharacter(candidatePlayer, player.Value) as Player;
                    }
                    if (candidatePlayer != null)
                    {
                        Controller.Npc.Selected = candidatePlayer;
                        candidatePlayer.AttachedNpcs.Add(Controller.Npc);
                    }
                }
                if (Controller.Attack.Attacked || !Controller.Npc.Hangar.Ship.IsNeutral &&
                    Controller.Npc.Selected != null)
                    Active();
            }
            catch (Exception e)
            {
                new ExceptionLog("npc_inactive_crash", "NPC Crashed", e);
            }
        }

        public void Paused()
        {
            var target = Controller.Npc.Selected as Player;
            var npc = Controller.Npc;

            try
            {
                if (target?.Position != null && target.Spacemap != null)
                {
                    if (target.State.InDemiZone || target.Controller.Dead)
                    {
                        Exit();
                        return;
                    }

                    if (!Vector.IsPositionInCircle(npc.Destination, target.Position, 400))
                        MovementController.Move(npc, Vector.GetPosOnCircle(target.Position, 400));

                }
                if (Controller.Npc.Range.Entities.Count(x => x.Value is Player) > 1)
                {
                    var players = Controller.Npc.Range.Entities.Where(x => x.Value is Player);
                    foreach (var player in players)
                    {
                        if (player.Value?.Position == null || player.Value.Spacemap == null) continue;
                        if (((Player) player.Value).AttachedNpcs.Count >
                            ((Player) Controller.Npc.Selected).AttachedNpcs.Count &&
                            player.Value.Position.DistanceTo(Controller.Npc.Position) < 500)
                        {
                            Exit();
                            Controller.Npc.Selected = player.Value;
                            ((Player) player.Value).AttachedNpcs.Add(Controller.Npc);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //new ExceptionLog("npc_paused_crash", "", e);
            }
        }

        public void Exit()
        {
            var selectedPlayer = (Player)Controller.Npc.Selected;
            selectedPlayer?.AttachedNpcs.Remove(Controller.Npc);
            Controller.Npc.Selected = null;
            MovementController.Move(Controller.Npc, Vector.Random(0, 20800, 0, 12800));
        }
    }
}
