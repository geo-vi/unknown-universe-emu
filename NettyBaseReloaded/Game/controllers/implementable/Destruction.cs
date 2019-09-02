using NettyBaseReloaded.Game.netty;
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
using System.Threading;
using NettyBaseReloaded.Main;
using Newtonsoft.Json;

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

        private readonly object ThreadLock = new object();
        public void Destroy(Character target, DeathType deathType = DeathType.MISC)
        {
            lock(ThreadLock)
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
                            if (target.FactionId == player.FactionId) //
                            {
                                Reward newReward = new Reward(new Dictionary<RewardType, int>());
                                var rewards = target.Reward;
                                foreach (var reward in rewards.Rewards)
                                {
                                    if (reward is int)
                                    {
                                        newReward.Rewards.Add((int) reward * -1); // * -1
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
                                            var baseReward = target.Reward;

                                            foreach (var rewardValue in baseReward.Rewards)
                                            {
                                                //TODO: Group reward modes & stuff
                                                var percentageTotalDmgGiven =
                                                    (double) attacker.Value.TotalDamage /
                                                    attackers.Sum(x => x.Value.TotalDamage);

                                                if (target.Hangar.Ship.Id != 80 &&
                                                    attacker.Value.Player == mainAttacker)
                                                    percentageTotalDmgGiven = 1;

                                                if (rewardValue is int value)
                                                {
                                                    reward.Rewards.Add(Convert.ToInt32(
                                                        value * percentageTotalDmgGiven));
                                                }
                                                else reward.Rewards.Add(rewardValue);
                                            }

                                            reward.ParseRewards(attacker.Value.Player);
                                            attacker.Value.Player.QuestData.AddKill(target);
                                        }
                                    }
                                    else
                                        target.Reward.ParseRewards(mainAttacker);
                                }
                                else
                                {
                                    target.Reward.ParseRewards(player);
                                }
                            }

                            Character.Hangar.AddDronePoint(target.Hangar.Ship);
                            foreach (var eventP in player.EventsPraticipating)
                                eventP.Value.DestroyAttackable(target);
                            player.QuestData.AddKill(target);
                            player.Information.AddKill(target.Hangar.Ship.Id);

                            var damageDealt = 0;
                            var attackStarted = DateTime.Now;
                            var playerAttacker = attackers.FirstOrDefault(x => x.Value.Player == player).Value;
                            if (playerAttacker != null)
                            {
                                damageDealt = playerAttacker.TotalDamage;
                                attackStarted = playerAttacker.AttackStartTime;
                            }

                            player.Statistics.AddKill(target, damageDealt, attackStarted);
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
                            case DeathType.PLAYER:
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
                                    var baddog = (Pet) Character;
                                    var gayowner = baddog.GetOwner();
                                    if (gayowner != null)
                                        new Killscreen(pTarget, gayowner, DeathType.PLAYER);
                                    else new Killscreen(pTarget, baddog, DeathType.MISC);
                                }

                                break;
                            default:
                                new Killscreen(Character as Player, null, DeathType.MISC);
                                break;
                        }
                    }
                    else if (target is Pet pet)
                    {
                        pet.Controller?.OnPetDestruction();
                    }
                }
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
            lock (ThreadLock)
            {
                if (Character.EntityState == EntityStates.DEAD) return;

                GameClient.SendToPlayerView(Character, ShipDestroyedCommand.write(Character.Id, 0), true);
                GameClient.SendToPlayerView(Character,
                    netty.commands.old_client.ShipDestroyedCommand.write(Character.Id, 0),
                    true);

                Character.EntityState = EntityStates.DEAD;

                Character.Invalidate();

                Character.CurrentHealth = 0;
                Character.CurrentNanoHull = 0;
                Character.CurrentShield = 0;

                if (Character is Player player)
                {
                    var lowerMapRespawn = player.Spacemap.Disabled;
                    var closestStation = player.GetClosestStation(lowerMapRespawn);
                    var newPos = closestStation.Item1;
                    player.MoveToMap(closestStation.Item2, newPos, 0);
                    player.Save();
                }
            }
        }

        public void RespawnPlayer()
        {
            lock (ThreadLock)
            {
                var player = (Player) Character;
                if (player.EntityState == EntityStates.ALIVE)
                {
                    return;
                }

                var killscreen = Killscreen.Load(player);
                if (killscreen == null)
                {
                    player.CurrentHealth = 1000;
                }
                else
                {
                    player.CurrentHealth =
                        killscreen.SelectedOption ==
                        netty.commands.old_client.KillScreenOptionTypeModule.AT_DEATHLOCATION_REPAIR
                            ? (player.Hangar.Ship.Health / 100) * 10
                            : 1000; //if its location repair %10 of base ship hp else just 1000 hp
                }

                Character.EntityState = EntityStates.ALIVE;

                if (player.Controller == null)
                {
                    player.Controller = new PlayerController(Character);
                }

                player.Controller.StopController = false;

                var (newPos, spacemap) = player.GetClosestStation();

                player.VirtualWorldId = 0;

                player.MoveToMap(spacemap, newPos, 0);

                if (!Character.Spacemap.Entities.ContainsKey(Character.Id))
                    Character.Spacemap.AddEntity(Character);


                player.Setup();
                player.Controller.Setup();
                player.Controller.Initiate();
                player.Refresh();
                player.Save();
                player.State.StartLoginProtection();
            }
        }

        private void RespawnAlien()
        {
            lock (ThreadLock)
            {
                if (Character.EntityState == EntityStates.ALIVE)
                    return;

                Vector newPos;

                var npc = (Npc) Character;

                if (!npc.Respawning) return;
                if (npc.MotherShip != null)
                {
                    npc.Controller.StopController = true;
                    return;
                }

                Character.EntityState = EntityStates.ALIVE;
                npc.CurrentHealth = npc.MaxHealth;
                npc.CurrentShield = npc.MaxShield;

                if (npc.RespawnTime == 5)
                {
                    newPos = Vector.Random(npc.Spacemap, new Vector(1000, 1000), new Vector(20000, 11800));
                    npc.SetPosition(newPos);
                }

                npc.Controller.DelayedRestart();
            }
        }

        public void RevivePet()
        {
            lock (ThreadLock)
            {
                Character.EntityState = EntityStates.ALIVE;
                Character.CurrentHealth = 1000;
                Character.Controller.StopController = false;
            }
        }
    }
}
