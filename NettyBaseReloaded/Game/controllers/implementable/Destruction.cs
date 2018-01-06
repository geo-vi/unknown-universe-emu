using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using NettyBaseReloaded.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Destruction : IAbstractCharacter
    {
        public Destruction(AbstractCharacterController controller) : base(controller)
        {
        }

        public override void Tick()
        {
        }

        public override void Stop()
        {
        }

        public void Destroy(Character target)
        {
            try
            {
                Vector pos = target.Position;
                if (target.CurrentHealth <= 0 && !target.Controller.Dead)
                {
                    target.Controller.Destruction.Kill();
                    if (Character is Player)
                    {
                        var player = Character as Player;
                        if (target.FactionId == player.FactionId)
                        {
                            Reward newReward = new Reward(new Dictionary<RewardType, int>());
                            var rewards = target.Hangar.Ship.Reward;
                            foreach (var reward in rewards.Rewards)
                            {
                                if (reward is int)
                                {
                                    newReward.Rewards.Add((int) reward * -1);
                                }
                                else newReward.Rewards.Add(reward);
                            }
                            newReward.ParseRewards(player);
                        }
                        else target.Hangar.Ship.Reward.ParseRewards(player);
                        Character.Hangar.AddDronePoints(target.Hangar.Ship.Id);
                    }
                    if (target is Npc)
                    {
                        target.Spacemap.CreateShipLoot(pos, target.Hangar.Ship.CargoDrop, Character);
                        target.Controller.Destruction.RespawnAlien();
                    }
                    else if (target is Player)
                    {
                        //TEMP UNTIL KILLSCREEN IS ADDED
                        var otherPlayer = target as Player;
                        var gameSession = otherPlayer.GetGameSession();
                        Packet.Builder.KillScreenCommand(gameSession, Character);
                    }
                }
            }
            catch (Exception e)
            {
                new ExceptionLog("destruction", "Destroy", e);
            }
        }

        public void Destroy()
        {
            Character.Controller.Destruction.Kill();
            RespawnPlayer();
        }

        public void Kill()
        {
            Character.CurrentHealth = 0;
            Character.CurrentNanoHull = 0;
            Character.CurrentShield = 0;

            GameClient.SendRangePacket(Character, ShipDestroyedCommand.write(Character.Id, 0), true);
            GameClient.SendRangePacket(Character, netty.commands.old_client.ShipDestroyedCommand.write(Character.Id, 0),
                true);

            Remove();
            Controller.Dead = true;
        }

        public void Remove()
        {
            Deselect(Character);
            Character.Controller.StopAll();
            Character.Spacemap.RemoveEntity(Character);
            if (Character is Player)
            {
                var player = (Player)Character;
                player.Storage.Clean();
            }
            Character.Range.Clear();
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

        public void RespawnPlayer()
        {
            Character.Controller.Dead = false;
            Character.Range.Clear();

            var player = (Player) Character;
            player.CurrentHealth = 1000;

            if (player.Controller == null)
            {
                player.Controller = new PlayerController(Character);
            }
            var closestStation = player.GetClosestStation();
            var newPos = closestStation.Item1;
            player.Spacemap = closestStation.Item2;

            Character.SetPosition(newPos);

            if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.AddEntity(Character);

            player.Refresh();

            player.Controller.Setup();
            player.Controller.Initiate();
        }


        private void RespawnAlien()
        {
            Character.Controller.Dead = false;
            Character.Range.Clear();

            Vector newPos;

            var npc = (Npc) Character;

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

            Character.SetPosition(newPos);

            if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.AddEntity(Character);

            npc.Controller.Restart();
        }
    }
}
