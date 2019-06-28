using System.Collections.Concurrent;
using System.Linq;
using Server.Game.controllers.implementable;
using Server.Game.objects.server;

namespace Server.Game.controllers.server
{
    class AttackController : ServerImplementedController
    {
        public ConcurrentQueue<PendingAttack> LaserAttackQueue = new ConcurrentQueue<PendingAttack>();
        public ConcurrentQueue<PendingAttack> RocketAttackQueue = new ConcurrentQueue<PendingAttack>();
        public ConcurrentQueue<PendingAttack> RocketLauncherAttacks = new ConcurrentQueue<PendingAttack>();

        public override void Tick()
        {
            PendingLaserAttacks();
            PendingRocketAttacks();
            PendingRocketLauncherAttacks();
        }

        private void PendingLaserAttacks()
        {
            while (LaserAttackQueue.Any())
            {
                PendingAttack pendingAttack;
                LaserAttackQueue.TryDequeue(out pendingAttack);

                if (pendingAttack.From == pendingAttack.To) continue;

                switch (pendingAttack.LootId)
                {
                    //todo: add all ammunitions
                }
            }
        }

        private void PendingRocketAttacks()
        {
            while (RocketAttackQueue.Any())
            {
                PendingAttack pendingAttack;
                RocketAttackQueue.TryDequeue(out pendingAttack);

                if (pendingAttack.From == pendingAttack.To) continue;

                switch (pendingAttack.LootId)
                {
                    //todo: add all ammunitions
                }
            }
        }

        private void PendingRocketLauncherAttacks()
        {
            while (RocketLauncherAttacks.Any())
            {
                PendingAttack pendingAttack;
                RocketLauncherAttacks.TryDequeue(out pendingAttack);

                if (pendingAttack.From == pendingAttack.To) continue;

                switch (pendingAttack.LootId)
                {
                    //todo: add all ammunitions
                }
            }
        }
    }
}
