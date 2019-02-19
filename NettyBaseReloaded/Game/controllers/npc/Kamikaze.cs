using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.controllers.npc
{
    class Kamikaze : INpc
    {
        private NpcController Controller { get; set; }

        public Kamikaze(NpcController controller)
        {
            Controller = controller;
        }


        public void Tick()
        {
            try
            {
                var npc = Controller.Npc;
                if (npc.SelectedCharacter != null)
                {
                    if (npc.CurrentHealth < npc.MaxHealth * 0.2)
                    {
                        Active();
                    }
                    else Inactive();

                    if (!npc.SelectedCharacter.InRange(npc, 401))
                    {
                        if (!npc.InRange(npc.SelectedCharacter))
                        {
                            npc.Selected = null;
                            GameClient.SendToPlayerView(Controller.Npc,
                                LegacyModule.write("0|n|fx|end|RAGE|" + Controller.Npc.Id));
                            GameClient.SendToPlayerView(Controller.Npc,
                                LegacyModule.write("0|n|fx|end|RAGE|" + Controller.Npc.Id));
                        }
                        else
                            MovementController.Move(npc,
                                Vector.GetPosOnCircle(npc.SelectedCharacter.Position, 400));
                    }
                }
                else Paused();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private Stopwatch Stopwatch;
        public void Active()
        {
            if (Stopwatch == null || !Stopwatch.IsRunning)
            {
                Stopwatch = Stopwatch.StartNew();
            }

            GameClient.SendToPlayerView(Controller.Npc, LegacyModule.write("0|n|fx|start|RAGE|" + Controller.Npc.Id));
            GameClient.SendToPlayerView(Controller.Npc, LegacyModule.write("0|n|fx|start|RAGE|" + Controller.Npc.Id));

            if (Stopwatch.ElapsedMilliseconds == 5000)
            {
                Controller.Npc.Destroy();
                Exit();
            }
        }

        public void Inactive()
        {
            var npc = Controller.Npc;
            var attackers = Controller.Attack.GetActiveAttackers();
            if (attackers.Count > 0)
            {
                npc.Selected = attackers.OrderBy(x => x.Damage).FirstOrDefault();
            }

            Controller.Attack.Attacking = true;
        }

        public void Paused()
        {
            var npc = Controller.Npc;
            if (npc.Spacemap.Entities.Any(x => x.Value is Player))
            {
                // move around
                if (npc.Range.Entities.Any(x => x.Value is Player))
                {
                    // make a search
                    Player selectedPlayer = null;
                    foreach (var entity in npc.Range.Entities.Where(x => x.Value is Player))
                    {
                        var playerEntity = entity.Value as Player;
                        if (selectedPlayer == null) selectedPlayer = playerEntity;
                        else
                        {
                            if (selectedPlayer.Position.DistanceTo(entity.Value.Position) >
                                playerEntity.Position.DistanceTo(npc.Position))
                            {
                                selectedPlayer = playerEntity;
                            }
                        }
                    }

                    npc.Selected = selectedPlayer;
                    MovementController.Move(npc, selectedPlayer.Position);
                }
                else if (!npc.Moving)
                {
                    MovementController.Move(npc, Vector.Random(npc.Spacemap));
                }
            }
        }

        public void Exit()
        {
        }
    }
}
