using System;
using System.Linq;
using Server.Game.controllers.characters;
using Server.Game.controllers.implementable;
using Server.Game.managers;
using Server.Game.objects.entities;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;
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

        public void DisplayCharacter(Character character)
        {
            foreach (var entity in character.Spacemap.Entities.Where(x =>
                x.Value.Position.DistanceTo(character.Position) < 2000))
            {
                if (CharacterStateManager.Instance.IsInState(entity.Value, CharacterStates.SPAWNED))
                {
                    entity.Value.Controller.GetInstance<CharacterRangeController>().LoadCharacter(entity.Value);
                }
            }
        }

        public void DisplayGameObject(GameObject gameObject)
        {
            
        }

        public void RemoveAttackable(IAttackable attackable)
        {
            
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            
        }
    }
}
