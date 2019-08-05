using System.Collections.Concurrent;
using Server.Game.objects;
using Server.Game.objects.entities.ships.equipment;
using Server.Game.objects.entities.ships.items;

namespace Server.Game.controllers.players
{
    class CpuController : PlayerSubController
    {
        private ConcurrentDictionary<string,Item> ToggledCpus = new ConcurrentDictionary<string, Item>();
        
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