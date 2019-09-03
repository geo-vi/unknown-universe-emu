using System;
using Server.Game.objects.enums;
using Server.Game.objects.implementable;

namespace Server.Game.objects.server
{
    class Cooldown
    {
        public AbstractAttacker Owner { get; set; }
        
        public CooldownTypes Type { get; set; }
        
        private int Span { get; set; }
        
        private Action OnCompleteAction { get; set; }

        public int TotalSeconds => Span / 1000;

        public int TotalMilliseconds => Span;
        
        public DateTime Created { get; private set; } 

        public DateTime Started { get; private set; }
        
        private Action OnStartAction { get; set; }
        
        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        /// <param name="span"></param>
        public Cooldown(AbstractAttacker sender, CooldownTypes type, int span)
        {
            Owner = sender;
            Type = type;
            Span = span;
            Created = DateTime.Now;
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        /// <param name="span"></param>
        /// <param name="onCooldownFinish"></param>
        public Cooldown(AbstractAttacker sender, CooldownTypes type, int span, Action onCooldownFinish) : this(sender, type, span)
        {
            OnCompleteAction = onCooldownFinish;
        }

        /// <summary>
        /// Secondary constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="type"></param>
        /// <param name="span"></param>
        /// <param name="onStartAction"></param>
        /// <param name="onFinishAction"></param>
        public Cooldown(AbstractAttacker sender, CooldownTypes type, int span, Action onStartAction, Action onFinishAction) :
            this(sender, type, span, onFinishAction)
        {
            OnStartAction = onStartAction;
        }
        
        /// <summary>
        /// Starting the cooldown
        /// </summary>
        public void Start()
        {
            Started = DateTime.Now;
            OnStartAction?.Invoke();
        }
        
        /// <summary>
        /// Upon cooldown finish, strike event
        /// </summary>
        public void OnFinishCooldown()
        {
            OnCompleteAction?.Invoke();
        }

        /// <summary>
        /// Cooldown on complete action
        /// </summary>
        /// <param name="action">Function to be executed</param>
        public void SetOnCompleteAction(Action action)
        {
            OnCompleteAction = action;
        }

        /// <summary>
        /// Setting on start action
        /// </summary>
        /// <param name="action">Function to be executed on start of cooldown</param>
        public void SetOnStartAction(Action action)
        {
            OnStartAction = action;
        }
    }
}