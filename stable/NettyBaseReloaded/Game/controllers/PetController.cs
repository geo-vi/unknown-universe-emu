using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;

namespace NettyBaseReloaded.Game.controllers
{
    class PetController : AbstractCharacterController
    {
        private Pet Pet { get; }
        private Gears _gears { get; }

        public PetController(Character character) : base(character)
        {
            Pet = Character as Pet;
            _gears = new Gears();
        }

        void AddGears()
        {
            _gears.GearList.Add(new Gears.Guard(this));
        }

        public void Start()
        {
            AddGears();
            Global.TickManager.Tickables.Add(this);
        }

        public void Exit()
        {

        }

        public void Tick()
        {
            Checkers();
            Follow();
        }

        public void Activate()
        {
            Pet.Spacemap.Entities.Add(Pet.Id, Pet);
            Packet.Builder.PetStatusCommand(World.StorageManager.GetGameSession(Pet.GetOwner().Id), Pet);
            Console.WriteLine("PET {0} spawned at {1}", Pet.Id, Pet.Position);
            MovementController.Move(Pet, Vector.GetPosInCircle(Pet.GetOwner().Position, 250));
            Start();
        }

        public void DeActivate()
        {
            
        }

        public void Repair()
        {
            
        }

        private DateTime LastTimeMoved = new DateTime(2017, 3, 6, 0, 0,0);
        void Follow()
        {
            if (LastTimeMoved.AddMilliseconds(500) > DateTime.Now) return;
            if (Pet.GetOwner().Moving)
            {
                MovementController.Move(Pet, Pet.GetOwner().Position);
            }
            else if (Pet.Position.DistanceTo(Pet.GetOwner().Position) > 300)
            {
                MovementController.Move(Pet, Vector.GetPosInCircle(Pet.GetOwner().Position, 250));
            }
            LastTimeMoved = DateTime.Now;
        }

        public class Gears
        {
            public abstract class IGear : IChecker
            {
                public PetController baseController { get; }

                public IGear(PetController controller)
                {
                    baseController = controller;
                }

                public abstract void Activate();

                public abstract void Check();
            }
            
            public List<IGear> GearList = new List<IGear>();

            public class Guard : IGear
            {
                internal Guard(PetController controller) : base(controller) { }

                public override void Activate()
                {

                }

                public override void Check()
                {

                }
            }
        }
    }
}
