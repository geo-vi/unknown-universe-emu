using Server.Main;
using Server.Main.objects;

namespace Server.Game.controllers.implementable
{
    abstract class ServerImplementedController : ITick
    {
        /// <summary>
        /// Tick ID
        /// </summary>
        public int TickId { get; set; }

        public abstract void Tick();

        /// <summary>
        /// Starts the controller up
        /// </summary>
        public void Initiate()
        {
            Global.TickManager.Add(this, out int tickId);
            TickId = tickId;
            OnFinishInitiation();
        }

        public virtual void OnFinishInitiation()
        {
            
        }
    }
}