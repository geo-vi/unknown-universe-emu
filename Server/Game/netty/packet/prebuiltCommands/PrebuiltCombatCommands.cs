using System;
using System.Collections.Generic;
using Server.Game.managers;
using Server.Game.netty.commands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;

namespace Server.Game.netty.packet.prebuiltCommands
{
    class PrebuiltCombatCommands : PrebuiltCommandBase
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static PrebuiltCombatCommands Instance
        {
            get
            {
            
                if (_instance == null)
                {
                    _instance = new PrebuiltCombatCommands();
                }

                return _instance;
            }
        }

        private static PrebuiltCombatCommands _instance;

        /// <summary>
        /// Adding all relevant commands
        /// </summary>
        public override void AddCommands()
        {
            Packet.Builder.OldCommands.Add(Commands.ATTACK_LASER_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 5, out actionParams);
                await client.Send(commands.old_client.AttackLaserRunCommand.write(Convert.ToInt32(actionParams[0]),
                    Convert.ToInt32(actionParams[1]), Convert.ToInt32(actionParams[2]),
                    Convert.ToBoolean(actionParams[3]),
                    Convert.ToBoolean(actionParams[4])).Bytes);
            });
            Packet.Builder.OldCommands.Add(Commands.ATTACK_HIT_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 8, out actionParams);
                await client.Send(commands.old_client.AttackHitCommand.write(new commands.old_client.AttackTypeModule(Convert.ToInt16(actionParams[0])), 
                    Convert.ToInt32(actionParams[1]), Convert.ToInt32(actionParams[2]), Convert.ToInt32(actionParams[3]),
                    Convert.ToInt32(actionParams[4]), Convert.ToInt32(actionParams[5]), 
                    Convert.ToInt32(actionParams[6]), Convert.ToBoolean(actionParams[7])).Bytes);
            });
            Packet.Builder.OldCommands.Add(Commands.ATTACK_MISS_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 3, out actionParams);
                await client.Send(commands.old_client.AttackMissedCommand.write(
                    new commands.old_client.AttackTypeModule(Convert.ToInt16(actionParams[0])),
                    Convert.ToInt32(actionParams[1]), Convert.ToInt32(actionParams[2])).Bytes);
            });

            Packet.Builder.OldCommands.Add(Commands.ATTACK_ROCKET_LAUNCHER_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 5, out actionParams);
                await client.Send(commands.old_client.HellstormAttackCommand.write(Convert.ToInt32(actionParams[0]),
                    Convert.ToInt32(actionParams[1]), Convert.ToBoolean(actionParams[2]),
                    Convert.ToInt32(actionParams[3]),
                    new commands.old_client.AmmunitionTypeModule(Convert.ToInt16(actionParams[4]))).Bytes);
            });
            
            Packet.Builder.OldCommands.Add(Commands.SHIP_DESTROY_COMMAND, async (client, actionParams) =>
            {
                ArgumentFixer(actionParams, 5, out actionParams);
                await client.Send(commands.old_client.ShipDestroyedCommand.write(Convert.ToInt32(actionParams[0]),
                    Convert.ToInt32(actionParams[1])).Bytes);
            });
        }

        public void AbortAttackCommand(Player player)
        {
            if (GetSession(player, out var session))
            {
                
            }
        }

        public void LaserAttackCommand(PendingAttack attack, int laserColor)
        {
            Packet.Builder.BuildToRange(attack.From, Commands.ATTACK_LASER_COMMAND, new object[]
            {
                attack.From.Id, attack.To.Id, laserColor
            }, new object[0]);
        }

        public void DamageCommand(Player player, PendingDamage pendingDamage)
        {
            var attackerId = 0;
            if (pendingDamage.Attacker != null)
            {
                attackerId = pendingDamage.Attacker.Id;
            }

            
            if (GetSession(player, out var session))
            {
                if (pendingDamage.Damage + pendingDamage.AbsorbDamage == 0)
                {
                    PrebuiltLegacyCommands.Instance.ServerMessage(player, "Missed!!!");
                    Packet.Builder.BuildCommand(session.GameClient, Commands.ATTACK_MISS_COMMAND, false, 
                        pendingDamage.AttackType, pendingDamage.Target.Id, 0);
                }
                else Packet.Builder.BuildCommand(session.GameClient, Commands.ATTACK_HIT_COMMAND, false,
                    pendingDamage.AttackType, attackerId, pendingDamage.Target.Id, pendingDamage.Target.CurrentHealth,
                    pendingDamage.Target.CurrentShield, pendingDamage.Target.CurrentNanoHull, pendingDamage.Damage + pendingDamage.AbsorbDamage);
            }
        }

        public void RocketAttack(PendingAttack attack, int rocketColor)
        {
            Packet.Builder.BuildToRange(attack.From, Commands.LEGACY_MODULE, new object[]
            {
                "0|v|" + attack.From.Id + "|" + attack.To.Id + "|H|" + rocketColor + "|1|0"
            }, new object[0]);
        }

        public void RocketLauncherAttack(PendingAttack attack, int rockets)
        {
            Packet.Builder.BuildToRange(attack.From, Commands.ATTACK_ROCKET_LAUNCHER_COMMAND, new object[]
                {
                    attack.From.Id, attack.To.Id, true, rockets, AmmoConvertManager.ToAmmoType(attack.LootId).type
                }, 
                new object[0]);
        }

        public void HealCommand(Player player, PendingHeal pendingHeal)
        {
            var healerId = 0;
            if (pendingHeal.From != null)
            {
                healerId = pendingHeal.From.Id;
            }

            if (GetSession(player, out var session))
            {
                switch (pendingHeal.HealingType)
                {
                    case HealingTypes.HEALTH:
                    case HealingTypes.HEALTH_AREA:
                        Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, "0|A|HL|" + healerId + "|" + pendingHeal.To.Id + "|HPT|" + pendingHeal.To.CurrentHealth + "|" +
                                                                                                     pendingHeal.Amount);
                        break;
                    case HealingTypes.SHIELD:
                    case HealingTypes.SHIELD_AREA:
                        Packet.Builder.BuildLegacyCommand(session.GameClient, player.UsingNewClient, "0|A|HL|" + healerId + "|" + pendingHeal.To.Id + "|SHD|" + pendingHeal.To.CurrentShield + "|" +
                                                                                                     pendingHeal.Amount);
                        break;
                }
            }
        }

        public void DestructionCommand(PendingDestruction pendingDestruction)
        {
            Packet.Builder.BuildToRange(pendingDestruction.Target, Commands.SHIP_DESTROY_COMMAND, new object[]
            {
                pendingDestruction.Target.Id, 1
            }, new object[]
            {
                
            });
        }
    }
}