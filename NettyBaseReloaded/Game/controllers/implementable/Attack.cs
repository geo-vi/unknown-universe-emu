using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Attack : IAbstractCharacter
    {
        public bool Attacking { get; set; }

        public bool Invincible { get; set; }

        public bool Targetable { get; set; }

        public int AttackRange = 700;

        public DateTime LastTimeAttacked = new DateTime();

        public Attack(AbstractCharacterController controller) : base(controller)
        {
            Targetable = true;
            if (controller is NpcController) AttackRange = 500;
        }

        public override void Tick()
        {
            if (Attacking)
                LaserAttack();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        public List<Character> GetAttackers()
        {
            List<Character> attackers = new List<Character>();
            foreach (var entity in Character.Range.Entities.Values)
            {
                if (entity.Controller.Dead) return null;
                if (entity.Selected == Character && entity.Controller.Attack.Attacking)
                {
                    attackers.Add(entity);
                }
            }
            return attackers;
        }

        public void LaserAttack()
        {
            var enemy = Character.Selected;
            if (Controller.Dead || Controller.StopController || enemy == null || !Attacking) return;
            if (!Character.Spacemap.Entities.ContainsKey(enemy.Id))
            {
                Character.Selected = null;
                Attacking = false;
                return;
            }
            if (Character.Spacemap.Starter && enemy.FactionId == Character.FactionId)
            {
                Attacking = false;
                var player = Character as Player;
                if (player != null)
                    Packet.Builder.LegacyModule(player.GetGameSession(), "0|A|STD|Can't attack members of your own company on starter maps.");
                return;
            }

            if (Character.Cooldowns.Exists(cooldown => cooldown is LaserCooldown))
            {
                if ((Character as Player)?.Settings.CurrentAmmo.LootId == "ammunition_laser_rsb-75")
                {
                    if (Character.Cooldowns.Exists(cooldown => cooldown is RSBCooldown)) return;

                    var cld = new RSBCooldown();
                    cld.Send(((Player) Character).GetGameSession());
                    Character.Cooldowns.Add(cld);
                }
                else return;
            }
            var newCooldown = new LaserCooldown();
            Character.Cooldowns.Add(newCooldown);


            if (!Character.InRange(enemy, AttackRange))
            {
                var pCharacter = Character as Player;
                pCharacter?.SendLogMessage("outofrange");
                return;
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

                if (gameSession.Player.Settings.CurrentAmmo.Shoot() == 0)
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

                var pEnemy = enemy as Player;
                if (pEnemy != null)
                {
                    if (pEnemy.State.InDemiZone)
                        return;
                }

                bool isRsb = false;
                var laserTypes = gameSession.Player.Equipment.LaserTypes();
                switch (gameSession.Player.Settings.CurrentAmmo.LootId)
                {
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
                        isRsb = true;
                        break;
                    case "ammunition_laser_job-100":
                        damage *= 4;
                        laserColor = 9;
                        break;
                }



                if (gameSession.Player.Controller.CPUs.Active.Any(x => x == player.CPU.Types.AUTO_ROK))
                {
                    LaunchMissle(gameSession.Player.Settings.CurrentRocket.LootId);
                }


                //if (gameSession.Player.Controller.CPUs.Active.Any(x => x == player.CPU.Types.AUTO_ROCKLAUNCHER))
                //{
                //    var rocketLauncher = Character.RocketLauncher;
                //    if (rocketLauncher?.Launchers != null)
                //    {
                //        //if(rocketLauncher.CurrentLoad != rocketLauncher.GetMaxLoad())
                //        //{
                //        //    rocketLauncher.Reload();
                //        //}
                //        //else
                //        //{
                //        //    LaunchRocketLauncher();
                //        //    rocketLauncher.Reload();
                //        //}
                //    }
                //}


            }
            else if (Character is Pet)
            {
                
            }

            damage = RandomizeDamage(damage);
            GameClient.SendRangePacket(Character,
                netty.commands.old_client.AttackLaserRunCommand.write(Character.Id, enemy.Id, laserColor, false,
                    true), true);
            GameClient.SendRangePacket(Character,
                netty.commands.new_client.AttackLaserRunCommand.write(Character.Id, enemy.Id, laserColor, false,
                    true), true);

            Controller.Damage?.Laser(enemy, damage, false);
            Controller.Damage?.Laser(enemy, absDamage, true);

            enemy.Controller.Attack.LastTimeAttacked = DateTime.Now;
        }

        public void LaunchMissle(string missleId)
        {
            var enemy = Character.Selected;
            if (enemy == null || enemy.Controller.Dead)
                return;

            var player = Character as Player;
            GameSession gameSession = null;
            if (player != null) gameSession = World.StorageManager.GetGameSession(player.Id);
            if (!Character.InRange(enemy, AttackRange))
            {
                if (player != null)
                {
                    Packet.Builder.LegacyModule(gameSession, "0|A|STM|outofrange");
                }
                return;
            }

            int damage = -1;
            int rocketId = 0;

            switch (missleId)
            {
                case "ammunition_rocket_r-310":
                    rocketId = 1;
                    damage = RandomizeDamage(Character.RocketDamage);
                    break;
                case "ammunition_rocket_plt-2026":
                    rocketId = 2;
                    damage = RandomizeDamage(Character.RocketDamage * 2);
                    break;
                case "ammunition_rocket_plt-2021":
                    rocketId = 3;
                    damage = RandomizeDamage(Character.RocketDamage * 3);
                    break;
                case "ammunition_rocket_plt-3030":
                    rocketId = 4;
                    damage = RandomizeDamage(Character.RocketDamage * 4);
                    break;
                case "ammunition_specialammo_pld-8":
                    rocketId = 5;
                    Plasma(enemy);
                    break;
                case "ammunition_specialammo_dcr-250":
                    rocketId = 6;
                    Decelerate(enemy);
                    break;
                case "ammunition_specialammo_wiz-x":
                    rocketId = 8;
                    if (Character.Cooldowns.Exists(c => c is WizardCooldown)) return;
                    Wizard(enemy);
                    break;
            }

            if (Character.Cooldowns.Exists(c => c is RocketCooldown)) return;

            if (player?.Settings.CurrentRocket.Shoot() == 0)
            {
                // NOTHING TO SHOOT
                Packet.Builder.LegacyModule(gameSession,
                    "0|A|STD|No more ammo (todo: find a proper STM message)");
                return;
            }

            RocketCooldown cooldown;
            double cooldown_time = 2;
            if (player != null && (player.Extras.ContainsKey("equipment_extra_cpu_rok-t01") || player.Information.Premium))
                cooldown_time *= 0.5;
            /*
            if (player.Extras.ContainsKey("equipment_extra_cpu_rok-t01"))
                cooldown_time *= 0.5;
            */

            cooldown = new RocketCooldown(cooldown_time);

            if (player != null) cooldown.Send(World.StorageManager.GetGameSession(player.Id));
            Character.Cooldowns.Add(cooldown);

            GameClient.SendRangePacket(Character, netty.commands.old_client.LegacyModule.write("0|v|" + Character.Id + "|" + enemy.Id + "|H|" + rocketId + "|1|1"), true);
            GameClient.SendRangePacket(Character, netty.commands.new_client.LegacyModule.write("0|v|" + Character.Id + "|" + enemy.Id + "|H|" + rocketId + "|1|1"), true);
            Controller.Damage?.Rocket(enemy, damage, false);

            enemy.Controller.Attack.LastTimeAttacked = DateTime.Now;
        }

        public void LaunchRocketLauncher()
        {
            var enemy = Character.Selected;
            if (Character.RocketLauncher == null || enemy == null || enemy.Controller.Dead)
                return;

            var player = Character as Player;
            GameSession gameSession = null;
            if (player != null) gameSession = World.StorageManager.GetGameSession(player.Id);
            if (!Character.InRange(enemy, AttackRange))
            {
                if (player != null)
                {
                        Packet.Builder.LegacyModule(gameSession, "0|A|STM|outofrange");
                    
                }
                return;
            }

            int damage = 0;
            int absDamage = 0;
            int rocketId = 0;
            Damage.Types dmgTypes = Damage.Types.ROCKET;

            switch (Character.RocketLauncher.LoadLootId)
            {
                case "ammunition_rocketlauncher_eco-10":
                    rocketId = 9;
                    damage = RandomizeDamage(2000 * Character.RocketLauncher.CurrentLoad);
                    break;
                case "ammunition_rocketlauncher_hstrm-01":
                    rocketId = 10;
                    damage = RandomizeDamage(2000 * Character.RocketLauncher.CurrentLoad);
                    break;
                case "ammunition_rocketlauncher_ubr-100":
                    rocketId = 11;
                    var baseDamage = 4000;
                    if (enemy is Npc) baseDamage = 7500;
                    damage = RandomizeDamage(baseDamage * Character.RocketLauncher.CurrentLoad);
                    break;
                case "ammunition_rocketlauncher_sar-01":
                    rocketId = 12;
                    absDamage = RandomizeDamage(1200 * Character.RocketLauncher.CurrentLoad);
                    dmgTypes = Damage.Types.SHIELD_ABSORBER_ROCKET_CREDITS;
                    break;
                case "ammunition_rocketlauncher_sar-02":
                    rocketId = 13;
                    absDamage = RandomizeDamage(5000 * Character.RocketLauncher.CurrentLoad);
                    dmgTypes = Damage.Types.SHIELD_ABSORBER_ROCKET_URIDIUM;
                    break;
            }

            if (Character.Cooldowns.Exists(cooldown => cooldown is RocketLauncherCooldown)) return;

            var newCooldown = new RocketLauncherCooldown();
            Character.Cooldowns.Add(newCooldown);

            Character.RocketLauncher.Shoot();

            GameClient.SendRangePacket(Character, netty.commands.old_client.LegacyModule.write("0|RL|A|" + Character.Id + "|" + enemy.Id + "|" + Character.RocketLauncher.CurrentLoad + "|" + rocketId), true);
            GameClient.SendRangePacket(Character, netty.commands.new_client.LegacyModule.write("0|RL|A|" + Character.Id + "|" + enemy.Id + "|" + Character.RocketLauncher.CurrentLoad + "|" + rocketId), true);

            Controller.Damage?.Rocket(enemy, absDamage, true, dmgTypes);
            Controller.Damage?.Rocket(enemy, damage, false, dmgTypes);

            Character.RocketLauncher.CurrentLoad = 0;
            if (player != null) Packet.Builder.HellstormStatusCommand(World.StorageManager.GetGameSession(player.Id));

            enemy.Controller.Attack.LastTimeAttacked = DateTime.Now;
        }

        private int RandomizeDamage(int baseDmg, double missProbability = 1.00)
        {
            var randNums = Random.Next(0, 6);

            if (missProbability < 1.00)
                randNums = Random.Next(0, 7);
            if (missProbability > 1.00 && missProbability < 2.00)
                randNums = Random.Next(0, 4);
            if (missProbability >= 2.00)
                randNums = Random.Next(2, 4);

            switch (randNums)
            {
                case 0:
                    return (int)(baseDmg * 1.10);
                case 1:
                    return (int)(baseDmg * 0.98);
                case 2:
                    return (int)(baseDmg * 1.02);
                case 3:
                    return 0;
                case 4:
                    return (int)(baseDmg * 0.92);
                case 5:
                    return (int)(baseDmg * 0.99);
                default:
                    return baseDmg;
            }
        }

        public void Wizard(Character target)
        {
            if (target == null)
            {
                return;
            }
            var wizCooldown = new WizardCooldown();
            if (Character is Player)
                wizCooldown.Send(World.StorageManager.GetGameSession(Character.Id));
            Character.Cooldowns.Add(new WizardCooldown());

            var wizEffect = new WizardEffect();
            wizEffect.OnStart(target);                
            target.Cooldowns.Add(new WizardEffect());
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
            Character.Cooldowns.Add(new DecelerationCooldown());

            var decEffect = new DecelerationEffect();
            decEffect.OnStart(target);
            target.Cooldowns.Add(new DecelerationEffect());

        }

        public void Plasma(Character target)
        {
            
        }
    }
}
