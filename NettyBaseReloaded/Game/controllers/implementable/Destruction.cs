﻿using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.players.killscreen;
using NettyBaseReloaded.Networking;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable.attack;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.statistics;
using System.Diagnostics;

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

        public void Destroy(Character target, DeathType deathType = DeathType.MISC)
        {
            try
            {
                Vector pos = target.Position;
                if (target.CurrentHealth <= 0 && target.EntityState == EntityStates.ALIVE)
                {
                    var mainAttacker = target.Controller.Attack.MainAttacker;
                    var attackers = target.Controller.Attack.Attackers.ToList();

                    target.Controller.Destruction.Kill();
                    if (Character is Player || Character is Pet)
                    {
                        var player = Character as Player;

                        if (Character is Pet pet)
                        {
                            player = pet.GetOwner();
                        }
                        
                        if (player != target)
                        {
                            if (target.FactionId == player.FactionId)//
                            {
                                Reward newReward = new Reward(new Dictionary<RewardType, int>());
                                var rewards = target.Hangar.Ship.Reward;
                                foreach (var reward in rewards.Rewards)
                                {
                                    if (reward is int)
                                    {
                                        newReward.Rewards.Add((int)reward * -1); // * -1
                                    }
                                    else newReward.Rewards.Add(reward);
                                }
                                newReward.ParseRewards(player);
                            }
                            else
                            {
                                if (attackers.Count > 1)
                                {
                                    if (target.Hangar.Ship.Id == 80 || mainAttacker.Group != null)
                                    {
                                        foreach (var attacker in attackers)
                                        {
                                            if (target.Hangar.Ship.Id != 80)
                                                if (attacker.Value.Player.Group != mainAttacker.Group)
                                                    continue; // TODO: Add proper mothership/ cubi                                          

                                            Reward reward = new Reward(new Dictionary<RewardType, int>());
                                            var baseReward = target.Hangar.Ship.Reward;

                                            foreach (var rewardValue in baseReward.Rewards)
                                            {
                                                //TODO: Group reward modes & stuff
                                                var percentageTotalDmgGiven =
                                                    (double)attacker.Value.TotalDamage /
                                                    (target.MaxHealth + target.MaxShield);

                                                if (target.Hangar.Ship.Id != 80 && attacker.Value.Player == mainAttacker)
                                                    percentageTotalDmgGiven = 1;

                                                if (rewardValue is int value)
                                                {
                                                    reward.Rewards.Add(Convert.ToInt32(
                                                        value * percentageTotalDmgGiven));
                                                }
                                                else reward.Rewards.Add(rewardValue);
                                            }
                                            reward.ParseRewards(attacker.Value.Player);
                                        }
                                    }
                                    else 
                                        target.Hangar.Ship.Reward.ParseRewards(mainAttacker);
                                }
                                else
                                {
                                    target.Hangar.Ship.Reward.ParseRewards(player);
                                }
                            }
                            Character.Hangar.AddDronePoint(target.Hangar.Ship);
                            foreach (var eventP in player.EventsPraticipating)
                                eventP.Value.DestroyAttackable(target);
                            player.QuestData.AddKill(target);
                            player.Information.AddKill(target.Hangar.Ship.Id);
                        }
                    }
                    if (target is Npc)
                    {
                        target.Spacemap.CreateShipLoot(pos, target.Hangar.Ship.CargoDrop, Character);
                        target.Controller.Destruction.RespawnAlien();
                    }
                    else if (target is Player)
                    {
                        var pTarget = target as Player;
                        foreach (var eventP in pTarget.EventsPraticipating)
                            eventP.Value.Destroyed();
                        switch (deathType)
                        {
                            case DeathType.MINE:

                                break;
                            case DeathType.BATTLESTATION:

                                break;
                            case DeathType.RADITATION:
                                new Killscreen(Character as Player, null, DeathType.RADITATION);
                                break;
                            default:
                                if (Character is Player)
                                {
                                    new Killscreen(pTarget, Character, DeathType.PLAYER);
                                }
                                else if (Character is Npc)
                                {
                                    new Killscreen(pTarget, Character, DeathType.NPC);
                                }
                                else if (Character is Pet)
                                {
                                    var baddog = (Pet)Character;
                                    var gayowner = baddog.GetOwner();
                                    if (gayowner != null)
                                        new Killscreen(pTarget, gayowner, DeathType.PLAYER);
                                    else new Killscreen(pTarget, baddog, DeathType.MISC);
                                }
                                break;
                        }
                    }
                    else if (target is Pet pet)
                    {
                        pet.Controller?.OnPetDestruction();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed destruction, " + e.Message + " [" + Character.Id + "]");
            }
        }

        public void SystemDestroy()
        {
            Character.Controller.Destruction.Kill();
            if (Character is Player)
                RespawnPlayer();
            if (Character is Npc)
                RespawnAlien();
        }

        public void Kill()
        {
            Controller.StopAll();

            GameClient.SendToPlayerView(Character, ShipDestroyedCommand.write(Character.Id, 1), true);
            GameClient.SendToPlayerView(Character, netty.commands.old_client.ShipDestroyedCommand.write(Character.Id, 1),
                true);
            Remove();

            Character.EntityState = EntityStates.DEAD;
            Character.CurrentHealth = 0;
            Character.CurrentNanoHull = 0;
            Character.CurrentShield = 0;

            if (Character is Player)
            {
                var player = (Player)Character;
                var closestStation = player.GetClosestStation();
                var newPos = closestStation.Item1;
                player.Spacemap = closestStation.Item2;
                player.SetPosition(newPos);
                player.Save();
            }
        }

        public void Remove()
        {
            Character.Controller.StopAll();
            Deselect(Character);
            Character.Selected = null;
            Character.Spacemap.RemoveEntity(Character);
            if (Character is Player player)
            {
                player.Storage.Clean();
            }
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
                    }

                    entity.Value.Selected = null;
                }
            }
        }

        public void RespawnPlayer()
        {
            Character.EntityState = EntityStates.ALIVE;

            var player = (Player) Character;
            var killscreen = Killscreen.Load(player);
            player.CurrentHealth = killscreen.SelectedOption == netty.commands.old_client.KillScreenOptionTypeModule.AT_DEATHLOCATION_REPAIR ? (player.Hangar.Ship.Health / 100) * 10 : 1000; //if its location repair %10 of base ship hp else just 1000 hp

            if (player.Controller == null)
            {
                player.Controller = new PlayerController(Character);
            }
            var (newPos, spacemap) = player.GetClosestStation();

            player.VirtualWorldId = 0;

            player.Spacemap = spacemap;

            Character.SetPosition(newPos);

            if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                Character.Spacemap.AddEntity(Character);

            player.Refresh();

            player.Controller.Setup();
            player.Controller.Initiate();
            player.Save();
        }

        private void RespawnAlien()
        {
            Character.EntityState = EntityStates.ALIVE;

            Vector newPos;

            var npc = (Npc) Character;
            if (!npc.Respawning) return;
            if (npc.MotherShip != null)
            {
                npc.Controller.StopController = true;
                return;
            }

            npc.CurrentHealth = npc.MaxHealth;
            npc.CurrentShield = npc.MaxShield;

            if (npc.RespawnTime == 5)
            {
                newPos = Vector.Random(npc.Spacemap, new Vector(1000, 1000), new Vector(20000, 11800));
                npc.SetPosition(newPos);
            }
            npc.Controller.DelayedRestart();
        }

        public void RevivePet()
        {
            Character.EntityState = EntityStates.ALIVE;
            Character.CurrentHealth = 1000;
        }
    }
}
