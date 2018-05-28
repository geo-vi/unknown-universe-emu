using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.controllers
{
    class AbstractCharacterController
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
            Task.Factory.StartNew(Tick);
        }

        public async void Tick()
        {
            while (Active)
            {
                if (StopController || Character.EntityState == EntityStates.DEAD)
                {
                    StopAll();
                    return;
                }

                await Task.Factory.StartNew(TickClasses);

                if (this is PlayerController)
                {
                    var player = (Player)Character;
                    await Task.Factory.StartNew(player.Controller.Tick);
                }
                else if (this is PetController)
                {
                    var pet = (Pet) Character;
                    await Task.Factory.StartNew(pet.Controller.Tick);
                }
                await Task.Delay(50);
            }
        }

        public void TickClasses()
        {
            Character.Position = MovementController.ActualPosition(Character);
            Checkers?.Tick();
            Parallel.Invoke(() =>
            {
                Attack?.Tick();
                Damage?.Tick();
                Heal?.Tick();
                Destruction?.Tick();
                Effects?.Tick();
            });
        }

        public void StopAll()
        {
            StopController = true;
            Active = false;
            Checkers.Stop();
            Attack.Stop();
        }

    }
}
