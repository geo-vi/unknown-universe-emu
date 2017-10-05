using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world;
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
                target.Controller.Destruction.Kill();
                if (target is Player)
                {
                    var pTarget = (Player)target;

                    //if (pTarget.UsingNewClient)
                    //{
                    //    var options = new List<netty.commands.new_client.KillScreenOptionModule>
                    //    {
                    //        new KillscreenOption(KillscreenOption.NEAREST_BASE).Object
                    //    };

                    //    World.StorageManager.GetGameSession(target.Id)
                    //        .Client.Send(
                    //            KillScreenPostCommand.write(Character.Name, "", Character.Hangar.Ship.LootId, new DestructionTypeModule(DestructionTypeModule.USER), options).Bytes);
                    //}
                }

                if (Character.Selected != target)
                {
                    target.Controller.Attack.Attacking = false;
                    Character.Selected = null;
                }
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

            //Remove from the spacemap
            if (Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.Entities.Remove(Character.Id);

            Controller.StopAll();

            Respawn();
        }

        public void Remove(Character targetCharacter)
        {
            if (targetCharacter == null)
                return;

            if (targetCharacter.Spacemap.Entities.ContainsKey(targetCharacter.Id))
                targetCharacter.Spacemap.Entities.Remove(targetCharacter.Id);

            if (targetCharacter is Player)
            {
                var player = (Player)targetCharacter;
                foreach (var attached in player.AttachedNpcs)
                {
                    attached.Selected = null;
                }
                player.AttachedNpcs.Clear();
                player.Save();
            }

            targetCharacter.Controller.StopController = true;
        }

        public void Deselect(Character targetCharacter)
        {
            if (targetCharacter == null)
                return;

            foreach (var entity in targetCharacter.Spacemap.Entities.ToList())
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
                    player.Controller = new PlayerController(Character);

                player.Controller.Start();

                var closestStation = player.GetClosestStation();
                newPos = player.Destination = closestStation.Item1;
                player.Spacemap = closestStation.Item2;

                player.Refresh();
                player.Update();
                player.Storage.Clean();
            }

            Character.SetPosition(newPos);

            if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.Entities.Add(Character.Id, Character);

            Character.RangeObjects.Clear();
            Character.RangeEntities.Clear();
            Character.RangeZones.Clear();
            Character.RangeCollectables.Clear();

            MovementController.Move(Character, MovementController.ActualPosition(Character));
        }
    }
}
