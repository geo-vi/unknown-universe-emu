using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.controllers.pet.gears;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;

namespace NettyBaseReloaded.Game.controllers
{
    class PetController : AbstractCharacterController
    {
        private Pet Pet { get; }
        private List<IGear> Gears { get; }

        public PetController(Character character) : base(character)
        {
            Pet = Character as Pet;
            Gears = new List<IGear>();
        }

        void AddGears()
        {
            Gears.Add(new Guard(this));
        }

        public void Start()
        {
            AddGears();
        }

        public void Exit()
        {

        }

        public new void Tick()
        {
            Follow();
        }

        public void Activate()
        {
            Pet.Spacemap.Entities.Add(Pet.Id, Pet);
            Packet.Builder.PetStatusCommand(World.StorageManager.GetGameSession(Pet.GetOwner().Id), Pet);
            Console.WriteLine("PET {0} spawned at {1}", Pet.Id, Pet.Position);
            MovementController.Move(Pet, Vector.GetPosOnCircle(Pet.GetOwner().Position, 250));
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
                MovementController.Move(Pet, Vector.GetPosOnCircle(Pet.GetOwner().Position, 250));
            }
            LastTimeMoved = DateTime.Now;
        }
    }
}
