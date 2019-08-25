using System;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;

namespace Server.Game.managers
{
    class CombatManager
    {
        public static CombatManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CombatManager();
                }

                return _instance;
            }
        }

        private static CombatManager _instance;

        /// <summary>
        /// Creating a combat with player
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="lootId"></param>
        public void CreateCombat(Player attacker, AbstractAttackable target, AttackTypes type, string lootId)
        {
            var laserCount = GameItemManager.Instance.CountLasers(attacker);
            if (laserCount == 0)
            {
                PrebuiltLegacyCommands.Instance.ServerMessage(attacker, "No lasers equipped on board.");
                return;
            }
            
            var pendingAttack = new PendingAttack(attacker, target, type, lootId, laserCount);

            Console.WriteLine("Creating a combat entry: \nFrom:" + attacker.Id + "; To: " +
                              target.Id + " " + type + " " + lootId + "; Amount: " + laserCount);
        }

        /// <summary>
        /// Creating a combat with anything other than player (uses no ammo)
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="lootId"></param>
        public void CreateCombat(AbstractAttacker attacker, AbstractAttackable target, AttackTypes type, string lootId)
        {
            var pendingAttack = new PendingAttack(attacker, target, type, lootId, 0);
        }
    }
}