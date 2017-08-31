using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Mothership : INpc
    {
        private NpcController Controller { get; set; }

        private bool Opened = false;

        public Mothership(NpcController controller)
        {
            Controller = controller;
        }

        public void Tick()
        {
            if (Opened) Paused();
            else Inactive();
        }

        private DateTime LastActiveTime = new DateTime();
        public void Active()
        {
            GameClient.SendRangePacket(Controller.Npc, netty.commands.old_client.LegacyModule.write("0|n|s|start|" + Controller.Npc.Id));
            Opened = true;

            for (int i = 0; i <= 20; i++)
            {
                var minionId = Controller.Npc.Spacemap.CreateNpc(World.StorageManager.Ships[81], AILevels.DAUGHTER, Controller.Npc);
                GameClient.SendRangePacket(Controller.Npc, netty.commands.old_client.NpcUndockCommand.write(Controller.Npc.Id, minionId));
            }
            LastActiveTime = DateTime.Now;
        }

        private Tuple<DateTime, Character> TickAttacked = null;
        public void Inactive()
        {
            if (Controller.Attacked)
            {
                if (TickAttacked == null)
                    TickAttacked = new Tuple<DateTime, Character>(DateTime.Now, Controller.GetAttacker());
                if (TickAttacked.Item1.AddSeconds(1) < DateTime.Now && Controller.Attacked)
                    Active();
            }
        }

        public void Paused()
        {
            if (LastActiveTime.AddSeconds(5) <= DateTime.Now)
                GameClient.SendRangePacket(Controller.Npc, netty.commands.old_client.LegacyModule.write("0|n|s|end|" + Controller.Npc.Id));
        }

        public void Exit()
        {
            Opened = false;
        }
    }
}
