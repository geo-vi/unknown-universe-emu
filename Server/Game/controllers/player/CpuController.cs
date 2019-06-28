using System.Collections.Concurrent;
using System.Collections.Generic;
using Server.Game.objects;
using Server.Game.objects.entities.ships.equipment;

namespace Server.Game.controllers.player
{
    class CpuController
    {
        private ConcurrentDictionary<string,Item> ToggledCpu = new ConcurrentDictionary<string, Item>();
        
        public void ToggleCpu(Item cpuItem)
        {
        }

        public bool CpuActive(string cpuItemId)
        {
            return false;
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