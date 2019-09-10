using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Game.objects;
using Server.Game.objects.entities;
using Server.Game.objects.implementable;
using Server.Game.objects.maps;
using Server.Game.objects.server;
using Server.Main;
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
        private readonly ConcurrentQueue<TemporaryGameObject> _temporaryGameObjects = new ConcurrentQueue<TemporaryGameObject>();
        
        public override void OnFinishInitiation()
        {
            CreateAliens();
            CreateGameObjects();
            Out.WriteLog("Successfully loaded Map Controller", LogKeys.SERVER_LOG);
        }

        public override void Tick()
        {
            ProcessTemporaryGameObjects();
        }

        private void ProcessTemporaryGameObjects()
        {
            //todo...
        }
        

        /// <summary>
        /// Creating all the aliens that are preloaded
        /// </summary>
        private void CreateAliens()
        {
//            foreach (var map in GameStorageManager.Instance.Spacemaps)
//            {
//                var npcs = map.Value.Npcs;
//                foreach (var npc in npcs)
//                {
//                    SpacemapManager.Instance.CreateNpc(map.Value, npc);
//                }
//            }
        }
        
        /// <summary>
        /// Creating all the game objects that are preloaded
        /// </summary>
        private void CreateGameObjects()
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

        /// <summary>
        /// Removing character from map
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Moving character to another map
        /// </summary>
        /// <param name="character"></param>
        /// <param name="map"></param>
        /// <param name="newPosition"></param>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void MoveCharacterToMap(Character character, Spacemap map, Vector newPosition = null)
        {
            if (character.Spacemap == null)
            {
                Out.QuickLog("Spacemap origin is invalid, cannot move", LogKeys.ERROR_LOG);
                throw new NullReferenceException("Character's spacemap is null");
            }

            if (map == null)
            {
                Out.QuickLog("Spacemap target is invalid, cannot move", LogKeys.ERROR_LOG);
                throw new ArgumentNullException(nameof(map), "Target map is null");
            }

            var originMap = character.Spacemap;
            RemoveCharacterFromMap(character);
            character.Spacemap = map;
            if (newPosition == null)
            {
                Out.WriteLog("No position change in map change", LogKeys.ALL_CHARACTER_LOG, character.Id);
            }
            else
            {
                character.Position = newPosition;
            }
            CreateCharacterOnMap(character);
            Out.WriteLog("Changed map for character from " + originMap.Name + " to " + map.Name, LogKeys.ALL_CHARACTER_LOG, character.Id);
        }
        
        public bool CreateGameObject(GameObject gameObject)
        {
            return false;
        }

        public bool RemoveGameObjectFromMap(GameObject gameObject)
        {
            return false;
        }

        public void CreateTemporaryObject(GameObject temporaryGameObject)
        {
            
        }
    }
}
