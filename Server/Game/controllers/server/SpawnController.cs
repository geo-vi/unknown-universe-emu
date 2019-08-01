using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    /// <summary>
    /// SpawnController will be used for indicating entities, objects and other loadables.
    /// Instances in which the controller will be called are:
    /// - Entity spawned
    /// 
    /// </summary>
    class SpawnController : ServerImplementedController
    {
        private ConcurrentQueue<Character> _charactersPendingSpawn = new ConcurrentQueue<Character>();

        private const int SPAWN_PROJECTION_RANGE = 2000;
        
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Spawn Controller", LogKeys.SERVER_LOG); 
        }

        public override void Tick()
        {
            while(!_charactersPendingSpawn.IsEmpty)
            {
                _charactersPendingSpawn.TryDequeue(out var character);
                SpawnCharacter(character);
                ProjectToRange(character);
                OnFinishSpawning(character);
            }
        }

        public void QueueCharacterForSpawning(Character character)
        {
            if (_charactersPendingSpawn.Contains(character))
            {
                Out.QuickLog("Character is already on the spawn queue, something went wrong", LogKeys.ERROR_LOG);
                throw new Exception("Something went wrong while adding character to the spawn queue");
            }
            _charactersPendingSpawn.Enqueue(character);
        }
        
        private void SpawnCharacter(Character character)
        {
            CharacterStateManager.Instance.RequestStateChange(character, CharacterStates.SPAWNED, out var stateChanged);
            if (!stateChanged)
            {
                // something is fucking wrong :?
                // so step 1 log, step 2 break ))
                Out.QuickLog("Something went wrong when trying to spawn character", LogKeys.ERROR_LOG);
                throw new Exception("Something went wrong when trying to spawn character");
            }

            CharacterStateManager.Instance.RemoveState(character, CharacterStates.LOGIN);
        }
        
        private void ProjectToRange(Character targetCharacter)
        {
            var entities = targetCharacter.Spacemap.Entities;
            foreach (var entity in entities.Where(x => x.Value.Position.DistanceTo(targetCharacter.Position) > SPAWN_PROJECTION_RANGE))
            {
                CreateRangeInstanceForEntity(entity.Value, targetCharacter);
                if (entity.Value is Player player)
                {
                    CreateEntityForPlayerScreen(player, targetCharacter);
                }
            }

            entities.TryAdd(targetCharacter.Id, targetCharacter);
        }

        private void CreateRangeInstanceForEntity(Character entity, Character rangeInstance)
        {
            if (rangeInstance == null)
            {
                Out.QuickLog("Spawn Controller couldn't access the value of the range's instance", LogKeys.ERROR_LOG);
                throw new Exception("Trying to create a null instance");
            }

            if (!entity.RangeView.CharactersInRenderRange.TryAdd(rangeInstance.Id, rangeInstance))
            {
                Out.QuickLog("Entity of the same key already exists", LogKeys.ERROR_LOG);
                throw new Exception("Trying to add a duplicate");
            }
        }

        private void CreateEntityForPlayerScreen(Player player, Character createdInstance)
        {
            PrebuiltPlayerCommands.Instance.CreateShipCommand(player, createdInstance);
        }
        
        private void OnFinishSpawning(Character character)
        {
            Out.WriteLog("Character [with ID: " + character.Id + "] successfully spawned", LogKeys.SERVER_LOG);
        }
    }
}
