using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world;
using System.Threading.Tasks;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;

namespace NettyBaseReloaded.Game.controllers
{
    class AbstractCharacterController : ITick
    {
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
            Global.TickManager.Add(this);
        }

        public virtual void Tick()
        {
            if (StopController || Character.EntityState == EntityStates.DEAD)
            {
                StopAll();
                return;
            }

            TickClasses();

            if (this is PlayerController)
            {
                var player = (Player) Character;
                player.Controller.Tick();
            }
            else if (this is PetController)
            {
                var pet = (Pet) Character;
                pet.Controller.Tick();
            }
            else if (this is NpcController)
            {
                var npc = (Npc) Character;
                npc.Controller.Tick();
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
