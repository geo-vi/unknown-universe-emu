using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.zones;
using Object = NettyBaseReloaded.Game.objects.world.map.Object;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Basic : INpc
    {
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
            if (Controller.Attack.MainAttacker != null)
            {
                Controller.Character.Selected = Controller.Attack.MainAttacker;
                Controller.Attack.Attacking = true;
            }
            if (!Controller.Npc.Hangar.Ship.IsNeutral)
                Controller.Attack.Attacking = true;
        }

        private DateTime LastMovedTime = new DateTime();

        public void Inactive()
        {
            try
            {
                if (Controller.Npc.Spacemap.Entities.Count(x => x.Value is Player) == 0)
                {
                    return;
                }

                Controller.Attack.Attacking = false;
                var noAccessObjects =
                    Controller.Npc.Range.Objects.Where(
                        x => x.Value is Asteroid || x.Value is Asset || x.Value is Station || x.Value is Jumpgate);
                var keyValuePairs = noAccessObjects as KeyValuePair<int, Object>[] ?? noAccessObjects.ToArray();
                if (LastMovedTime.AddSeconds(45) <= DateTime.Now || keyValuePairs.Any())
                {
                    newDest:
                    var dest = Vector.Random(Controller.Npc.Spacemap);
                    if (keyValuePairs.Count(x => x.Value?.Position.DistanceTo(dest) < 1000) > 0)
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
                        if (player.Value == null || player.Value.EntityState == EntityStates.DEAD || player.Value.Invisible || !player.Value.Targetable) continue;
                        if (candidatePlayer == null)
                        {
                            var _player = (Player) player.Value;
                            if (_player.Spacemap.Id == Controller.Npc.Spacemap.Id && !_player.State.InDemiZone && _player.Range.Zones.FirstOrDefault(x => x.Value is DemiZone).Value == null)
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
                if (Controller.Character.LastCombatTime.AddMilliseconds(500) > DateTime.Now || !Controller.Npc.Hangar.Ship.IsNeutral &&
                    Controller.Npc.Selected != null)
                    Active();
            }
            catch (Exception e)
            {
                //new ExceptionLog("npc_inactive_crash", "NPC Crashed", e);
            }
        }

        public void Paused()
        {
            var target = Controller.Npc.Selected as Player;
            var npc = Controller.Npc;

            try
            {
                if (Controller.Npc.Range.Entities.Count(x => x.Value is Player) > 1 && Controller.Attack.MainAttacker == null)
                {
                    var players = Controller.Npc.Range.Entities.Where(x => x.Value is Player);
                    foreach (var player in players)
                    {
                        if (player.Value?.Position == null || player.Value.Spacemap == null) continue;
                        if (((Player)player.Value).AttachedNpcs.Count >
                            ((Player)Controller.Npc.Selected).AttachedNpcs.Count &&
                            player.Value.Position.DistanceTo(Controller.Npc.Position) < 500)
                        {
                            Exit();
                            Controller.Npc.Selected = player.Value;
                            ((Player)player.Value).AttachedNpcs.Add(Controller.Npc);
                        }
                    }
                }

                if (target?.Position != null && target.Spacemap != null)
                {
                    if ((target.State.InDemiZone) || npc.Range.Zones.FirstOrDefault(x => x.Value is DemiZone).Value != null && Controller.Attack.GetActiveAttackers().Count == 0|| target.EntityState == EntityStates.DEAD || !target.Targetable)
                    {
                        Exit();
                        return;
                    }

                    if (npc.CurrentHealth < npc.MaxHealth * 0.1)
                    {
                        MovementController.Move(npc, Vector.Random(npc.Spacemap, new Vector(0,0), new Vector(20800, 12800)));
                    }
                    else if (!Vector.IsPositionInCircle(npc.Destination, target.Position, 400))
                        MovementController.Move(npc, Vector.GetPosOnCircle(target.Position, 400));

                }
                
                if (Controller.Character.LastCombatTime.AddMilliseconds(500) > DateTime.Now || !Controller.Npc.Hangar.Ship.IsNeutral &&
                    target != null)
                    Active();
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
            MovementController.Move(Controller.Npc, Vector.Random(Controller.Npc.Spacemap));
        }
    }
}
