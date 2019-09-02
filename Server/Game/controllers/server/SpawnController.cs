using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Main;
using Server.Main.managers;
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
                DisplayEntitiesWithinRange(character);
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
        
        /// <summary>
        /// Creating state and preparing
        /// </summary>
        /// <param name="character"></param>
        /// <exception cref="Exception"></exception>
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
        
        /// <summary>
        /// Projecting to range the character's entry
        /// </summary>
        /// <param name="targetCharacter"></param>
        private void ProjectToRange(Character targetCharacter)
        {
            ServerController.Get<MapController>().CreateCharacterOnMap(targetCharacter);
        }

        /// <summary>
        /// Displaying all entities which are in visible range
        /// </summary>
        /// <param name="targetCharacter">Character which is going to get all entities added to him</param>
        private void DisplayEntitiesWithinRange(Character targetCharacter)
        {
            targetCharacter.Controller.GetInstance<CharacterRangeController>().LoadCharactersInRange();
        }
        
        /// <summary>
        /// Once all the spawn process finishes this will trigger
        /// </summary>
        /// <param name="character"></param>
        private void OnFinishSpawning(Character character)
        {
            Out.WriteLog("Character [with ID: " + character.Id + "] successfully spawned", LogKeys.SERVER_LOG);
        }
    }
}
