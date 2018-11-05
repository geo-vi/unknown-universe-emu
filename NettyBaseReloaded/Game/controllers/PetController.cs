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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packet = NettyBaseReloaded.Game.netty.Packet;
using PetGearTypeModule = NettyBaseReloaded.Game.netty.commands.old_client.PetGearTypeModule;

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
            if (Pet.GetOwner() == null) return;

            Global.TickManager.Add(Pet);
            Initiate();
            MovementController.Move(Pet, Vector.GetPosOnCircle(Pet.GetOwner().Position, 250));
        }

        private void LoadGears()
        {
            if (Pet.Gears.Count == 0)
            {
                Pet.Gears.Add(new PassiveGear(this));
                Pet.Gears.Add(new GuardGear(this));
                //Pet.Gears.Add(new AutoLootGear(this));
                //Pet.Gears.Add(new ComboRepairGear(this));
                //Pet.Gears.Add(new AutoResourceCollectionGear(this));
            }
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
            Destruction.Remove();

            Pet.BasicSave();
            StopController = true;
            Active = false;
            Global.TickManager.Remove(Pet);
            StopAll();
            if (Character.Moving)
                Character.SetPosition(MovementController.ActualPosition(Character));
            StopGears();

            var owner = Pet.GetOwner();
            if (owner != null && owner.GetGameSession() != null)
            {
                Packet.Builder.PetInitializationCommand(owner.GetGameSession(), Pet);
            }
        }

        public void StopGears()
        {
            foreach (var gear in Pet.Gears)
            {
                gear.End(true);
            }
        }
        
        public new void Tick()
        {
            if (Active || !StopController)
            {
                if (!Pet.HasFuel())
                {
                    Deactivate();
                    return;
                }
                Gear.Check();
            }
        }

        public void Activate()
        {
            if (Pet.Fuel <= 0) return;

            Character character;
            if (Pet.Spacemap.Entities.ContainsKey(Pet.Id))
                Pet.Spacemap.Entities.TryRemove(Pet.Id, out character);
            Pet.Spacemap = Pet.GetOwner().Spacemap;
            Pet.SetPosition(Pet.GetOwner().Position);

            if (!Pet.Spacemap.Entities.ContainsKey(Pet.Id))
                Pet.Spacemap.AddEntity(Pet);

            Start();
            var session = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            Packet.Builder.PetHeroActivationCommand(session, Pet);
            Packet.Builder.PetStatusCommand(session, Pet);
            LoadGears();
        }

        public void Deactivate()
        {
            Exit();

            var ownerSession = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            if (ownerSession != null)
                Packet.Builder.PetDeactivationCommand(ownerSession, Pet);
        }

        public void Destroy()
        {
            Exit();

            var ownerSession = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            if (ownerSession != null)
            {
                Packet.Builder.PetIsDestroyedCommand(ownerSession);
                Packet.Builder.PetUIRepairButtonCommand(ownerSession, true, 0);
            }
        }

        public void Repair()
        {
            Pet.CurrentHealth = 1000;
            Pet.EntityState = EntityStates.ALIVE;
            var ownerSession = World.StorageManager.GetGameSession(Pet.GetOwner().Id);
            if (ownerSession != null)
                Packet.Builder.PetRepairCompleteCommand(ownerSession);
            World.DatabaseManager.RepairPet(Pet);
        }

        public void SwitchGear(short gearType, int optParam)
        {
            if (Gear.Active)
                Gear.End();
            var gearIndex = Pet.Gears.FindIndex(x => (short) x.Type == gearType);
            Gear = Pet.Gears[gearIndex];
            Gear.Activate();
        }
    }
}
