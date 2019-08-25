using System.Linq;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.maps;
using Server.Main.objects;
using Server.Utils;

namespace Server.Game.controllers.server
{
    class RangeController : ServerImplementedController
    {
        /// <summary>
        /// When a new object is created, make sure to display it in every range
        /// </summary>
        
        public override void OnFinishInitiation()
        {
            Out.WriteLog("Successfully loaded Range Controller", LogKeys.SERVER_LOG); 
        }

        public override void Tick()
        {
        }

        /// <summary>
        /// Displaying the target character to all range entities
        /// </summary>
        /// <param name="targetCharacter">Target Character which will be displayed</param>
        public void DisplayCharacter(Character targetCharacter)
        {
            var entities = targetCharacter.Spacemap.Entities;
            foreach (var entity in entities.Where(x => x.Value != targetCharacter &&
                                                       x.Value.InCalculatedRange(targetCharacter)))
            {
                if (CharacterStateManager.Instance.IsInState(entity.Value, CharacterStates.SPAWNED))
                {
                    entity.Value.Controller.GetInstance<CharacterRangeController>().LoadCharacter(targetCharacter);
                }
            }
        }

        public void DisplayGameObject(GameObject gameObject)
        {
            
        }

        /// <summary>
        /// Removing the target character from all entities's screen
        /// </summary>
        /// <param name="character">Target Character which will be removed</param>
        public void RemoveCharacter(Character targetCharacter)
        {
            var entities = targetCharacter.Spacemap.Entities;
            foreach (var entity in entities.Where(x => x.Value != targetCharacter))
            {
                if (CharacterStateManager.Instance.IsInState(entity.Value, CharacterStates.SPAWNED))
                {
                    entity.Value.Controller.GetInstance<CharacterRangeController>().RemoveCharacter(targetCharacter);
                }
            }
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            
        }
    }
}
