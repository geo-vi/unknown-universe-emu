using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Game.controllers.implementable;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class HealingController : ServerImplementedController
    {
        private readonly ConcurrentQueue<PendingHeal> _pendingHeals = new ConcurrentQueue<PendingHeal>();

        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Healing Controller", LogKeys.SERVER_LOG);
        }
        
        public override void Tick()
        {
            while (!_pendingHeals.IsEmpty)
            {
                _pendingHeals.TryDequeue(out var pendingHeal);

                if (pendingHeal.CalculationType == CalculationTypes.RANDOMISED)
                {
                    Out.QuickLog("Called heal with random calculation", LogKeys.ERROR_LOG);
                    throw new NotImplementedException("Not added health randomisation");
                }

                if (pendingHeal.Amount == 0)
                {
                    //nothing to heal so continue
                    continue;
                }
                
                switch (pendingHeal.HealingType)
                {
                    case HealingTypes.HEALTH:
                        ProcessHealthHeal(pendingHeal);
                        break;
                    case HealingTypes.SHIELD:
                        ProcessShieldHeal(pendingHeal);
                        break;
                    case HealingTypes.HEALTH_AREA:
                        ProcessAreaHealthHeal(pendingHeal);
                        break;
                    case HealingTypes.SHIELD_AREA:
                        ProcessAreaShieldHeal(pendingHeal);
                        break;
                }
            }
        }

        /// <summary>
        /// Processing a basic health heal
        /// </summary>
        /// <param name="pendingHeal"></param>
        private void ProcessHealthHeal(PendingHeal pendingHeal)
        {
            var target = pendingHeal.To;
            
            if (target == null)
            {
                Out.QuickLog("Something went wrong with healing health of target", LogKeys.ERROR_LOG);
                throw new ArgumentNullException(nameof(target),"Something went wrong with healing health of target");
            }

            if (target.IsDead)
            {
                Out.WriteLog("Cannot heal a dead entity", LogKeys.ERROR_LOG, target.Id);
                throw new Exception("Trying to heal a dead entity");
            }
            
            var calculated = false;
            switch (pendingHeal.CalculationType)
            {
                case CalculationTypes.PERCENTAGE:
                    var percentage = pendingHeal.Amount;

                    var range = target.MaxHealth - target.CurrentHealth;
                    pendingHeal.Amount = Convert.ToInt32(range * percentage * 0.01);
                    calculated = true;
                    break;
            }
            
            var amount = pendingHeal.Amount;

            if (!calculated)
            {
                if (target.CurrentHealth + amount > target.MaxHealth)
                    amount = target.MaxHealth - target.CurrentHealth;
            }

            target.CurrentHealth += amount;
            pendingHeal.Amount = amount;

            AnnounceHeal(pendingHeal);
        }

        /// <summary>
        /// Processing a basic shield heal
        /// </summary>
        /// <param name="pendingHeal"></param>
        private void ProcessShieldHeal(PendingHeal pendingHeal)
        {
            var target = pendingHeal.To;

            if (target == null)
            {
                Out.QuickLog("Something went wrong with healing shield of target", LogKeys.ERROR_LOG);
                throw new ArgumentNullException(nameof(target),"Something went wrong with healing shield of target");
            }

            if (target.IsDead)
            {
                Out.WriteLog("Cannot heal a dead entity", LogKeys.ERROR_LOG, target.Id);
                throw new Exception("Trying to heal a dead entity");
            }
            
            var calculated = false;
            switch (pendingHeal.CalculationType)
            {
                case CalculationTypes.PERCENTAGE:
                    var percentage = pendingHeal.Amount;

                    var range = target.MaxShield - target.CurrentShield;
                    pendingHeal.Amount = Convert.ToInt32(range * percentage * 0.01);
                    calculated = true;
                    break;
            }
            
            var amount = pendingHeal.Amount;

            if (!calculated)
            {
                if (target.CurrentShield + amount > target.MaxShield)
                    amount = target.MaxShield - target.CurrentShield;
            }

            target.CurrentShield += amount;
            pendingHeal.Amount = amount;

            AnnounceHeal(pendingHeal);
        }

        /// <summary>
        /// Processing an area health heal
        /// </summary>
        /// <param name="pendingHeal"></param>
        private void ProcessAreaHealthHeal(PendingHeal pendingHeal)
        {
            foreach (var affectedCharacter in pendingHeal.From.Spacemap.Entities.Where(x =>
                x.Value.Position.DistanceTo(pendingHeal.From.Position) < pendingHeal.AffectedDistance))
            {
                var newPendingHeal = new PendingHeal(affectedCharacter
                    .Value, pendingHeal.HealingType, pendingHeal.CalculationType, pendingHeal.Amount);
                ProcessHealthHeal(newPendingHeal);
            }
        }

        /// <summary>
        /// Processing an area shield heal
        /// </summary>
        /// <param name="pendingHeal"></param>
        private void ProcessAreaShieldHeal(PendingHeal pendingHeal)
        {
            foreach (var affectedCharacter in pendingHeal.From.Spacemap.Entities.Where(x =>
                x.Value.Position.DistanceTo(pendingHeal.From.Position) < pendingHeal.AffectedDistance))
            {
                var newPendingHeal = new PendingHeal(affectedCharacter
                    .Value, pendingHeal.HealingType, pendingHeal.CalculationType, pendingHeal.Amount);
                ProcessShieldHeal(newPendingHeal);
            }
        }
        
        /// <summary>
        /// Announcing the heal event
        /// </summary>
        /// <param name="pendingHeal"></param>
        private void AnnounceHeal(PendingHeal pendingHeal)
        {
            pendingHeal.To.OnHealed(pendingHeal);
        }

        /// <summary>
        /// Enqueueing a pending heal
        /// </summary>
        /// <param name="pendingHeal"></param>
        /// <exception cref="Exception">Duplicate record already exist</exception>
        /// <exception cref="ArgumentNullException">Incorrect pending heal parameters</exception>
        public void EnforcePendingHeal(PendingHeal pendingHeal)
        {
            if (_pendingHeals.Contains(pendingHeal))
            {
                Out.QuickLog("Already enqueued pending heal", LogKeys.ERROR_LOG);
                throw new Exception("Already existing pending heal, trying to add duplicate");
            }
            if (pendingHeal.To == null && !pendingHeal.IsAreaHeal())
            {
                Out.QuickLog("Incorrect non-area heal", LogKeys.ERROR_LOG);
                throw new ArgumentNullException("pendingHeal", "Something went wrong with (To) origin of pending heal");
            }
            _pendingHeals.Enqueue(pendingHeal);
        }
    }
}
