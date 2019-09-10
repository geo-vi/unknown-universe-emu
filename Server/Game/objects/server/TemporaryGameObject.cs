using System;
using Server.Game.objects.maps;

namespace Server.Game.objects.server
{
    class TemporaryGameObject
    {
        public GameObject GameObject { get; set; }
        
        public int LifeSpan { get; set; }
        

        public int TotalSeconds => TotalMilliseconds / 1000;

        public int TotalMilliseconds => Convert.ToInt32((EstimatedEndTime - DateTime.Now).TotalMilliseconds);

        public DateTime EstimatedEndTime { get; set; }
        
        public DateTime Created { get; set; }

        public Action OnLifeFinishAction { get; set; }

        public event EventHandler<GameObject> OnLifeStarted;

        public event EventHandler<GameObject> OnLifeEnded;
        
        /// <summary>
        /// Primary constructor
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="lifeSpan"></param>
        public TemporaryGameObject(GameObject gameObject, int lifeSpan)
        {
            GameObject = gameObject;
            LifeSpan = lifeSpan;
            Created = DateTime.Now;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="lifeSpan"></param>
        /// <param name="onLifeFinishAction"></param>
        public TemporaryGameObject(GameObject gameObject, int lifeSpan, Action onLifeFinishAction) :
            this(gameObject, lifeSpan)
        {
            OnLifeFinishAction = onLifeFinishAction;
        }
        
        /// <summary>
        /// Starting a temporary object's life
        /// </summary>
        public void StartLife()
        {
            EstimatedEndTime = DateTime.Now.AddMilliseconds(LifeSpan);
            OnLifeStarted?.Invoke(this, GameObject);
        }

        /// <summary>
        /// Ending a temporary object's life
        /// </summary>
        public void EndLife()
        {
            OnLifeFinishAction?.Invoke();
            OnLifeEnded?.Invoke(this, GameObject);
        }
    }
}