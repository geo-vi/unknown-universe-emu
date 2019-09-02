using System;
using Server.Game.controllers;
using Server.Game.controllers.characters;
using Server.Game.controllers.server;
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
        /// <param name="attacker">Attacker</param>
        /// <param name="target">Target</param>
        /// <param name="type">Type of attack</param>
        /// <param name="lootId">LootId</param>
        public void CreateCombat(Player attacker, AbstractAttackable target, AttackTypes type, string lootId)
        {
            var amount = 0;
            switch (type)
            {
                case AttackTypes.LASER:
                    var laserCount = GameItemManager.Instance.CountLasers(attacker);
                    if (laserCount == 0)
                    {
                        PrebuiltLegacyCommands.Instance.ServerMessage(attacker, "No lasers equipped on board.");
                        PrebuiltCombatCommands.Instance.AbortAttackCommand(attacker);
                        return;
                    }

                    amount = laserCount;
                    break;
                case AttackTypes.ROCKET:
                    amount = 1;
                    break;
            }
            
            CreateCombat(attacker, target, type, lootId, amount);
        }

        /// <summary>
        /// Creating an attack for every sort of entity
        /// </summary>
        /// <param name="attacker">Attacker</param>
        /// <param name="target">Target</param>
        /// <param name="type">Type</param>
        /// <param name="lootId">Optional</param>
        /// <param name="amount">Optional</param>
        public void CreateCombat(AbstractAttacker attacker, AbstractAttackable target, AttackTypes type, string lootId = "",
            int amount = 0)
        {
            CreateAttackCombat(new PendingAttack(attacker, target, type, lootId, amount));
        }
        
        /// <summary>
        /// Creating a pending attack which is ready to be added into the server
        /// </summary>
        /// <param name="pendingAttack"></param>
        public void CreateAttackCombat(PendingAttack pendingAttack)
        {
            ServerController.Get<AttackController>().CreateCombat(pendingAttack);
        }

        public PendingAttack[] GetActiveCombatsForAttacker(AbstractAttacker attacker)
        {
            return ServerController.Get<AttackController>().GetActiveAttacksByAttacker(attacker);
        }
    }
}