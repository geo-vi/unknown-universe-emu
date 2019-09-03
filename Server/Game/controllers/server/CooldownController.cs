using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class CooldownController : ServerImplementedController
    {
        private readonly ConcurrentQueue<Cooldown> _cooldowns = new ConcurrentQueue<Cooldown>();

        public event EventHandler<Cooldown> OnCooldownAdded;

        public event EventHandler<Cooldown> OnCooldownFinish;
        
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Cooldown Controller", LogKeys.SERVER_LOG);
        }

        public override void Tick()
        {
            ProcessCooldowns();
        }

        /// <summary>
        /// Processing all the cooldowns
        /// </summary>
        private void ProcessCooldowns()
        {
            for (var i = 0; i < _cooldowns.Count; i++)
            {
                _cooldowns.TryDequeue(out var cooldown);
                if (cooldown.Started.AddMilliseconds(cooldown.TotalMilliseconds) > DateTime.Now)
                {
                    _cooldowns.Enqueue(cooldown);
                    continue;
                }

                FinishCooldown(cooldown);
            }
        }

        /// <summary>
        /// Adding a cooldown
        /// </summary>
        /// <param name="cooldown">Cooldown</param>
        public void AddCooldown(Cooldown cooldown)
        {
            if (_cooldowns.Contains(cooldown))
            {
                Out.QuickLog("Something went wrong with adding a cooldown, duplicate error", LogKeys.ERROR_LOG);
                throw new Exception("Same cooldown already exists, failed adding");
            }
            _cooldowns.Enqueue(cooldown);

            OnCooldownAdded?.Invoke(this, cooldown);
        }

        private void FinishCooldown(Cooldown cooldown)
        {
            cooldown.OnFinishCooldown();

            OnCooldownFinish?.Invoke(this, cooldown);
        }

        /// <summary>
        /// Checking if it exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cooldownType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool Exists(AbstractAttacker sender, CooldownTypes cooldownType)
        {
            if (sender == null)
            {
                Out.QuickLog("Something went wrong while checking the cooldown", LogKeys.ERROR_LOG);
                throw new Exception("Sender is null, something went wrong while checking cooldown");
            }

            return _cooldowns.Any(x => x.Owner == sender && x.Type == cooldownType);
        }

        /// <summary>
        /// Getting the cooldown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cooldownType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Cooldown Get(AbstractAttacker sender, CooldownTypes cooldownType)
        {
            if (sender == null)
            {
                Out.QuickLog("Something went wrong while getting the cooldown", LogKeys.ERROR_LOG);
                throw new Exception("Sender is null, something went wrong while getting cooldown");
            }

            if (_cooldowns.Count(x => x.Owner == sender && x.Type == cooldownType) > 1)
            {
                Out.QuickLog("More than one cooldown found");
                throw new Exception("Something went wrong with the cooldown, more than 1 record found");
            }

            return _cooldowns.FirstOrDefault(x => x.Owner == sender && x.Type == cooldownType);
        }
    }
}