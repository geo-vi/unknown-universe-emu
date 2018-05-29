using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.controllers.pet;
using NettyBaseReloaded.Game.controllers.pet.gears;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetGearTypeModule = NettyBaseReloaded.Game.netty.commands.old_client.PetGearTypeModule;

namespace NettyBaseReloaded.Game.controllers
{
    class PetController : AbstractCharacterController
    {
        public Pet Pet { get; }
        private Gear Gear { get; set; }

        internal event EventHandler Shutdown;

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
            Pet.Gears.Add(new PassiveGear(this));
            Pet.Gears.Add(new GuardGear(this));
            Pet.Gears.Add(new AutoLootGear(this));
            Pet.Gears.Add(new ComboRepairGear(this));
            Pet.Gears.Add(new AutoResourceCollectionGear(this));
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
            Global.TickManager.Remove(Pet);
            StopAll();
            Checkers.Stop();
            Pet.Gears.Clear();
            Gear = null;
            Shutdown?.Invoke(this, EventArgs.Empty);
        }

        public new void Tick()
        {
            Gear.Check();
        }

        public void Activate()
        {
            Pet.Position = Pet.GetOwner().Position;
            Pet.Spacemap = Pet.GetOwner().Spacemap;

            if (!Pet.Spacemap.Entities.ContainsKey(Pet.Id))
                Pet.Spacemap.AddEntity(Pet);

            if (!Pet.GetOwner().Range.Entities.ContainsKey(Pet.Id))
                Pet.GetOwner().Range.AddEntity(Pet);

            Global.TickManager.Add(Pet);

            var session = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            Packet.Builder.PetHeroActivationCommand(session, Pet);
            Packet.Builder.PetStatusCommand(session, Pet);
            MovementController.Move(Pet, Vector.GetPosOnCircle(Pet.GetOwner().Position, 250));
            Start();
        }

        public void Deactivate()
        {
            var ownerSession = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            Packet.Builder.PetDeactivationCommand(ownerSession, Pet);
            Exit();
            Destruction.Remove();
        }

        public void Repair()
        {
            
        }

        public void SwitchGear(short gearType, int optParam)
        {
            Gear.End();
            var gearIndex = Pet.Gears.FindIndex(x => (short) x.Type == gearType);
            Gear = Pet.Gears[gearIndex];
            Gear.Activate();
        }
    }
}
