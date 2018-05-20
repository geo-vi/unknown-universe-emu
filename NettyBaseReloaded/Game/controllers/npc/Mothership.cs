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

        public Ship DaughterType { get; }

        public int DaughterSpawnCount { get; set; }

        public Mothership(NpcController controller, Ship daughterType, int daughterSpawnCount = 20)
        {
            Controller = controller;
            DaughterType = daughterType;
            DaughterSpawnCount = daughterSpawnCount;
        }

        public void Tick()
        {
            if (Opened) Paused();
            else Inactive();
        }

        private DateTime LastActiveTime = new DateTime();
        public void Active()
        {
            GameClient.SendToPlayerView(Controller.Npc, netty.commands.old_client.LegacyModule.write("0|n|s|start|" + Controller.Npc.Id));
            GameClient.SendToPlayerView(Controller.Npc, netty.commands.new_client.LegacyModule.write("0|n|s|start|" + Controller.Npc.Id));
            Opened = true;

            for (int i = 0; i <= DaughterSpawnCount; i++)
            {
                var minionId = Controller.Npc.Spacemap.CreateNpc(DaughterType, AILevels.DAUGHTER, Controller.Npc);
                GameClient.SendToPlayerView(Controller.Npc, netty.commands.old_client.NpcUndockCommand.write(Controller.Npc.Id, minionId));
            }
            LastActiveTime = DateTime.Now;
        }

        private Tuple<DateTime, Character> TickAttacked = null;
        public void Inactive()
        {
            if (Controller.Character.LastCombatTime.AddMilliseconds(100) > DateTime.Now)
            {
                if (TickAttacked == null)
                    TickAttacked = new Tuple<DateTime, Character>(DateTime.Now, Controller.Npc.MotherShip.Controller.Attack.GetActiveAttackers().OrderBy(x => x.Damage).FirstOrDefault());
                if (TickAttacked.Item1.AddSeconds(1) < DateTime.Now)
                    Active();
            }
        }

        public void Paused()
        {
            if (LastActiveTime.AddSeconds(5) <= DateTime.Now)
            {
                GameClient.SendToPlayerView(Controller.Npc,
                    netty.commands.old_client.LegacyModule.write("0|n|s|end|" + Controller.Npc.Id));
                GameClient.SendToPlayerView(Controller.Npc, netty.commands.new_client.LegacyModule.write("0|n|s|end|" + Controller.Npc.Id));

            }
        }

        public void Exit()
        {
            Opened = false;
        }
    }
}
