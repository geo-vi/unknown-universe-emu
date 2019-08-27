using Server.Game.netty.commands;
using Server.Game.objects.entities;
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
        }

        public void AbortAttackCommand(Player player)
        {
            if (GetSession(player, out var session))
            {
                
            }
        }

        public void LaserAttackCommand()
        {
            //sent to all range...
        }

        public void DamageCommand(Player player, PendingDamage pendingDamage)
        {
            
        }
    }
}