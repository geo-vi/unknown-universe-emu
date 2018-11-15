using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world;
using System.Threading.Tasks;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Game.controllers
{
    class AbstractCharacterController : ITick
    {
        /// <summary>
        /// TICK ID
        /// </summary>
        private int TickId { get; set; }
        
        public Character Character { get; }

        public Checkers Checkers { get; }

        public Attack Attack { get; }

        public Damage Damage { get; }

        public Heal Heal { get; }

        public Destruction Destruction { get; }

        public Effects Effects { get; }

        public bool StopController { get; set; }

        public bool Active { get; set; }

        public AbstractCharacterController(Character character)
        {
            Character = character;

            Checkers = new Checkers(this);
            Attack = new Attack(this);
            Damage = new Damage(this);
            Heal = new Heal(this);
            Destruction = new Destruction(this);
            Effects = new Effects(this);

            StopController = false;

        }

        public virtual void Initiate()
        {
            Active = true;
            StopController = false;
            var id = -1;
            Global.TickManager.Add(this, out id);
            TickId = id;
        }

        public int GetId()
        {
            return TickId;
        }

        public void Tick()
        {
            if (StopController || Character.EntityState == EntityStates.DEAD)
            {
                StopAll();
                return;
            }

            TickClasses();

            if (this is PlayerController playerController)
            {
                playerController.Tick();
            }
            else if (this is NpcController npcController)
            {
                npcController.Tick(); 
            }
            else if (this is PetController petController)
            {
                petController.Tick();
            }
        }


        public void TickClasses()
        {
            Character.Position = MovementController.ActualPosition(Character);
            Checkers?.Tick();
            Attack?.Tick();
            Damage?.Tick();
            Heal?.Tick();
            Destruction?.Tick();
            Effects?.Tick();
        }

        public void StopAll()
        {
            StopController = true;
            Active = false;
            Checkers.Stop();
            Attack.Stop();
            Global.TickManager.Remove(this);
        }

    }
}
