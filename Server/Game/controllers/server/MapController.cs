using System;
using System.Collections.Concurrent;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    /// <summary>
    /// This controller will be used for controlling map objects and their behavior.
    /// Instances in which this controller shall be used:
    /// - Map Object Added
    /// - Map Object Removed
    /// - Map Object Moved
    /// - Temporary Objects with Disposal
    /// </summary>
    class MapController : ServerImplementedController
    {
        private ConcurrentDictionary<int, Spacemap> Spacemaps
        {
            get
            {
                var spacemaps = GameStorageManager.Instance.Spacemaps;
                return spacemaps;
            }
        } 
        
        public override void OnFinishInitiation()
        {
            // Create NPCs
            // Create Objects
            Out.WriteLog("Successfully loaded Map Controller", LogKeys.SERVER_LOG);
        }

        public override void Tick()
        {
            
        }

        /// <summary>
        /// Trying to create character on the map
        /// </summary>
        /// <param name="character">Character trying to create</param>
        /// <returns></returns>
        /// <exception cref="Exception">Something went wrong with the character</exception>
        public bool CreateCharacterOnMap(Character character)
        {
            if (character.Spacemap == null)
            {
                Out.WriteLog("Invalid Spacemap when adding a character", LogKeys.ALL_CHARACTER_LOG, character.Id);
                throw new Exception("Invalid Spacemap when adding Character");
            }
            
            var mapAdd = character.Spacemap.Entities.TryAdd(character.Id, character);
            ServerController.Get<RangeController>().DisplayCharacter(character);
            return mapAdd;
        }

        public bool RemoveCharacterFromMap(Character character)
        {
            if (character.Spacemap == null)
            {
                Out.WriteLog("Invalid Spacemap when removing a character", LogKeys.ALL_CHARACTER_LOG, character.Id);
                throw new Exception("Invalid Spacemap when removing Character");
            }
            
            var mapRemove = character.Spacemap.Entities.TryRemove(character.Id, out _);
            ServerController.Get<RangeController>().RemoveCharacter(character);
            return mapRemove;
        }
    }
}
