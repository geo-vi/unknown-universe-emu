using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.extra.abilities;

namespace NettyBaseReloaded.Game.objects.world.players.extra
{
    abstract class Ability : PlayerBaseClass
    {
        public bool Enabled => !CooldownActive && !Active;

        public bool Active { get; set; }

        public DateTime TimeFinish = new DateTime();

        public Cooldown Cooldown;

        public Abilities AbilityType { get; }

        public int AbilityId => (int) AbilityType;

        public virtual int ActivatorId => Player.Id;

        private bool CooldownActive => Cooldown?.EndTime  > DateTime.Now;

        private bool LastStatusSent;

        protected Ability(Player player, Abilities typeOfAbility) : base(player)
        {
            AbilityType = typeOfAbility;
        }

        public int TimeLeft => Math.Abs((TimeFinish - DateTime.Now).Seconds);
        public virtual bool IsStoppable => false;

        public List<int> TargetIds = new List<int>();

        public abstract void Tick();

        public abstract void execute();

        public virtual void ThreadUpdate() { }

        public void Start()
        {
            if (Enabled || CooldownActive) return;
            Task.Factory.StartNew(() =>
            {
                while (Active && Player.Controller.Active)
                {
                    System.Threading.Thread.Sleep(1000);
                    ThreadUpdate();
                }
            });

            foreach (var target in TargetIds)
            {
                var targetSession = World.StorageManager.GetGameSession(target);
                if (targetSession != null)
                {
                    Packet.Builder.AbilityStartCommand(targetSession, this);
                }
            }
            Packet.Builder.AbilityStartCommand(Player.GetGameSession(), this);
        }
        
        protected void ShowEffect()
        {
            foreach (var target in TargetIds)
            {
                var targetSession = World.StorageManager.GetGameSession(target);
                if (targetSession != null)
                {
                    Packet.Builder.AbilityEffectActivationCommand(targetSession, this);
                }
            }
            Packet.Builder.AbilityEffectActivationCommand(Player.GetGameSession(), this);
        }

        public void End()
        {
            foreach (var target in TargetIds)
            {
                var targetSession = World.StorageManager.GetGameSession(target);
                if (targetSession != null)
                {
                    Packet.Builder.AbilityStopCommand(targetSession, this);
                    Packet.Builder.AbilityEffectDeActivation(targetSession, this);
                }
            }

            Packet.Builder.AbilityStopCommand(Player.GetGameSession(), this);
            Packet.Builder.AbilityEffectDeActivation(Player.GetGameSession(), this);
        }
        
        protected bool Update()
        {
            bool final = Enabled;
            if (LastStatusSent != final)
            {
                Packet.Builder.AbilityStatusSingleCommand(Player.GetGameSession(), this);
                LastStatusSent = final;
            }

            return final;
        }
    }
}
