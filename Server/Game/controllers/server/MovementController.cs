using System;
using System.Collections.Concurrent;
using System.Linq;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Game.netty;
using Server.Game.netty.commands;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
using Server.Game.objects.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class MovementController : ServerImplementedController
    {
        /// <summary>
        /// Start movement when possible
        /// </summary>
        private ConcurrentQueue<PendingMovement> PendingMovements = new ConcurrentQueue<PendingMovement>();
        
        /// <summary>
        /// Update range every tick about movement
        /// </summary>
        private ConcurrentDictionary<Character, PendingMovement> MovementsInProgress = new ConcurrentDictionary<Character, PendingMovement>();

        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Map Controller", LogKeys.SERVER_LOG); 
        }

        public override void Tick()
        {
            ProcessQueue();
            ProcessMovements();
        }

        /// <summary>
        /// Processing the queue
        /// </summary>
        /// <exception cref="Exception">Caused by duplicate movement OR dequeue failed</exception>
        private void ProcessQueue()
        {
            while (!PendingMovements.IsEmpty)
            {
                if (PendingMovements.TryDequeue(out var dequeued))
                {
                    if (MovementsInProgress.ContainsKey(dequeued.ParentCharacter))
                    {
                        Out.QuickLog("Duplicate movement during dequeue tick", LogKeys.ERROR_LOG);
                        throw new Exception("Duplicate movement during dequeue tick");
                    }

                    MovementsInProgress.TryAdd(dequeued.ParentCharacter, dequeued);
                    dequeued.ParentCharacter.OnConfigurationChanged += OnMovementRateChange;
                    dequeued.ParentCharacter.Controller.GetInstance<CharacterMovementController>().OnMovementStarted();
                }
                else
                {
                    Out.QuickLog("Dequeue isn't possible in Movement Tick", LogKeys.ERROR_LOG);
                    throw new Exception("Something went totally fucking wrong with movement, cannot dequeue");
                }
            }
        }

        /// <summary>
        /// Processing the movements
        /// </summary>
        private void ProcessMovements()
        {
            foreach (var movement in MovementsInProgress)
            {
                var character = movement.Value.ParentCharacter;
                if (movement.Value.EndPosition == character.Position)
                {
                    MovementsInProgress.TryRemove(movement.Key, out var pendingMovement);
                    OnMovementFinished(pendingMovement);
                    continue;
                }

                character.Position = GetActualPosition(character);
                
                CharacterRangeManager.Instance.UpdateCharacterRange(character);
                
                if (movement.Value.MovementRendered) 
                    continue;
                
                //Gets the movement time
                character.MovementTime = GetMovementTime(character, movement.Value.EndPosition);

                //Gets the system time when the movement starts
                character.MovementStartTime = DateTime.Now;
                character.Moving = true;

                movement.Value.MovementRendered = true;

                var parameters = new object[]
                    {character.Id, character.Destination.X, character.Destination.Y, character.MovementTime};
                
                Packet.Builder.BuildToRange(character, Commands.MOVE_COMMAND, parameters, parameters);
//                PrebuiltLegacyCommands.Instance.ServerMessage(character as Player, "X: " + character.Position.X + "; Y:" + character.Position.Y + "; T:" + character.MovementTime + "ms\n" +
//                                                                                   "X2: " + movement.Value.EndPosition.X + "; Y2: " + movement.Value.EndPosition.Y);
            }
        }

        /// <summary>
        /// Finishing player movement
        /// </summary>
        /// <param name="pendingMovement"></param>
        /// <exception cref="Exception"></exception>
        private void OnMovementFinished(PendingMovement pendingMovement)
        {
            var character = pendingMovement.ParentCharacter;
            if (character == null)
            {
                Out.QuickLog("Something went wrong, character in PendingMovement is null");
                throw new Exception("Something went wrong during movement finishing");
            }
            
            character.OnConfigurationChanged -= OnMovementRateChange;
            character.Controller.GetInstance<CharacterMovementController>().OnMovementFinished();
            Out.WriteLog("Movement finished for character", LogKeys.ALL_CHARACTER_LOG, character.Id);
        }
        
        /// <summary>
        /// Getting the actual position of a character
        /// </summary>
        /// <param name="character">Target Character</param>
        /// <returns></returns>
        private Vector GetActualPosition(Character character)
        {
            Vector actualPosition;

            if (character.Moving)
            {
                var timeElapsed = (DateTime.Now - character.MovementStartTime).TotalMilliseconds;

                //if the character continues moving
                if (timeElapsed < character.MovementTime)
                {
                    //maths time, it returns the actual position while flying
                    actualPosition = new Vector((int)Math.Round(character.OldPosition.X + (character.Direction.X * (timeElapsed / character.MovementTime))),
                        (int)Math.Round(character.OldPosition.Y + (character.Direction.Y * (timeElapsed / character.MovementTime))));
                }
                else
                {
                    //the character should be on the destination position
                    character.Moving = false;
                    actualPosition = character.Destination;
                }
            }
            else
            {
                //the character is not moving
                actualPosition = character.Position;
            }

            //updates the actual position into the character
            character.Position = actualPosition;

            /*
             *  TODO: FIXING A BUG WHERE IT VISUALLY TELEPORTS PLAYER / PLAYER BECOMES INVSIBILE
             */
            return actualPosition;
        }

        /// <summary>
        /// Getting movement time of a character to destination
        /// </summary>
        /// <param name="character">Target Character</param>
        /// <param name="destination">Destination Vector (x;y)</param>
        /// <returns></returns>
        private int GetMovementTime(Character character, Vector destination)
        {
            //Sets the position before the movement
            character.OldPosition = GetActualPosition(character);

            //And the destination position
            var destinationPosition = destination;
            character.Destination = destinationPosition;

            //Same with the direction, will be used to calculate the position
            character.Direction = new Vector(destinationPosition.X - character.OldPosition.X, destinationPosition.Y - character.OldPosition.Y);

            var distance = destinationPosition.DistanceTo(character.OldPosition);

            var time = Math.Round(distance / character.Speed * 1000);

//            Console.WriteLine("Character: {0} => Position = {1} , Destination = {2} , Distance = {3} , Speed = {4} , Time = {5}", character.Name, character.Position, character.Destination, distance, character.Speed, time);

            return (int)time;
        }
        
        /// <summary>
        /// Creating and queueing the movement
        /// </summary>
        /// <param name="character">Character</param>
        /// <param name="destination">Destination Vector (x;y)</param>
        public void CreateMovement(Character character, Vector destination)
        {
            if (MovementsInProgress.ContainsKey(character))
            {
                var movement = MovementsInProgress[character];
                movement.EndPosition = destination;
                movement.MovementRendered = false;
                Out.WriteLog("Changing destination for character", LogKeys.ALL_CHARACTER_LOG, character.Id);
                return;
            }
            
            CreateMovement(new PendingMovement(character, character.Position, destination));
        }

        /// <summary>
        /// Creating and queueing the movement
        /// </summary>
        /// <param name="movement">Pending movement class containing datas</param>
        /// <exception cref="Exception">Same movement exists</exception>
        public void CreateMovement(PendingMovement movement)
        {
            CharacterStateManager.Instance.RequestStateChange(movement.ParentCharacter,
                CharacterStates.MOVING, out var stateChanged);
            if (!stateChanged)
            {
                Out.WriteLog("Error moving character", LogKeys.ERROR_LOG, movement.ParentCharacter.Id);
                throw new Exception("Something went totally wrong with movement state of character");    
            }
            
            if (PendingMovements.Contains(movement))
            {
                Out.QuickLog("Trying to create a duplicate movement", LogKeys.ERROR_LOG);
                throw new Exception("Trying to create a duplicate movement, something went wrong");
            }
            PendingMovements.Enqueue(movement);
        }

        /// <summary>
        /// Movement rate changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMovementRateChange(object sender, int e)
        {
            if (!(sender is Character character))
            {
                Out.QuickLog("Something went wrong while updating movement rate", LogKeys.ERROR_LOG);
                throw new ArgumentException("Something went wrong while updating movement rate, sender is not a character");
            }

            if (!MovementsInProgress.ContainsKey(character))
            {
                Out.QuickLog("Character is not moving, something went wrong", LogKeys.ERROR_LOG);
                throw new Exception("Character is not moving, event not removed");
            }
            
            var foundMoveEntry = MovementsInProgress[character];
            foundMoveEntry.MovementRendered = false;
        }
    }
}
