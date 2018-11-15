using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.controllers.player;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using Range = NettyBaseReloaded.Game.controllers.player.Range;

namespace NettyBaseReloaded.Game.controllers
{
    class PlayerController : AbstractCharacterController
    {
        /// <summary>
        /// All checked classes (used for Ticker)
        /// </summary>
        public List<IChecker> CheckedClasses = new List<IChecker>();
        
        /// <summary>
        /// CPU Class
        /// There you can find all types
        /// of CPUs such as Cloak()
        /// </summary>
        public CPU CPUs { get; set; }

        /// <summary>
        /// Range Class
        /// Adding / Removing stuff from range such as
        /// Objects, Zones & so on
        /// </summary>
        public Range Ranges { get; set; }

        /// <summary>
        /// Miscs class
        /// All that wasn't listed above is basically here
        /// </summary>
        public Misc Miscs { get; set; }

        public Player Player { get; }

        public bool Repairing { get; set; }

        public bool Jumping { get; set; }

        public PlayerController(Character character) : base(character)
        {
            Player = Character as Player;
            Repairing = false;
            Jumping = false;
        }

        private void AddClasses()
        {
            try
            {
                CPUs = new CPU(this);
                CheckedClasses.Add(CPUs);
                Ranges = new Range(this);
                CheckedClasses.Add(Ranges);
                Miscs = new Misc(this);
                CheckedClasses.Add(Miscs);
            }
            catch (Exception e)
            {
            }
        }
        
        public void Setup()
        {
            if (CheckedClasses.Count == 0)
                AddClasses();    
        }

        public new void Tick()
        {
            try
            {
                foreach (var _class in CheckedClasses.ToList())
                {
                    _class.Check();
                }
            }
            catch (Exception e)
            {
            }
        }

        public void Exit()
        {
            StopAll();
            CheckedClasses.Clear();
            Jumping = false;
            Console.WriteLine("ticking? " + Global.TickManager.Exists(this));
        }
    }
}
