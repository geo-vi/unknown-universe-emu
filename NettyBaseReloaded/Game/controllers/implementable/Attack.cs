using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable.attack;
using NettyBaseReloaded.Game.controllers.player;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;
using NettyBaseReloaded.Game.objects.world.players.extra.techs;
using Newtonsoft.Json;
using NettyBaseReloaded.Game.objects.world.npcs;
using NettyBaseReloaded.Game.objects.world.players.ammo;
using NettyBaseReloaded.Game.objects.world.players.equipment.extras;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Attack : IAbstractCharacter
    {
        public bool Attacking { get; set; }

        public Player MainAttacker { get; set; }

        public ConcurrentDictionary<int, Attacker> Attackers = new ConcurrentDictionary<int, Attacker>();

        public bool Disabled = false;

        public Attack(AbstractCharacterController controller) : base(controller)
        {
        }

        public override void Tick()
        {
            if (Attacking && Character.Selected != null && TrySelect(Character.Selected))
            {
                LaserAttack();
                if (Character is Npc npc)
                {
                    if (npc is EventNpc)
                    {
                        Wizard(npc.SelectedCharacter);
                    }

                    if (npc.RocketLauncher != null && npc.RocketLauncher.ReadyForLaunch)
                    {
                        LaunchRocketLauncher();
                    }

                    if (npc.Hangar.Ship.Id == 113) // saboteur
                    {
                        SlowdownAttack();
                    }
                }
            }
            RefreshAttackers();
        }

        public override void Stop()
        {
            Attacking = false;
            MainAttacker = null;
            Attackers.Clear();
        }

        public List<Character> GetActiveAttackers()
        {
            List<Character> attackers = new List<Character>();
            foreach (var entity in Character.Range.Entities.Values)
            {
                if (entity.EntityState == EntityStates.DEAD) continue;
                if (entity.Selected == Character && entity.Controller.Attack.Attacking)
                {
                    attackers.Add(entity);
                }
            }
            return attackers;
        }

        private DateTime LastLaserAttack = new DateTime();

        private DateTime RSBCooldownEnd = new DateTime();

        public bool TrySelect(IAttackable target)
        {
            if (target != null && target.Targetable && target.EntityState != EntityStates.DEAD && target.Spacemap == Character.Spacemap)
            {
                Controller.Character.Selected = target;
                return true;
            }

            return false;
        }

        public void LaserAttack()
        {
            var enemy = Character.Selected;
            if (!AssembleEnemy(enemy)) return;

            var isRsb = (Character as Player)?.Settings.CurrentAmmo.LootId == "ammunition_laser_rsb-75" || (Character as Pet)?.GetOwner().Settings.CurrentAmmo.LootId == "ammunition_laser_rsb-75";
            if (isRsb)
            {
                if (Character is Player)
                {
                    if (RSBCooldownEnd > DateTime.Now) return;

                    var cld = new RSBCooldown();
                    Character.Cooldowns.Add(cld);
                    cld.Send(((Player) Character).GetGameSession());
                    RSBCooldownEnd = cld.EndTime;
                }
                else
                {
                    var cld = Character.Cooldowns.CooldownDictionary.FirstOrDefault(x => x.Value is RSBCooldown);
                    if (cld.Value != null)
                    {
                        if (cld.Value.EndTime > DateTime.Now)  return;
                    }

                    var newCld = new RSBCooldown();
                    Character.Cooldowns.Add(newCld);
                }
            }
            else
            {
                if (LastLaserAttack.AddMilliseconds(750) > DateTime.Now) return;
                LastLaserAttack = DateTime.Now;
            }

            var damage = Character.Damage;
            var absDamage = 0; //This variable will be used for ammo that absobrs shield too
            var laserColor = 0;

            if (Character is Player)
            {
                var gameSession = World.StorageManager.GetGameSession(Character.Id);
                if (gameSession.Player.Equipment.LaserCount() == 0)
                {
                    Attacking = false; // Will stop attacking if there are no lasers equipped.
                    Packet.Builder.LegacyModule(gameSession, "0|A|STM|no_lasers_on_board");
                    return;
                }

                var shootingAmmo = gameSession.Player.Settings.CurrentAmmo.Shoot();
                if (shootingAmmo == 0)
                {
                    // NOTHING TO SHOOT
                    Packet.Builder.LegacyModule(gameSession, "0|A|STD|No more ammo (todo: find a proper STM message)");
                    var getAmmo = gameSession.Player.Information.Ammunitions.Keys.ToList();
                    if (gameSession.Player.Settings.CurrentAmmo.LootId == getAmmo[0])
                    {
                        Attacking = false;
                        return;
                    }
                    var index = getAmmo.FindIndex(x => x == gameSession.Player.Settings.CurrentAmmo.LootId) - 1;
                    gameSession.Player.Settings.CurrentAmmo =
                        gameSession.Player.Information.Ammunitions[getAmmo[index]];
                    gameSession.Player.Settings.OldClientShipSettingsCommand.selectedLaser = index;
                    Packet.Builder.SendSlotbars(gameSession);
                }
                gameSession.Player.Skylab.ReduceLaserOre(shootingAmmo);

                var laserTypes = gameSession.Player.Equipment.LaserTypes();
                switch (gameSession.Player.Settings.CurrentAmmo.LootId)
                {
                    case "ammunition_laser_lcb-10":
                        if (laserTypes == 3)
                            laserColor = 1;
                        break;
                    case "ammunition_laser_mcb-25":
                        damage *= 2;
                        if (laserTypes == 3)
                            laserColor = 1;
                        break;
                    case "ammunition_laser_mcb-50":
                        damage *= 3;
                        if (laserTypes == 3)
                            laserColor = 2;
                        break;
                    case "ammunition_laser_ucb-100":
                        damage *= 4;
                        laserColor = 3;
                        break;
                    case "ammunition_laser_sab-50":
                        absDamage = damage * 2;
                        damage *= 0;
                        laserColor = 4;
                        break;
                    case "ammunition_laser_cbo-100":
                        absDamage = damage;
                        damage = damage * 2;
                        laserColor = 8;
                        break;
                    case "ammunition_laser_rsb-75":
                        damage *= 6;
                        laserColor = 6;
                        break;
                    case "ammunition_laser_job-100":
                        damage *= 4;
                        laserColor = 9;
                        break;
                }

                if (enemy is Character)
                    UpdateAttacker(enemy as Character, gameSession.Player);
            }
            else if (Character is Pet)
            {
                var pet = (Pet) Character;
                if (pet.GetOwner() == null) return;
                var gameSession = World.StorageManager.GetGameSession(pet.GetOwner().Id);
                
                if (gameSession.Player.Settings.CurrentAmmo.Shoot("pet") == 0)
                {
                    // NOTHING TO SHOOT
                    Packet.Builder.LegacyModule(gameSession,
                        "0|A|STD|No more ammo (todo: find a proper STM message)");
                    var getAmmo = gameSession.Player.Information.Ammunitions.Keys.ToList();
                    if (gameSession.Player.Settings.CurrentAmmo.LootId == getAmmo[0])
                    {
                        Attacking = false;
                        return;
                    }
                    var index = getAmmo.FindIndex(x => x == gameSession.Player.Settings.CurrentAmmo.LootId) - 1;
                    gameSession.Player.Settings.CurrentAmmo =
                        gameSession.Player.Information.Ammunitions[getAmmo[index]];
                    gameSession.Player.Settings.OldClientShipSettingsCommand.selectedLaser = index;
                    Packet.Builder.SendSlotbars(gameSession);
                }

                var laserTypes = gameSession.Player.Equipment.LaserTypes();
                switch (gameSession.Player.Settings.CurrentAmmo.LootId)
                {
                    case "ammunition_laser_lcb-10":
                        break;
                    case "ammunition_laser_mcb-25":
                        damage *= 2;
                        if (laserTypes == 3)
                            laserColor = 1;
                        break;
                    case "ammunition_laser_mcb-50":
                        damage *= 3;
                        if (laserTypes == 3)
                            laserColor = 2;
                        break;
                    case "ammunition_laser_ucb-100":
                        damage *= 4;
                        laserColor = 3;
                        break;
                    case "ammunition_laser_sab-50":
                        absDamage = damage * 2;
                        damage *= 0;
                        laserColor = 4;
                        break;
                    case "ammunition_laser_cbo-100":
                        absDamage = damage;
                        damage = damage * 2;
                        laserColor = 8;
                        break;
                    case "ammunition_laser_rsb-75":
                        damage *= 6;
                        laserColor = 6;
                        break;
                    case "ammunition_laser_job-100":
                        damage *= 4;
                        laserColor = 9;
                        break;
                }
            }

            damage = RandomizeDamage(damage);
            absDamage = RandomizeDamage(absDamage);

            GameClient.SendToPlayerView(Character,
                netty.commands.old_client.AttackLaserRunCommand.write(Character.Id, enemy.Id, laserColor, enemy is Player,
                    Character.Skills.HasFatLasers()), true);
            GameClient.SendToPlayerView(Character,
                netty.commands.new_client.AttackLaserRunCommand.write(Character.Id, enemy.Id, laserColor, enemy is Player,
                    Character.Skills.HasFatLasers()), true);

            Controller.Damage?.Laser(enemy, damage, absDamage);
            //Controller.Damage?.Laser(enemy, absDamage, true);

            if (Character is Player player)
            {
                if (player.State.LoginProtection)
                    player.State.EndLoginProtection();

                if (player.Settings.CurrentAmmo.LootId != "ammunition_laser_sab-50") {
                    if (player.Techs.ContainsKey(Techs.ENERGY_LEECH))
                    {
                        var energyLeech = player.Techs[Techs.ENERGY_LEECH] as EnergyLeech;
                        energyLeech?.ExecuteHeal(damage);
                    }
                }

                if (player.Controller.CPUs.Active.Contains(CPU.Types.AUTO_ROK))
                {
                    LaunchMissle(player.Settings.CurrentRocket.LootId);
                }

                if (player.Controller.CPUs.Active.Contains(CPU.Types.AUTO_ROCKLAUNCHER))
                {
                    LaunchRocketLauncher();
                }
            }
        }

        public void LaunchMissle(string missleId)
        {
            var enemy = Character.Selected;
            if (!AssembleEnemy(enemy) && !(missleId == "ammunition_specialammo_pld-8" || missleId == "ammunition_specialammo_dcr-250" || missleId == "ammunition_specialammo_wiz-x")) return;

            Player player = Character as Player;

            int damage = -1;
            int rocketId = 0;

            switch (missleId)
            {
                case "ammunition_rocket_r-310":
                    rocketId = 1;
                    damage = RandomizeDamage(Character.RocketDamage, (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 0 : 1));
                    break;
                case "ammunition_rocket_plt-2026":
                    rocketId = 2;
                    damage = RandomizeDamage(Character.RocketDamage * 2, (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 0 : 1));
                    break;
                case "ammunition_rocket_plt-2021":
                    rocketId = 3;
                    damage = RandomizeDamage(Character.RocketDamage * 3, (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 0 : 1));
                    break;
                case "ammunition_rocket_plt-3030":
                    rocketId = 4;
                    damage = RandomizeDamage(Character.RocketDamage * 4, (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 0 : 1));
                    break;
                case "ammunition_specialammo_pld-8":
                    rocketId = 5;
                    if (Character.Cooldowns.CooldownDictionary.Any(c => c.Value is PlasmaCooldown)) return;
                    Plasma(enemy as Character);
                    return;
                case "ammunition_specialammo_dcr-250":
                    rocketId = 6;
                    if (Character.Cooldowns.CooldownDictionary.Any(c => c.Value is DecelerationCooldown)) return;
                    Decelerate(enemy as Character);
                    return;
                case "ammunition_specialammo_wiz-x":
                    rocketId = 8;
                    if (Character.Cooldowns.CooldownDictionary.Any(c => c.Value is WizardCooldown)) return;
                    Wizard(enemy as Character);
                    return;
            }

            if (Character.Cooldowns.CooldownDictionary.Any(c => c.Value is RocketCooldown)) return;

            if (player?.Settings.CurrentRocket.Shoot() == 0)
            {
                // NOTHING TO SHOOT
                Packet.Builder.LegacyModule(player.GetGameSession(),
                    "0|A|STD|No more ammo (todo: find a proper STM message)");
                return;
            }

            player?.Skylab.ReduceRocketOre(1);

            double cooldown_time = 2;
            if (player != null && (player.Extras.Any(x => x.Value is RocketTurbo) || player.Information.Premium.Active))
                cooldown_time *= 0.5;

            var cooldown = new RocketCooldown(cooldown_time);

            if (player != null) cooldown.Send(World.StorageManager.GetGameSession(player.Id));
            Character.Cooldowns.Add(cooldown);

            if (player != null && enemy is Character) UpdateAttacker(enemy as Character, player);

            GameClient.SendToPlayerView(Character, netty.commands.old_client.LegacyModule.write("0|v|" + Character.Id + "|" + enemy.Id + "|H|" + rocketId + "|1|" + (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 1 : 0)), true);
            GameClient.SendToPlayerView(Character, netty.commands.new_client.LegacyModule.write("0|v|" + Character.Id + "|" + enemy.Id + "|H|" + rocketId + "|1|" + (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 1 : 0)), true);
            Controller.Damage?.Rocket(enemy, damage, 0);

            if (player != null && player.State.LoginProtection)
            {
                player.State.EndLoginProtection();
            }
        }

        public void LaunchRocketLauncher()
        {
            var enemy = Character.Selected;
            if (Character.RocketLauncher == null || !AssembleEnemy(enemy)) return;

            var player = Character as Player;

            int damage = 0;
            int absDamage = 0;
            int rocketId = 0;
            Damage.Types dmgTypes = Damage.Types.ROCKET;

            var loadedRockets = Character.RocketLauncher.LoadedRockets;

            switch (Character.RocketLauncher.LoadLootId)
            {
                case "ammunition_rocketlauncher_eco-10":
                    damage = RandomizeDamage(2000 * loadedRockets);
                    break;
                case "ammunition_rocketlauncher_hstrm-01":
                    damage = RandomizeDamage(4000 * loadedRockets);
                    break;
                case "ammunition_rocketlauncher_ubr-100":
                    var baseDamage = 4000;
                    if (enemy is Npc) baseDamage = 7500;
                    damage = RandomizeDamage(baseDamage * loadedRockets);
                    break;
                case "ammunition_rocketlauncher_sar-01":
                    absDamage = RandomizeDamage(1200 * loadedRockets);
                    dmgTypes = Damage.Types.SHIELD_ABSORBER_ROCKET_CREDITS;
                    break;
                case "ammunition_rocketlauncher_sar-02":
                    absDamage = RandomizeDamage(5000 * loadedRockets);
                    dmgTypes = Damage.Types.SHIELD_ABSORBER_ROCKET_URIDIUM;
                    break;
            }

            GameClient.SendToPlayerView(Character, netty.commands.old_client.HellstormAttackCommand.write(Character.Id, enemy.Id, damage != 0, loadedRockets, AmmoConverter.ToAmmoType(Character.RocketLauncher.LoadLootId)), true);
            //GameClient.SendToPlayerView(Character, netty.commands.new_client.LegacyModule.write("0|RL|A|" + Character.Id + "|" + enemy.Id + "|" + loadedRockets + "|" + rocketId), true);

            Character.RocketLauncher.Shoot(loadedRockets);

            Controller.Damage?.Rocket(enemy, damage, absDamage, dmgTypes);

            if (player != null && enemy is Character)
            {
                Packet.Builder.HellstormStatusCommand(World.StorageManager.GetGameSession(player.Id));
                UpdateAttacker(enemy as Character, player);
            }

            if (player != null && player.State.LoginProtection)
            {
                player.State.EndLoginProtection();
            }
        }

        private bool AssembleEnemy(IAttackable attacked)
        {
            if (attacked == null || attacked.EntityState == EntityStates.DEAD)
                return false;
            if (Character.Spacemap.Starter && attacked.FactionId == Character.FactionId)
            {
                Attacking = false;
                var player = Character as Player;
                if (player != null)
                    Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|STD|Can't attack members of your own company on starter maps.");
                return false;
            }
            if (attacked.Position.DistanceTo(Character.Position) > Character.AttackRange)
            {
                var pCharacter = Character as Player;
                pCharacter?.SendLogMessage("outofrange");
                return false;
            }
            if (attacked is Player)
            {
                var attackedCharacter = (Player) attacked;
                if (attackedCharacter.State.InDemiZone) return false;
            }
            return true;
        }

        private int RandomizeDamage(int baseDmg, double hitProbability = 0.90)
        {
            var random = RandomInstance.getInstance(this);
            double randNums = random.NextDouble();
            if (hitProbability - randNums < 0)
            {
                return 0;
            }

            var difference = (baseDmg * randNums) * 0.15;
            if (randNums > 0.5)
                return (int)(baseDmg + difference);
            return (int)(baseDmg - difference);
        }

        public void Wizard(Character target)
        {
            if (target == null || Character.Cooldowns.CooldownDictionary.Any(c => c.Value is WizardCooldown)) return;

            var wizCooldown = new WizardCooldown();
            if (Character is Player)
                wizCooldown.Send(World.StorageManager.GetGameSession(Character.Id));
            Character.Cooldowns.Add(wizCooldown);

            var wizEffect = new WizardEffect();
            wizEffect.OnStart(target);                
            target.Cooldowns.Add(wizEffect);

            GameClient.SendToPlayerView(Character, netty.commands.old_client.LegacyModule.write("0|v|" + Character.Id + "|" + target.Id + "|H|" + 8 + "|1|" + (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 1 : 0)), true);
            GameClient.SendToPlayerView(Character, netty.commands.new_client.LegacyModule.write("0|v|" + Character.Id + "|" + target.Id + "|H|" + 8 + "|1|" + (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 1 : 0)), true);

        }

        public void Decelerate(Character target)
        {
            if (!(target is Player))
            {
                return;
            } 

            var decCooldown = new DecelerationCooldown();
            if (Character is Player)
                decCooldown.Send(World.StorageManager.GetGameSession(Character.Id));
            Character.Cooldowns.Add(decCooldown);

            var decEffect = new DecelerationEffect();
            decEffect.OnStart(target);
            target.Cooldowns.Add(decEffect);

            GameClient.SendToPlayerView(Character, netty.commands.old_client.LegacyModule.write("0|v|" + Character.Id + "|" + target.Id + "|H|" + 7 + "|1|" + (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 1 : 0)), true);
            GameClient.SendToPlayerView(Character, netty.commands.new_client.LegacyModule.write("0|v|" + Character.Id + "|" + target.Id + "|H|" + 7 + "|1|" + (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 1 : 0)), true);

        }

        public void Plasma(Character target)
        {
            if (!(target is Player))
            {
                return;
            }

            var cooldown = new PlasmaCooldown();
            if (Character is Player)
                cooldown.Send(World.StorageManager.GetGameSession(Character.Id));
            Character.Cooldowns.Add(new PlasmaCooldown());

            var effect = new PlasmaEffect();
            effect.OnStart(target);
            target.Cooldowns.Add(new PlasmaEffect());

            GameClient.SendToPlayerView(Character, netty.commands.old_client.LegacyModule.write("0|v|" + Character.Id + "|" + target.Id + "|H|" + 6 + "|1|" + (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 1 : 0)), true);
            GameClient.SendToPlayerView(Character, netty.commands.new_client.LegacyModule.write("0|v|" + Character.Id + "|" + target.Id + "|H|" + 6 + "|1|" + (Character is Player && ((Player)Character).Storage.PrecisionTargeterActivated ? 1 : 0)), true);

        }

        public void UpdateAttacker(Character target, Player player)
        {
            if (target == null || player == null) return;
            if (target.Controller.Attack.MainAttacker == null)
            {
                target.Controller.Attack.MainAttacker = player;
            }
            if (!target.Controller.Attack.Attackers.ContainsKey(Character.Id))
            {
                target.Controller.Attack.Attackers.TryAdd(Character.Id, new Attacker(player));
            }
            else
            {
                target.Controller.Attack.Attackers[player.Id].Refresh();
            }
        }

        public void RefreshAttackers()
        {
            foreach (var attacker in Attackers)
            {
                if (attacker.Value?.Player != null && attacker.Value.LastRefresh.AddSeconds(10) > DateTime.Now)
                {
                    //check for fade
                    if (attacker.Value.FadedToGray && MainAttacker == attacker.Value.Player)
                    {
                        Packet.Builder.LegacyModule(attacker.Value.Player.GetGameSession(), $"0|n|USH|{Character.Id}");
                        attacker.Value.FadedToGray = false;
                        // fade back to red.
                    }

                    if (!attacker.Value.FadedToGray && MainAttacker != attacker.Value.Player)
                    {
                        // fade to gray
                        Packet.Builder.LegacyModule(attacker.Value.Player.GetGameSession(),
                            $"0|n|LSH|{Character.Id}|{Character.Id}");
                        attacker.Value.FadedToGray = true;
                    }
                }
                else
                {
                    Attacker removedAttacker;
                    Attackers.TryRemove(attacker.Key, out removedAttacker);
                }
            }
            if (MainAttacker != null)
            {
                if (!Attackers.ContainsKey(MainAttacker.Id))
                {
                    MainAttacker = null;
                }
            }
        }

        public void SlowdownAttack()
        {
            var enemy = Character.SelectedCharacter;
            if (!AssembleEnemy(enemy) || Character.Cooldowns.CooldownDictionary.Any(c => c.Value is SlowdownAttackCooldown)) return;

            GameClient.SendToPlayerView(Character, netty.commands.old_client.LegacyModule.write("0|n|SAB_SHOT|" + Character.Id + "|" + enemy.Id), true);
            GameClient.SendToPlayerView(Character, netty.commands.new_client.LegacyModule.write("0|n|SAB_SHOT|" + Character.Id + "|" + enemy.Id), true);

            Character.Cooldowns.Add(new SlowdownAttackCooldown());

            var decEffect = new DecelerationEffect();
            decEffect.OnStart(enemy);
            enemy.Cooldowns.Add(decEffect);
        }
    }
}
