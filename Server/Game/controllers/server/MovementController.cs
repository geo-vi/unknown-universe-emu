using System.Collections.Concurrent;
using Server.Game.controllers.implementable;
using Server.Game.objects.entities;
using Server.Game.objects.implementable;
using Server.Game.objects.server;

namespace Server.Game.controllers.server
{
    class MovementController : ServerImplementedController
    {
        /// <summary>
        /// Start movement when possible
        /// </summary>
        public ConcurrentQueue<PendingMovement> PendingMovements = new ConcurrentQueue<PendingMovement>();
        
        /// <summary>
        /// Update range every tick about movement
        /// </summary>
        public ConcurrentDictionary<Character, PendingMovement> MovementsInProgress = new ConcurrentDictionary<Character, PendingMovement>();
        
        public override void Tick()
        {
            throw new System.NotImplementedException();
        }

        public void CreateMovement(Character character, Vector destination)
        {
            
        }
    }
}
