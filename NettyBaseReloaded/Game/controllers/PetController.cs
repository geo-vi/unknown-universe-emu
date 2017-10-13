using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.controllers.pet.gears;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;

namespace NettyBaseReloaded.Game.controllers
{
    class PetController : AbstractCharacterController
    {
        public Pet Pet { get; }
        private Gear Gear { get; set; }

        public PetController(Character character) : base(character)
        {
            Pet = Character as Pet;
        }

        private void Start()
        {
            LoadGears();
            Initiate();
        }

        private void LoadGears()
        {
            Pet.Gears.Add(new Passive(this));
            Pet.Gears.Add(new Guard(this));
            Gear = Pet.Gears[0];
            var owner = Pet.GetOwner();
            var gameSession = World.StorageManager.GetGameSession(owner.Id);
            foreach (var gear in Pet.Gears)
            {
                Packet.Builder.PetGearAddCommand(gameSession, gear);
            }
            Packet.Builder.PetGearSelectCommand(gameSession, Gear);
        }

        public void Exit()
        {

        }

        public new void Tick()
        {
            foreach (var gear in Pet.Gears)
            {
                gear.Check();
            }
        }

        public void Activate()
        {
            // TODO Fix PetActivation & PetHero

            Pet.Position = Pet.GetOwner().Position;
            Pet.Spacemap = Pet.GetOwner().Spacemap;

            if (!Pet.Spacemap.Entities.ContainsKey(Pet.Id))
                Pet.Spacemap.Entities.Add(Pet.Id, Pet);

            if (!Pet.GetOwner().RangeEntities.ContainsKey(Pet.Id))
                Pet.GetOwner().RangeEntities.Add(Pet.Id, Pet);

            var session = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            Packet.Builder.PetHeroActivationCommand(session, Pet);
            Packet.Builder.PetStatusCommand(session, Pet);
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
    }
}
