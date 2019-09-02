using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.npcs;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Mothership : INpc
    {
        private Cubikon Mother;

        private NpcController Controller { get; set; }

        private bool Opened = false;

        public Ship DaughterType { get; }

        public int DaughterSpawnCount { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="daughterType"></param>
        /// <param name="daughterSpawnCount"></param>
        public Mothership(NpcController controller, Ship daughterType, int daughterSpawnCount = 20)
        {
            Controller = controller;
            Mother = controller.Npc as Cubikon;
            DaughterType = daughterType;
            DaughterSpawnCount = daughterSpawnCount;
        }

        /// <summary>
        /// Will jump to Inactive if not interacted for more than 2 seconds
        /// </summary>
        public void Tick()
        {
            if (Opened) Paused();
            else Inactive();
        }

        private DateTime LastActiveTime = new DateTime();

        public void Active()
        {
            var daughtersAlive = GetActiveDaughtersCount();
            if (daughtersAlive >= 20)
            {
                return;
            }

            GameClient.SendToPlayerView(Controller.Npc,
                netty.commands.old_client.LegacyModule.write("0|n|s|start|" + Controller.Npc.Id), true);
            GameClient.SendToPlayerView(Controller.Npc,
                netty.commands.new_client.LegacyModule.write("0|n|s|start|" + Controller.Npc.Id), true);
            Opened = true;

            for (int i = 0; i < 20 - daughtersAlive; i++)
            {
                var minionId = Controller.Npc.Spacemap.CreateNpc(DaughterType, AILevels.DAUGHTER, Controller.Npc);
                Mother.Children.TryAdd(minionId, Mother.Spacemap.Entities[minionId] as Npc);
                GameClient.SendToPlayerView(Controller.Npc,
                    netty.commands.old_client.NpcUndockCommand.write(Controller.Npc.Id, minionId), true);

            }

            LastActiveTime = DateTime.Now;
        }

        private Tuple<DateTime, Character> TickAttacked = null;
        public void Inactive()
        {
            if (Controller.Character.LastCombatTime.AddMilliseconds(2000) > DateTime.Now)
            {
                if (TickAttacked == null)
                    TickAttacked = new Tuple<DateTime, Character>(DateTime.Now, Controller.Attack.GetActiveAttackers().OrderBy(x => x.Damage).FirstOrDefault());
                if (TickAttacked.Item1.AddSeconds(2) < DateTime.Now)
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
                Opened = false;
            }

            if (Controller.Npc.LastCombatTime > DateTime.Now.AddSeconds(10))
            {
                Opened = false;
                if (Controller.Npc is Cubikon cube)
                {
                    foreach (var child in cube.Children)
                    {
                        child.Value.Controller.ExitAI();
                    }
                }
            }
        }

        public void Exit()
        {
            Opened = false;
        }

        public int GetActiveDaughtersCount()
        {
            if (Mother?.Children != null)
            {
                return Mother.Children.Count(x => x.Value.Controller.Active);
            }

            return 0;
        }
    }
}
