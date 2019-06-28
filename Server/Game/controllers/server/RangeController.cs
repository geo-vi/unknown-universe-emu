using Server.Game.controllers.implementable;
using Server.Game.objects.implementable;
using Server.Game.objects.maps;
using Server.Main.objects;

namespace Server.Game.controllers.server
{
    class RangeController : ServerImplementedController
    {
        /// <summary>
        /// When a new object is created, make sure to display it in every range
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        
        public override void Tick()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayAttackable(IAttackable attackable)
        {
            
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
