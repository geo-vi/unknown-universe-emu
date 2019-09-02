using System;
using Server.Game.netty.commands;
using Server.Game.objects.entities;
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
            Packet.Builder.OldCommands.Add(Commands.ATTACK_ABORT_COMMAND, async (client, actionParams) =>
            {
                //await client.Send(commands.old_client.)
            });
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
    }
}