using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Destruction : IAbstractCharacter
    {
        public Destruction(AbstractCharacterController controller) : base(controller)
        {
        }

        public override void Tick()
        {
            //throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public void Destroy(Character target)
        {
            if (target.CurrentHealth <= 0 && !target.Controller.Dead)
            {
                if (Character is Player)
                {
                    var player = Character as Player;
                    target.Hangar.Ship.Reward.ParseRewards(player);
                }
                if (target is Player)
                {
                    // TODO: Send killscreen to target
                }
                else target.Spacemap.CreateShipLoot(target.Position, target.Hangar.Ship.CargoDrop, Character);
                target.Controller.Destruction.Kill();
            }
        }

        public void Kill()
        {
            GameClient.SendRangePacket(Character, ShipDestroyedCommand.write(Character.Id, 0), true);
            GameClient.SendRangePacket(Character, netty.commands.old_client.ShipDestroyedCommand.write(Character.Id, 0), true);

            Deselect(Character);

            Controller.Attack.Attacking = false;
            Character.Selected = null;
            Controller.Dead = true;

            Remove(Character);

            if (Character is Player)
            {
                var player = Character as Player;
                player.Pet?.Controller?.Deactivate();
                player.Controller.Exit();
            }

            Controller.StopAll();
            Respawn();
            //new Killscreen(Character as Player);
        }

        public void Remove(Character targetCharacter)
        {
            if (Character is Npc)
            {
                targetCharacter.Controller.StopController = true;
                return;
            }
            targetCharacter.Position = null;
            targetCharacter.Range.Clear();
            if (targetCharacter is Player)
            {
                var player = targetCharacter as Player;
                player.Storage.Clean();
            }
            targetCharacter.Spacemap.RemoveEntity(targetCharacter);
            //targetCharacter.Spacemap = null;
        }

        public void Deselect(Character targetCharacter)
        {
            if (targetCharacter == null)
                return;

            foreach (var entity in targetCharacter.Spacemap.Entities)
            {
                if (entity.Value.Selected != null && entity.Value.Selected == targetCharacter)
                {
                    if (entity.Value.Controller != null)
                    {
                        if (entity.Value.Controller.Attack.Attacking)
                        {
                            entity.Value.Controller.Attack.Attacking = false;
                        }
                    }

                    if (entity.Value is Player)
                    {
                        Packet.Builder.ShipSelectionCommand(World.StorageManager.GetGameSession(entity.Value.Id), null);
                        //World.StorageManager.GetGameSession(entity.Value.Id).Client.Send(Builder.ShipDeselectionCommand());
                    }

                    entity.Value.Selected = null;
                }
            }
        }

        public void Respawn()
        {
            Controller.Dead = false;
            Controller.StopController = false;
            Controller.Attack.Attacking = false;

            Character.Range.Clear();

            Vector newPos = null;

            if (Character is Npc)
            {
                var npc = (Npc)Character;

                if (npc.MotherShip != null)
                {
                    npc.Controller.StopController = true;
                    return;
                }

                npc.CurrentHealth = npc.MaxHealth;
                npc.CurrentShield = npc.MaxShield;

                if (npc.RespawnTime == 0)
                    newPos = Vector.Random(1000, 28000, 1000, 12000);
                else
                {
                    npc.Controller.DelayedRestart();
                    return;
                }

                npc.Controller.Restart();
            }
            else if (Character is Player)
            {
                var player = (Player)Character;
                player.CurrentHealth = 1000;

                if (player.Controller == null)
                {
                    player.Controller = new PlayerController(Character);
                }
                player.Controller.Start();
                player.Controller.Initiate();
                var closestStation = player.GetClosestStation();
                newPos = player.Destination = closestStation.Item1;
                player.Spacemap = closestStation.Item2;
            }

            if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.AddEntity(Character);

            Character.SetPosition(newPos);

            (Character as Player)?.Refresh();
        }
    }
}
