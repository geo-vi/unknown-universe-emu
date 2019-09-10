using System;
using Server.Game.controllers.server;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects.enums;
using Server.Game.objects.server;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.characters
{
    class CharacterDestructionController : AbstractedSubController
    {
        public override void OnAdded()
        {
            Character.OnDestroyed += CharacterOnDestroyed;
        }

        public override void OnOverwritten()
        {
            Character.OnDestroyed -= CharacterOnDestroyed;
        }

        public override void OnRemoved()
        {
            Character.OnDestroyed -= CharacterOnDestroyed;
        }
        
        protected virtual void CharacterOnDestroyed(object sender, PendingDestruction e)
        {
            Character.UnsetMovement();

            CharacterStateManager.Instance.RequestStateChange(Character, CharacterStates.DEAD, out var stateChanged);

            if (!stateChanged)
            {
                Out.WriteLog("Something went wrong with destroying character", LogKeys.ERROR_LOG, Character.Id);
                throw new Exception("Failed changing state to Dead");
            }

            PrebuiltCombatCommands.Instance.DestructionCommand(e);
            
            Out.WriteLog("Character destroyed", LogKeys.ALL_CHARACTER_LOG, Character.Id);
        }
        
        public virtual void ReviveCharacter()
        {
            CharacterStateManager.Instance.WipeStates(Character);
            
            Character.CurrentHealth = 1000;
            Character.IsDead = false;
            
            ServerController.Get<SpawnController>().QueueCharacterForSpawning(Character);
            Character.OnRevive();
        }
    }
}