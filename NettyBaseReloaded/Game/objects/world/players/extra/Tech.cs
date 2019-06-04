using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players.extra
{
    abstract class Tech : PlayerBaseClass
    {
        public bool Enabled => !Active && TimeFinish < DateTime.Now;
        public bool Active { get; set; }

        public int Count { get; set; }
        public DateTime TimeFinish { get; set; }

        public bool TaskRun = false;

        protected Tech(Player player) : base(player) { }

        public int TimeLeft => Math.Abs((TimeFinish - DateTime.Now).Seconds);
        
        public abstract void Tick();

        public abstract void Disable();

        public abstract void execute();

        public virtual void ThreadUpdate() { }

        public void Start()
        {
            Task.Factory.StartNew(TechAsynchronisation);
        }

        public async void TechAsynchronisation()
        {
            if (TaskRun) return;
            TaskRun = true;
            while (Active && Player.Controller != null)
            {
                if (!Player.Controller.Active || Player.Controller.StopController)
                {
                    TimeFinish = DateTime.Now;
                    return;
                }
                if (TimeFinish < DateTime.Now)
                    Disable();

                ThreadUpdate();
                await Task.Delay(1000);
            }

            TaskRun = false;
        }
        
        public int GetStatus()
        {
            if (Active) return 2;
            if (Enabled) return 1;
            return 0;
        }
    }
}
