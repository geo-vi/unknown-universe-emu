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

        protected Tech(Player player) : base(player) { }

        public int TimeLeft => Math.Abs((TimeFinish - DateTime.Now).Seconds);
        
        public abstract void Tick();

        public abstract void execute();

        public virtual void ThreadUpdate() { }

        private Task TechTask;

        public void Start()
        {
            var session = Player.GetGameSession();
            Packet.Builder.LegacyModule(session, "0|A|STD|Techs disabled temporarily");
            //if (TechTask != null && !TechTask.IsCompleted) return;
            //TechTask = Task.Factory.StartNew(() =>
            //{
            //    while (Active && Player.Controller.Active)
            //    {
            //        System.Threading.Thread.Sleep(1000);
            //        ThreadUpdate();
            //    }
            //});
        }

        public int GetStatus()
        {
            if (Active) return 2;
            if (Enabled) return 1;
            return 0;
        }
    }
}
