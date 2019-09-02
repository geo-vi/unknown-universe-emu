using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Game.objects;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.players
{
    class CpuController : PlayerSubController
    {
        protected readonly Dictionary<string, Action> ActivationLinks = new Dictionary<string, Action>();
        
        private void PreloadActions()
        {
            ActivationLinks.TryAdd("cloak", CloakShip);
            ActivationLinks.TryAdd("jump", ActivateJumpingSequence);
        }
        
        public override void OnAdded()
        {
            PreloadActions();
            Player.OnConfigurationChanged += OnPlayerConfigurationChanged;
            InitiateCpuView();
        }

        public override void OnRemoved()
        {
            Player.OnConfigurationChanged -= OnPlayerConfigurationChanged;
        }

        private void OnPlayerConfigurationChanged(object sender, int e)
        {
            if (sender != Player)
            {
                Out.QuickLog("Sender is invalid, failed updating cpus for config", LogKeys.ERROR_LOG);
                throw new ArgumentException("Invalid sender, failed to process");
            }
            
            InitiateCpuView();
        }

        private void InitiateCpuView()
        {
        }
        
        /// <summary>
        /// Will activate a CPU
        /// </summary>
        /// <param name="cpuItem">target item for activation</param>
        public void ActivateCpuProcess(Item cpuItem)
        {
            cpuItem.Activated = true;
            
            if (ActivationLinks.ContainsKey(cpuItem.LootId))
            {
                ActivationLinks[cpuItem.LootId].Invoke();
            }
        }
        
        public void CloakShip()
        {
            
        }

        public void ActivateJumpingSequence()
        {
            
        }

        public void ActivateAdvancedJumpingSequence(Spacemap targetMap)
        {
            
        }

        public void FinishJumpSequence()
        {
            
        }

        public void RepairDrone()
        {
            
        }
    }
}