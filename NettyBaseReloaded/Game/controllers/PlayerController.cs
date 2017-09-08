using System.Collections.Generic;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.controllers.player;
using NettyBaseReloaded.Game.objects.world;
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
            CPUs = new CPU(this);
            CheckedClasses.Add(CPUs);
            Ranges = new Range(this);
            CheckedClasses.Add(Ranges);
            Miscs = new Misc(this);
            CheckedClasses.Add(Miscs);
        }

        public void Start()
        {
            AddClasses();    
        }

        public new void Tick()
        {
            foreach (var _class in CheckedClasses)
            {
                _class.Check();
            }
        }
    }
}
