using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Game.controllers.implementable;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.maps.objects.assets.attackable;
using Server.Game.objects.server;
using Server.Main;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    /// <summary>
    /// This controller will be used for base of destructing attackables and objects
    /// Instances in which this controller will be called or used:
    /// - Player explosion / destruction
    /// - Pet explosion / destruction
    /// - Npc explosion / destruction
    /// 
    /// </summary>
    class DestructionController : ServerImplementedController
    {
        private readonly ConcurrentQueue<PendingDestruction> _pendingDestruction = new ConcurrentQueue<PendingDestruction>();

        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Destruction Controller", LogKeys.SERVER_LOG);
        }
        
        public override void Tick()
        {
            ProcessDestruction();
        }

        /// <summary>
        /// Processing all the destruction queued
        /// </summary>
        /// <exception cref="Exception">Failed dequeueing</exception>
        private void ProcessDestruction()
        {
            while (!_pendingDestruction.IsEmpty)
            {
                if (!_pendingDestruction.TryDequeue(out var dequeued))
                {
                    Out.QuickLog("Failed dequeueing a pending destruction", LogKeys.ERROR_LOG);
                    throw new Exception("Something went wrong in Destructions queue");
                }

                if (dequeued.Target == null)
                {
                    Out.QuickLog("Failed destroying target", LogKeys.ERROR_LOG);
                    throw new NullReferenceException("Failed destroying target due to it being null");
                }

                if (dequeued.Target.IsDead)
                {
                    Out.WriteLog("Trying to double destroy", LogKeys.ERROR_LOG, dequeued.Target.Id);
                    throw new Exception("Trying to destroy an already dead entity");
                }

                switch (dequeued.Target)
                {
                    case Character _:
                        ProcessCharacterDestruction(dequeued);
                        break;
                    case AttackableAsset _:
                        ProcessAttackableAssetDestruction(dequeued);
                        break;
                }
            }
        }

        private void ProcessCharacterDestruction(PendingDestruction pendingDestruction)
        {
            var target = pendingDestruction.Target;

            target.CurrentHealth = 0;
            target.CurrentNanoHull = 0;
            target.CurrentShield = 0;
            
            target.IsDead = true;
            
            target.OnDestroy(pendingDestruction);
        }

        private void ProcessAttackableAssetDestruction(PendingDestruction pendingDestruction)
        {
            var target = pendingDestruction.Target;

            target.CurrentHealth = 0;
            target.CurrentNanoHull = 0;
            target.CurrentShield = 0;
            
            target.IsDead = true;
            
            target.OnDestroy(pendingDestruction);
        }

        public void CreateDestroyRecord(PendingDestruction pendingDestruction)
        {
            if (_pendingDestruction.Contains(pendingDestruction))
            {
                Out.QuickLog("Trying to add an already existing destruction", LogKeys.ERROR_LOG);
                throw new Exception("Trying to duplicate add a pending destruction");
            }
            
            _pendingDestruction.Enqueue(pendingDestruction);
        }
    }
}
