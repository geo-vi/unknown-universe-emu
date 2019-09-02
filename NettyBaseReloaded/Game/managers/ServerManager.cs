using System;
using System.Linq;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Game.managers
{
    class ServerManager : ITick
    {

        /* Shutdown system */
        private DateTime TimeClosing { get; set; }
    
        private bool ShutdownInProcess { get; set; }

        private int LastNumberStreamed = -1;
        /*- End shutdown system -*/

        /* Npc routeine check */
        public DateTime LastRoutineCheck { get; set; }

        public void BeginServerCloseCountdown(int time = 15)
        {
            ShutdownInProcess = true;
            TimeClosing = DateTime.Now.AddSeconds(time);
            var timeLeft = (DateTime.Now - TimeClosing).TotalSeconds;
            Console.WriteLine("Server closing in... -= " + timeLeft + " =-");
            World.StreamMessageToWorld("Server closing in " + timeLeft);
            LastNumberStreamed = (int)timeLeft;
        }

        private void EntitiesRouteineCheck()
        {
            if (LastRoutineCheck.AddMinutes(2) < DateTime.Now)
            {
                foreach (var spacemap in World.StorageManager.Spacemaps.Values)
                {
                    var entities = spacemap.Entities;

                    var playersCount = entities.Count(x => x.Value is Player);
                    var npcCount = entities.Count(x => x.Value is Npc);

                    Console.WriteLine("Players count [" + playersCount + "] vs Npc count [" + npcCount + "]");

                    if (playersCount > 0)
                    {
                        spacemap.SpawnNpcs();
                    }

                    foreach (var entity in entities)
                    {
                        if (entity.Value is Player player)
                        {
                            if (player.CurrentHealth == 0 && player.EntityState != EntityStates.DEAD)
                            {
                                Console.WriteLine("Something is wrong with Player : " + player.Id + "; " + player.Name +
                                                  "; " + player.Position);
                            }
                            else if (player.Controller.Active)
                            {
                                if (player.Controller.StopController)
                                {
                                    Console.WriteLine("Something is wrong with Player's Controller : " + player.Id +
                                                      "; " + player.Name + "; " + player.Position);
                                }
                            }
                        }
                        else if (entity.Value is Npc npc)
                        {
                            if (npc.CurrentHealth == 0 && npc.EntityState != EntityStates.DEAD)
                            {
                                Console.WriteLine("Something is wrong with NPC : " + npc.Id + "; " + npc.Name + "; " +
                                                  npc.Position);
                            }
                            else if (npc.Controller.Active)
                            {
                                if (npc.Controller.StopController)
                                {
                                    Console.WriteLine("Something is wrong with NPC's Controller : " + npc.Id + "; " +
                                                      npc.Name + "; " + npc.Position);
                                }
                            }
                        }
                        else if (entity.Value is Pet pet)
                        {
                            //none.
                        }
                    }
                }
                LastRoutineCheck = DateTime.Now;
            }
        }

        private void CollectableRouteineCheck()
        {

        }

        private void ResourcesRouteineCheck()
        {

        }

        public void Tick()
        {
            if (ShutdownInProcess)
            {
                var timeLeft = (DateTime.Now - TimeClosing).TotalSeconds;

                if (LastNumberStreamed > timeLeft)
                {
                    Console.WriteLine("Server closing in... -= " + timeLeft + " =-");
                    World.StreamMessageToWorld("Server closing in " + timeLeft);
                }

                if (timeLeft <= 0)
                {
                    Program.Exit();
                }
            }

            //EntitiesRouteineCheck();
            //CollectableRouteineCheck();
            //ResourcesRouteineCheck();
        }

        public void Start()
        {
            Global.TickManager.Add(this, out _);
        }

        public int GetId() => -1;
    }
}
