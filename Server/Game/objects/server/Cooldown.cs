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
        
        public Cooldown(AbstractAttacker sender, CooldownTypes type, int span)
        {
            Owner = sender;
            Type = type;
            Span = span;
            Created = DateTime.Now;
        }
        
        public Cooldown(AbstractAttacker sender, CooldownTypes type, int span, Action onCooldownFinish) : this(sender, type, span)
        {
            OnCompleteAction = onCooldownFinish;
        }

        private Cooldown(AbstractAttacker sender, CooldownTypes type, int span, Action onStartAction, Action onFinishAction) :
            this(sender, type, span, onFinishAction)
        {
            OnStartAction = onStartAction;
        }

        public void Start()
        {
            Started = DateTime.Now;
            OnStartAction?.Invoke();
        }
        
        public void OnFinishCooldown()
        {
            OnCompleteAction?.Invoke();
        }

        public void SetOnCompleteAction(Action action)
        {
            Console.WriteLine("Setting on Complete Action");
            OnCompleteAction = action;
        }

        public void SetOnStartAction(Action action)
        {
            OnStartAction = action;
        }
    }
}